# 3D Game Programing

-  解释 游戏对象（GameObjects） 和 资源（Assets）的区别与联系。
- 下载几个游戏案例，分别总结资源、对象组织的结构（指资源的目录组织结构与游戏对象树的层次结构）
**答：在显式支持面向对象的语言中，”对象”一般是指类在内存中装载的实例，具有相关的成员变量和成员函数(也称为方法)。对象一般都是直接体现于游戏的场景中，整合了各种游戏的资源。对象一般指游戏中玩家、队友、敌人、周围生物、环境事物、摄像机和音乐等虚拟父类，这些父节点本身没有实体，但它们的子类真正包含了游戏中会出现的对象。而资源文件夹通常有对象、材质、场景、声音、预设、贴图、脚本、动作，在这些文件夹下可以继续进行划分。资源可以被多个对象使用。有些资源作为模板，可实例化成游戏中具体的对象。**
-  编写一个代码，使用 debug 语句来验证 MonoBehaviour 基本行为或事件触发的条件

	- 基本行为包括 Awake() Start() Update() FixedUpdate() LateUpdate()
	- 常用事件包括 OnGUI() OnDisable() OnEnable()
 **答：验证如下： 
（1）当一个脚本实例被载入时Awake被调用。
（2）Start仅在Update函数第一次被调用前调用。 
（3）当MonoBehaviour启用时，其Update在每一帧被调用。
（4）渲染和处理GUI事件时调用OnGUI。 
（5）当MonoBehaviour启用时，其 FixedUpdate 在每一帧被调用。
（6）当Behaviour启用时，其LateUpdate在每一帧被调用。 
（7）Reset,重置为默认值。 
（8）当对象变为不可用或非激活状态时OnDisable函数被调用。
（9）当MonoBehaviour将被销毁时,OnDestory函数被调用。**

- 查找脚本手册，了解 GameObject，Transform，Component 对象

	- 分别翻译官方对三个对象的描述（Description）
**答：GameObject是Unity场景里面所有实体的基类.Transform附加到GameObject（游戏物体）（如无附加则为空）。Component是一切附加到游戏物体的基类。因此，GameObject中包含了Component，而Transform属于Component的一种。GameObject必定包含一个Transform组件，也可能包含其它Component。 **
	- 描述下图中 table 对象（实体）的属性、table 的 Transform 的属性、 table 的部件
		- 本题目要求是把可视化图形编程界面与 Unity API 对应起来，当你在 Inspector 面板上每一个内容，应该知道对应 API。
		- 例如：table 的对象是 GameObject，第一个选择框是 activeSelf 属性。
	- 用 UML 图描述 三者的关系（请使用 UMLet 14.1.1 stand-alone版本出图）
![这里写图片描述](https://img-blog.csdn.net/20180326171217820?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2phbmtpbmdtZWFuaW5n/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
**答：**
table 对象(实体)的属性

			layer : Default
			tag : Untagged
	table 的 Transform 的属性

			Position: (0, 0, 0)
			Rotation: (0, 0, 0)
			Scale : (1, 1, 1)
	table 的 部件

			Transform
			Mesh Renderer
			Box Collider
			
![UML图](https://img-blog.csdn.net/20180327003224376?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2phbmtpbmdtZWFuaW5n/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
- 整理相关学习资料，编写简单代码验证以下技术的实现：
	- 查找对象
	- 添加子对象
	- 遍历对象树
	- 清除所有子对象

**答：**

```
//查找对象
	GameObject.Find("Name");

//添加子对象
	GameObject.CreatePrimitive(PrimitiveType);

//遍历对象树
	foreach(Transform child in transform) {
		//operation
	}

//清除所有子对象
	foreach(Transform child in transform) {
		Destroy(child.gameObject);
	}
```

- 资源预设（Prefabs）与 对象克隆 (clone)
	- 预设（Prefabs）有什么好处？
	- 预设与对象克隆 (clone or copy or Instantiate of Unity Object) 关系？
	- 制作 table 预制，写一段代码将 table 预制资源实例化成游戏对象
  
**答：Prefabs（预设）是最非常用的一种资源类型，是一种可被重复使用的游戏对象。它可以被置入多个场景中，也可以在一个场景中多次置入。它可以预设物体，类似与C/C++中的指针，可大大降低资源的占用，特别是手机开发方面，并还可以方便的设置物体的属性，预设类似于一个模板，通过预设可以创建相同属性的对象，这些对象和预设关联。一旦预设发生改变，所有通过预设实例化的对象都会产生相应的变化（适合批量处理）。对象克隆不受克隆本体的影响，因此A对象克隆的对象B不会因为A的改变而相应改变。**

```
public class NewBehaviourScript : MonoBehaviour {
    public GameObject preGO;
    void Start () {
        GameObject table = Instantiate(preGO);
}
```

- 尝试解释组合模式（Composite Pattern / 一种设计模式）。使用 BroadcastMessage() 方法向子对象发送消息GameObject table = Instantiate(prefab);

**答：**

```
public class BehaviourScript : MonoBehaviour {
   void Start () {
       this.BroadcastMessage("Test");
   }
   void Test() {
        Debug.Log("Child Received");
    }
}
```
