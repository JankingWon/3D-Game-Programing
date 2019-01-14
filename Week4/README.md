# Week4---3D-Game-Programming
**为了避免演示视频传到某些视频网站上有广告，还是直接传到GITHUB上吧，并不是很大**
 - 游戏开始时点击开始按钮
 - 就有很多随机颜色，大小，速度的飞碟飞过来，点中之后会根据不同的飞碟增加分数（如下函数）
 - 分数达到1000分结束游戏，可以点击重新开始
 - 游戏有倒计时，60秒后没有到1000分，则游戏失败
```
public void Record(GameObject disk)
	{

		score += (100 - (disk.GetComponent<DiskData>().size) *(20 - disk.GetComponent<DiskData>().speed));

		Color c = disk.GetComponent<DiskData>().color;
		switch (c.ToString())
		{
		case "red":
			score += 50;
			break;
		case "green":
			score += 40;
			break;
		case "blue":
			score += 30;
			break;
		case "yellow":
			score += 10;
			break;
		}

	}
```
**参考了老师给的师兄博客**
[参考博客](https://blog.csdn.net/x2_yt/article/details/66969242)
主要是参考了设计模式，重新设定了游戏规则和界面，还有不同类与接口之间的联系也重新设计过了
