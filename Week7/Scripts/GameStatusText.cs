using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameStatusText : MonoBehaviour {
    private int score = 0;
    private int textType;
    private bool isEnd = false;
    private Timer _timer;

    void Start () {
        _timer = new Timer(30);
        _timer.tickEvent += SendMessage;
        _timer.StartTimer();
        distinguishText();
	}
	
	void Update () {
        _timer.UpdateTimer(Time.deltaTime);
        if (_timer.DisplayTime() <= 0)
            gameOver();
        if (textType == 2 && !isEnd){
                gameObject.GetComponent<Text>().text = "剩余时间：" + (_timer.DisplayTime() >= 0 ? _timer.DisplayTime() : 0) + " 秒";
        }
        
        
    }
    private void SendMessage()

    {

        Debug.Log("Game Over!");

    }

    void distinguishText() {
        if (gameObject.name.Contains("Score"))
            textType = 0;
        else if (gameObject.name.Contains("GameOver"))
            textType = 1;
        else
            textType = 2;
    }

    void OnEnable() {
        GameEventManager.myGameScoreAction += gameScore;
        GameEventManager.myGameOverAction += gameOver;
    }

    void OnDisable() {
        GameEventManager.myGameScoreAction -= gameScore;
        GameEventManager.myGameOverAction -= gameOver;
    }

    void gameScore() {
        if(!isEnd)
            score++;
        if (!isEnd && textType == 0) {
            
            this.gameObject.GetComponent<Text>().text = "得分: " + score;
        }
    } 

    void gameOver() {
        isEnd = true;
        _timer.StopTimer();
        if (textType == 1)
        {
            this.gameObject.GetComponent<Text>().text = "游戏结束!\n您的总得分为：" + score;
            
            
        }
        if(textType == 0)
        {
            this.gameObject.GetComponent<Text>().text = "游戏已结束! 将不再计分";
        }
        if(textType == 2)
        {
            this.gameObject.GetComponent<Text>().text = "游戏时间：" + _timer.GetTime() + " 秒";
        }
        
    }
}
