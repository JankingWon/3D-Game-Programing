using System;
using System.Collections.Generic;
using UnityEngine;

public class CCSequeneActions : SSAction, ISSActionCallback {
    public List<SSAction> actionList;
    public int repeatTimes = -1;  
    public int subActionIndex = 0;  

    public static CCSequeneActions CreateSSAction(List<SSAction> _actionList, int _repeatTimes = 0) {
        CCSequeneActions action = ScriptableObject.CreateInstance<CCSequeneActions>();
        action.repeatTimes = _repeatTimes;
        action.actionList = _actionList;
        return action;
    }

    public override void Start() {
        foreach (SSAction action in actionList) {
            action.gameObject = this.gameObject;
            action.transform = this.transform;
            action.callBack = this;
            action.Start();
        }
    }

    public override void Update() {
        if (actionList.Count == 0)
            return;
        else if (subActionIndex < actionList.Count) {
            actionList[subActionIndex].Update();
        }
    }
    public void SSActionEvent(SSAction source, 
        SSActionEventType eventType = SSActionEventType.Completed,
        SSActionTargetType intParam = SSActionTargetType.Normal, string strParam = null, object objParam = null) {

        source.destroy = false;
        this.subActionIndex++;
        if (this.subActionIndex >= actionList.Count) {
            this.subActionIndex = 0;
            if (repeatTimes > 0)
                repeatTimes--;
            if (repeatTimes == 0) {
                this.destroy = true;
                this.callBack.SSActionEvent(this);
            }
        }
    }

    void OnDestroy() {

    }
}
