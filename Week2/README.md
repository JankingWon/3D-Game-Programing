# Week2---3D-game-programming
 - 游戏对象运动的本质是什么？
**答：游戏对象运动的本质，其实是游戏对象跟随每一帧的变化，空间地变化。这里的空间变化包括了游戏对象的transform属性中的position跟rotation两个属性。一个是绝对或者相对位置的改变，一个是所处位置的角度的旋转变化。**
 - 请用三种方法以上方法，实现物体的抛物线运动。（如，修改Transform属性，使用向量Vector3的方法…）
```
public float speed = 1f;
void Update () {
    this.transform.position += speed * Vector3.up * Time.deltaTime ;
    this.transform.position += 10 * Vector3.right * Time.deltaTime ;
    speed+= 0.1f;
}
```

```
public float speed = 1f;
void Update () {
    Vector3 tmp = new Vector3(10 * Time.deltaTime, speed * Time.deltaTime, 0);
    this.transform.position += tmp;
    speed+= 0.1f;
}
```

```
public float speed = 1f;
void Update () {
    Vector3 tmp = new Vector3(10 * Time.deltaTime, speed * Time.deltaTime, 0);
    this.transform.Translate(tmp);
    speed+= 0.1f;
}
```

 - 写一个程序，实现一个完整的太阳系， 其他星球围绕太阳的转速必须不一样，且不在一个法平面上。   
**效果图** 
![这里写图片描述](https://img-blog.csdn.net/20180403145210372?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2phbmtpbmdtZWFuaW5n/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
**公转脚本**
![这里写图片描述](https://img-blog.csdn.net/20180403145349227?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2phbmtpbmdtZWFuaW5n/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
**自转脚本**
![这里写图片描述](https://img-blog.csdn.net/2018040314551328?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2phbmtpbmdtZWFuaW5n/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
**Materials以及布局**
![这里写图片描述](https://img-blog.csdn.net/2018040315105914?watermark/2/text/aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L2phbmtpbmdtZWFuaW5n/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70)
