using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUserAction
{
	void shoot();//射击动作
}

public class UserGUI : MonoBehaviour
{
	private IUserAction action;
	private float width, height;
	private string countDownTitle;

	void Start()
	{
		countDownTitle = "开始";
		action = SSDirector.getInstance().currentScenceController as IUserAction;
	}

	float castw(float scale)
	{
		return (Screen.width - width) / scale;
	}

	float casth(float scale)
	{
		return (Screen.height - height) / scale;
	}

	void OnGUI()
	{
		width = Screen.width / 12;
		height = Screen.height / 12;


		GUI.Label(new Rect(Screen.width / 2 - 50, 80, 100, 100), "剩余时间：" + ((RoundController)SSDirector.getInstance().currentScenceController).leaveSeconds.ToString() + "秒" );

		GUI.Button(new Rect(Screen.width - 90, 10, 80, 30), "得分：" + ((RoundController)SSDirector.getInstance().currentScenceController).scoreRecorder.getScore().ToString() );

		if (SSDirector.getInstance().currentScenceController.state != State.WIN && SSDirector.getInstance().currentScenceController.state != State.LOSE
			&& GUI.Button(new Rect(10, 10, 80, 30), countDownTitle))
		{

			if (countDownTitle == "开始")
			{

				countDownTitle = "暂停";
				SSDirector.getInstance().currentScenceController.Resume();
			}
			else
			{

				countDownTitle = "开始";
				SSDirector.getInstance().currentScenceController.Pause();
			}
		}

		if (SSDirector.getInstance().currentScenceController.state == State.WIN)
		{
			if (GUI.Button(new Rect(castw(2f), casth(6f), width, height), "胜利!"))
			{

				SSDirector.getInstance().currentScenceController.Restart();
			}
		}
		else if (SSDirector.getInstance().currentScenceController.state == State.LOSE)
		{
			if (GUI.Button(new Rect(castw(2f), casth(6f), width, height), "失败!"))
			{
				SSDirector.getInstance().currentScenceController.Restart();
			}
		}
	}

	void Update()
	{

		action.shoot();
	}

}