using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { WIN, LOSE, PAUSE, CONTINUE, START };

public interface ISceneController
{
	State state { get; set; }
	void LoadResources();
	void Pause();
	void Resume();
	void Restart();
}

public class RoundController : MonoBehaviour, IUserAction, ISceneController {

	public DiskFactory diskFactory;
	public RoundActionManager actionManager;
	public ScoreRecorder scoreRecorder;
	private List<GameObject> disks;
	private GameObject shootAtSth;
	GameObject explosion;

	public State state { get; set; }

	public int leaveSeconds;


	public int count;

	IEnumerator DoCountDown()
	{
		while (leaveSeconds >= 0)
		{
			yield return new WaitForSeconds(1);
			leaveSeconds--;
		}
	}

	void Awake()
	{
		SSDirector director = SSDirector.getInstance();
		director.setFPS(100);
		director.currentScenceController = this;

		LoadResources();

		diskFactory = Singleton<DiskFactory>.Instance;
		scoreRecorder = Singleton<ScoreRecorder>.Instance;
		actionManager = Singleton<RoundActionManager>.Instance;

		leaveSeconds = 60;
		count = leaveSeconds;

		state = State.PAUSE;

		disks = new List<GameObject>();
	}


	void Start () {
		//LoadResources();
	}

	void Update()
	{
		LaunchDisk();
		Judge();
		RecycleDisk();
	}

	public void LoadResources()
	{
		Camera.main.transform.position = new Vector3(0, 0, -15);
		explosion = Instantiate(Resources.Load("Prefabs/ParticleSys"), new Vector3(-40, 0, 0), Quaternion.identity) as GameObject;


	}

	public void shoot()
	{
		if (Input.GetMouseButtonDown(0) && (state == State.START || state == State.CONTINUE))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				if ((SSDirector.getInstance().currentScenceController.state == State.START || SSDirector.getInstance().currentScenceController.state == State.CONTINUE))
				{
					shootAtSth = hit.transform.gameObject;

					explosion.transform.position = hit.collider.gameObject.transform.position;
					explosion.GetComponent<Renderer>().material = hit.collider.gameObject.GetComponent<Renderer>().material;
					explosion.GetComponent<ParticleSystem>().Play();
				}
			}
		}
	}

	public void LaunchDisk()
	{
		if(count - leaveSeconds == 1)
		{
			count = leaveSeconds;
			GameObject disk = diskFactory.GetDisk();
			Debug.Log(disk);
			disks.Add(disk);
			actionManager.addRandomAction(disk);
		}
	}

	public void RecycleDisk()
	{
		for(int i = 0; i < disks.Count; i++)
		{
			if( disks[i].transform.position.z < -18)
			{
				diskFactory.FreeDisk(disks[i]);
				disks.Remove(disks[i]);
			}
		}
	}



	public void Judge()
	{
		if(shootAtSth != null && shootAtSth.transform.tag == "Disk" && shootAtSth.activeInHierarchy)
		{
			scoreRecorder.Record(shootAtSth);
			diskFactory.FreeDisk(shootAtSth);
			shootAtSth = null;
		}

		if (scoreRecorder.getScore() >= 1000)
		{
			StopAllCoroutines();
			state = State.WIN;
		}

		else if (leaveSeconds == 0)
		{
			StopAllCoroutines();
			state = State.LOSE;
		} 
		else
			state = State.CONTINUE;

	}

	public void Pause()
	{
		state = State.PAUSE;
		StopAllCoroutines();
		for (int i = 0; i < disks.Count; i++)
		{
			disks[i].SetActive(false);
		}
	}

	public void Resume()
	{
		StartCoroutine(DoCountDown());       
		state = State.CONTINUE;
		for (int i = 0; i < disks.Count; i++)
		{
			disks[i].SetActive(true);
		}
	}

	public void Restart()
	{
		scoreRecorder.Reset();
		Application.LoadLevel(Application.loadedLevelName);
		SSDirector.getInstance().currentScenceController.state = State.START;
	}

}