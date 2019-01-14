# Week3---3D-Game-Programming
Week3 - 3D-Game-Programming

## 1、操作与总结
 - 参考 Fantasy Skybox FREE 构建自己的游戏场景
见文件夹《自己的游戏场景》
 - 写一个简单的总结，总结游戏对象的使用

> 
所有游戏对象都有Active属性，Name属性，Tag属性等。每个游戏对象 (GameObject) 还包含一个变换transform组件。我们可以通过这个组件来使游戏对象改变位置，旋转和缩放。我们还可以添加许许多多不同的组件或脚本来增加游戏对象的功能。
> 
常用的游戏对象分为
**Empty（空对象）
Camera（摄像机）
Light（光线）
3D物体
Audio（声音））**
> 空对象（Empty）：不显示却是最常用的对象之一
> 摄像机（Camara）：观察游戏世界的窗口
> 光线（Light）：游戏世界的光源，让游戏世界富有魅力 
> 3D物体：3D游戏中的重要组成部分，可以改变其网格和材质，三角网格是游戏物体表面的唯一形式 
> 声音（Audio）：3D游戏中的声音来源

## 2、编程实践
 - 牧师与魔鬼 动作分离版
具体代码见文件夹《恶魔与牧师 动作分离版》

 1. 先贴上一张老师讲的思路图，也是结构图
![这里写图片描述](https://img-blog.csdn.net/20180410192525547?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2phbmtpbmdtZWFuaW5n/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
主要的思路呢，就是删掉之前的Move部分，然后以图示的思路实现新的Move方法
 2. 然后在第二周的代码基础上作适当修改
	 - BaseCode

先把Moveable类给删了，然后用新的结构实现Move
```
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
```
这就是动作分离版的实现move的一系列代码了

```
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
```

还有要注意，在其它的类里面的有关Move的行为都要改写,不然会提示找不到Moveable类
  - FirstController
  
FirstController里面其实也是根据BaseCode来变化的，改动的比较分散，下面举个点击物体的事件处理的例子
```
	public void characterIsClicked(MyCharacterController characterCtrl) {
		if (ifGameover)
			return;
		if (characterCtrl.isOnBoat ()) {
			CoastController whichCoast;
			if (boat.get_to_or_from () == -1) { // to->-1; from->1
				whichCoast = toCoast;
			} else {
				whichCoast = fromCoast;
			}

			boat.GetOffBoat (characterCtrl.getName());
			//characterCtrl.moveToPosition (whichCoast.getEmptyPosition ());

			actionManager.moveCharacter(characterCtrl.getGameObject(), whichCoast.getEmptyPosition());//新加代码
			characterCtrl.getOnCoast (whichCoast);
			whichCoast.getOnCoast (characterCtrl);

		} else {			
			CoastController whichCoast = characterCtrl.getCoastController ();

			if (boat.getEmptyIndex () == -1) {		
				return;
			}

			if (whichCoast.get_to_or_from () != boat.get_to_or_from ())
				return;

			whichCoast.getOffCoast(characterCtrl.getName());
			//characterCtrl.moveToPosition (boat.getEmptyPosition());

			actionManager.moveCharacter(characterCtrl.getGameObject(), boat.getEmptyPosition());//新加代码
			characterCtrl.getOnBoat (boat);
			boat.GetOnBoat (characterCtrl);
		}
		userGUI.status = check_game_over ();
		ifGameover = userGUI.status != 0 ? true : false;
	}
```
