using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Mygame;

namespace Com.Mygame {

	public class Director : System.Object {
		private static Director _instance;
		public SceneController currentSceneController { get; set; }

		public static Director getInstance() {
			if (_instance == null) {
				_instance = new Director ();
			}
			return _instance;
		}
	}

	public interface SceneController {
		void loadResources ();
	}

	public interface UserAction {
		void moveBoat();
		void characterIsClicked(MyCharacterController characterCtrl);
		void restart();
	}
	/*	
	public class Moveable: MonoBehaviour {

		readonly float move_speed = 20;

		int moving_status;
		Vector3 dest;
		Vector3 middle;
		void Update() {
			if (moving_status == 1) {
				transform.position = Vector3.MoveTowards (transform.position, middle, move_speed * Time.deltaTime);
				if (transform.position == middle) {
					moving_status = 2;
				}
			} else if (moving_status == 2) {
				transform.position = Vector3.MoveTowards (transform.position, dest, move_speed * Time.deltaTime);
				if (transform.position == dest) {
					moving_status = 0;
				}
			}
		}
		public void setDestination(Vector3 _dest) {
			dest = _dest;
			middle = _dest;
			if (_dest.y == transform.position.y) {
				moving_status = 2;
			}
			else if (_dest.y < transform.position.y) {	
				middle.y = transform.position.y;
			} else {							
				middle.x = transform.position.x;
			}
			moving_status = 1;
		}
		public void reset() {
			moving_status = 0;
		}
	}
	*/
	//code
	public enum SSActionEventType: int { Started, Completed };

	//动作事件回调接口
	public interface ISSActionCallBack
	{
		void SSActionEvent(SSAction sourse, SSActionEventType events = SSActionEventType.Completed);
	}


	public class SSAction: ScriptableObject
	{
		public bool enabled = true;
		public bool destory = false;

		public GameObject gameObject { get; set; }
		public Transform transform { get; set; }
		public ISSActionCallBack callback { get; set; }

		protected SSAction() { }

		public virtual void Start() { throw new System.NotImplementedException(); }

		public virtual void Update() { throw new System.NotImplementedException(); }
	}


	public class CCCharacterMove : SSAction
	{
		private Vector3 target;
		private Vector3 mid;
		int status = 0; // 0 for not moving; 1 for moving to middle; 2 for moving to dest
		public float speed = 20;

		public static CCCharacterMove GetSSAction(Vector3 target)
		{
			CCCharacterMove action = ScriptableObject.CreateInstance<CCCharacterMove>();
			action.target = target;
			action.mid = target;
			return action;
		}

		public override void Start()
		{
			Debug.Log("人物开始移动");
			if (target.y == transform.position.y)
			{
				status = 2;
			}
			else if (target.y < transform.position.y)
			{
				mid.y = transform.position.y;
			}
			else
			{
				mid.x = transform.position.x;
			}

			status = 1;
		}

		public override void Update()
		{
			if (status == 1)
			{
				transform.position = Vector3.MoveTowards(transform.position, mid, speed * Time.deltaTime);
				if (transform.position == mid)
				{
					status = 2;
				}
			}
			else if (status == 2)
			{
				transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
				if (transform.position == target)
				{
					status = 0;
					Debug.Log("人物移动结束");
					this.destory = true;
					this.callback.SSActionEvent(this);
				}
			}
		}
	}


	public class SSActionManager: MonoBehaviour
	{
		private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
		private List<SSAction> waitingAdd = new List<SSAction>();
		private List<int> waitingDelete = new List<int>();

		protected void Update()
		{
			foreach (SSAction ac in waitingAdd) actions[ac.GetInstanceID()] = ac;
			waitingAdd.Clear();
			foreach(KeyValuePair<int, SSAction> kv in actions)
			{
				SSAction ac = kv.Value;
				if (ac.destory)
				{
					waitingDelete.Add(ac.GetInstanceID());
				}else if(ac.enabled){
					ac.Update();
				}
			}

			foreach (int key in waitingDelete)
			{
				SSAction ac = actions[key];
				actions.Remove(key);
				DestroyObject(ac);
			}
			waitingDelete.Clear();
		}

		protected void Start() { }

		public void RunAction(GameObject gameObject, SSAction action, ISSActionCallBack manager)
		{
			action.gameObject = gameObject;
			action.callback = manager;
			action.transform = gameObject.transform;
			waitingAdd.Add(action);
			action.Start();
		}
	}

	public class CCActionManager: SSActionManager, ISSActionCallBack
	{
		public FirstController sceneController;

		public void moveBoat(GameObject boat, int to_or_from)
		{
			if (to_or_from == -1)
			{
				Debug.Log("船开始向右移");
				CCBoatMove boatMoveToRight = CCBoatMove.GetSSAction(new Vector3(5, 1, 0));
				this.RunAction(boat, boatMoveToRight, this);
			}
			else
			{
				Debug.Log("船开始向左移");
				CCBoatMove boatMoveToLeft = CCBoatMove.GetSSAction(new Vector3(-5, 1, 0));
				this.RunAction(boat, boatMoveToLeft, this);
			}
		}

		public void moveCharacter(GameObject obj, Vector3 dest) {
			CCCharacterMove characterMove = CCCharacterMove.GetSSAction(dest);
			this.RunAction(obj, characterMove, this);
		}

		protected new void Start()
		{
			sceneController = (FirstController)Director.getInstance().currentSceneController;
			sceneController.actionManager = this;
		}

		protected new void Update()
		{
			base.Update();
		}

		public void SSActionEvent(SSAction sourse, SSActionEventType events = SSActionEventType.Completed)
		{
			Debug.Log("Completed!");
		}
	}

	public class CCBoatMove : SSAction
	{
		public Vector3 target;
		public float speed = 20;

		public static CCBoatMove GetSSAction(Vector3 target)
		{
			CCBoatMove action = ScriptableObject.CreateInstance<CCBoatMove>();
			action.target = target;
			return action;
		}

		public override void Start()
		{
			Debug.Log("船开始移动");
			this.transform.position = Vector3.MoveTowards(this.transform.position, this.target, this.speed * Time.deltaTime);
		}

		public override void Update() {
			if (this.transform.position == target)
			{
				Debug.Log("移动结束");
				this.destory = true;
				this.callback.SSActionEvent(this);
			}
			else
			{
				this.transform.position = Vector3.MoveTowards(this.transform.position, this.target, this.speed * Time.deltaTime);
			}
		}
	}


	//new code
		
	public class MyCharacterController {
		readonly GameObject character;
		//readonly Moveable moveableScript;
		readonly ClickGUI clickGUI;
		readonly int characterType;	

		bool _isOnBoat;
		CoastController coastController;


		public MyCharacterController(string which_character) {

			if (which_character == "priest") {
				character = Object.Instantiate (Resources.Load ("Perfabs/Priest", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
				characterType = 0;
			} else {
				character = Object.Instantiate (Resources.Load ("Perfabs/Devil", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
				characterType = 1;
			}
			//moveableScript = character.AddComponent (typeof(Moveable)) as Moveable;

			clickGUI = character.AddComponent (typeof(ClickGUI)) as ClickGUI;
			clickGUI.setController (this);
		}

		public void setName(string name) {
			character.name = name;
		}
		public GameObject getGameObject()
		{
			return character;
		}


		public void setPosition(Vector3 pos) {
			character.transform.position = pos;
		}

		//public void moveToPosition(Vector3 destination) {
		//	moveableScript.setDestination(destination);
		//}

		public int getType() {
			return characterType;
		}

		public string getName() {
			return character.name;
		}

		public void getOnBoat(BoatController boatCtrl) {
			coastController = null;
			character.transform.parent = boatCtrl.getGameobj().transform;
			_isOnBoat = true;
		}

		public void getOnCoast(CoastController coastCtrl) {
			coastController = coastCtrl;
			character.transform.parent = null;
			_isOnBoat = false;
		}

		public bool isOnBoat() {
			return _isOnBoat;
		}

		public CoastController getCoastController() {
			return coastController;
		}

		public void reset() {
			//moveableScript.reset ();
			coastController = (Director.getInstance ().currentSceneController as FirstController).fromCoast;
			getOnCoast (coastController);
			setPosition (coastController.getEmptyPosition ());
			coastController.getOnCoast (this);
		}
	}


	public class CoastController {
		readonly GameObject coast;
		readonly Vector3 from_pos = new Vector3(9,1,0);
		readonly Vector3 to_pos = new Vector3(-9,1,0);
		readonly Vector3[] positions;
		readonly int to_or_from;

		MyCharacterController[] passengerPlaner;

		public CoastController(string _to_or_from) {
			positions = new Vector3[] {new Vector3(6.5F,2.25F,0), new Vector3(7.5F,2.25F,0), new Vector3(8.5F,2.25F,0), 
				new Vector3(9.5F,2.25F,0), new Vector3(10.5F,2.25F,0), new Vector3(11.5F,2.25F,0)};

			passengerPlaner = new MyCharacterController[6];

			if (_to_or_from == "from") {
				coast = Object.Instantiate (Resources.Load ("Perfabs/Stone", typeof(GameObject)), from_pos, Quaternion.identity, null) as GameObject;
				coast.name = "from";
				to_or_from = 1;
			} else {
				coast = Object.Instantiate (Resources.Load ("Perfabs/Stone", typeof(GameObject)), to_pos, Quaternion.identity, null) as GameObject;
				coast.name = "to";
				to_or_from = -1;
			}
		}

		public int getEmptyIndex() {
			for (int i = 0; i < passengerPlaner.Length; i++) {
				if (passengerPlaner [i] == null) {
					return i;
				}
			}
			return -1;
		}

		public Vector3 getEmptyPosition() {
			Vector3 pos = positions [getEmptyIndex ()];
			pos.x *= to_or_from;
			return pos;
		}

		public void getOnCoast(MyCharacterController characterCtrl) {
			int index = getEmptyIndex ();
			passengerPlaner [index] = characterCtrl;
		}

		public MyCharacterController getOffCoast(string passenger_name) {
			for (int i = 0; i < passengerPlaner.Length; i++) {
				if (passengerPlaner [i] != null && passengerPlaner [i].getName () == passenger_name) {
					MyCharacterController charactorCtrl = passengerPlaner [i];
					passengerPlaner [i] = null;
					return charactorCtrl;
				}
			}
			Debug.Log ("cant find passenger on coast: " + passenger_name);
			return null;
		}

		public int get_to_or_from() {
			return to_or_from;
		}

		public int[] getCharacterNum() {
			int[] count = {0, 0};
			for (int i = 0; i < passengerPlaner.Length; i++) {
				if (passengerPlaner [i] == null)
					continue;
				if (passengerPlaner [i].getType () == 0) {	
					count[0]++;
				} else {
					count[1]++;
				}
			}
			return count;
		}

		public void reset() {
			passengerPlaner = new MyCharacterController[6];
		}
	}

	public class BoatController {
		readonly GameObject boat;
		//readonly Moveable moveableScript;
		readonly Vector3 fromPosition = new Vector3 (5, 1, 0);
		readonly Vector3 toPosition = new Vector3 (-5, 1, 0);
		readonly Vector3[] from_positions;
		readonly Vector3[] to_positions;

		int to_or_from; 
		MyCharacterController[] passenger = new MyCharacterController[2];

		public BoatController() {
			to_or_from = 1;

			from_positions = new Vector3[] { new Vector3 (4.5F, 1.5F, 0), new Vector3 (5.5F, 1.5F, 0) };
			to_positions = new Vector3[] { new Vector3 (-5.5F, 1.5F, 0), new Vector3 (-4.5F, 1.5F, 0) };

			boat = Object.Instantiate (Resources.Load ("Perfabs/Boat", typeof(GameObject)), fromPosition, Quaternion.identity, null) as GameObject;
			boat.name = "boat";

			//moveableScript = boat.AddComponent (typeof(Moveable)) as Moveable;
			boat.AddComponent (typeof(ClickGUI));
		}

		/*
		public void Move() {
			if (to_or_from == -1) {
				moveableScript.setDestination(fromPosition);
				to_or_from = 1;
			} else {
				moveableScript.setDestination(toPosition);
				to_or_from = -1;
			}
		}*/

		public int getEmptyIndex() {
			for (int i = 0; i < passenger.Length; i++) {
				if (passenger [i] == null) {
					return i;
				}
			}
			return -1;
		}

		public bool isEmpty() {
			for (int i = 0; i < passenger.Length; i++) {
				if (passenger [i] != null) {
					return false;
				}
			}
			return true;
		}

		public Vector3 getEmptyPosition() {
			Vector3 pos;
			int emptyIndex = getEmptyIndex ();
			if (to_or_from == -1) {
				pos = to_positions[emptyIndex];
			} else {
				pos = from_positions[emptyIndex];
			}
			return pos;
		}

		public void GetOnBoat(MyCharacterController characterCtrl) {
			int index = getEmptyIndex ();
			passenger [index] = characterCtrl;
		}

		public MyCharacterController GetOffBoat(string passenger_name) {
			for (int i = 0; i < passenger.Length; i++) {
				if (passenger [i] != null && passenger [i].getName () == passenger_name) {
					MyCharacterController charactorCtrl = passenger [i];
					passenger [i] = null;
					return charactorCtrl;
				}
			}
			Debug.Log ("Cant find passenger in boat: " + passenger_name);
			return null;
		}

		public GameObject getGameobj() {
			return boat;
		}

		public int get_to_or_from() { 
			return to_or_from;
		}

		public int[] getCharacterNum() {
			int[] count = {0, 0};
			for (int i = 0; i < passenger.Length; i++) {
				if (passenger [i] == null)
					continue;
				if (passenger [i].getType () == 0) {	
					count[0]++;
				} else {
					count[1]++;
				}
			}
			return count;
		}

		public void reset() {
			//moveableScript.reset ();
			if (to_or_from == -1) {
				//Move ();
				var actionManager = ((FirstController)Director.getInstance().currentSceneController).actionManager;
				actionManager.moveBoat(boat, to_or_from);
				to_or_from = 1;
			}
			passenger = new MyCharacterController[2];
		}
		public void changeToOrFrom()
		{
			if (to_or_from == -1) to_or_from = 1;
			else to_or_from = -1;
		}
	}


}