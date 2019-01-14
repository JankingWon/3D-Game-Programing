using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Patrols;


public class PatrolBehaviour : MonoBehaviour {
    private IAddAction addAction;
    private IGameStatusOp gameStatusOp;

    public int ownIndex;
    public bool isCatching;

    private float CATCH_RADIUS = 3.0f;

    void Start () {
        addAction = SceneController.getInstance() as IAddAction;
        gameStatusOp = SceneController.getInstance() as IGameStatusOp;

        ownIndex = getOwnIndex();
        isCatching = false;
    }
	
	void Update () {
        checkNearByHero();
	}

    int getOwnIndex() {
        string name = this.gameObject.name;
        char cindex = name[name.Length - 1];
        int result = cindex - '0';
        return result;
    }
    
    void checkNearByHero () {
        if (gameStatusOp.getHeroStandOnArea() == ownIndex) { 
            if (!isCatching) {
                isCatching = true;
                addAction.addDirectMovement(this.gameObject);
            }
        }
        else {
            if (isCatching) { 
                gameStatusOp.heroEscapeAndScore();
                isCatching = false;
                addAction.addRandomMovement(this.gameObject, false);
            }
        }
    }

    void OnCollisionStay(Collision e) {
        if (e.gameObject.name.Contains("Patrol") || e.gameObject.name.Contains("fence")
            || e.gameObject.tag.Contains("FenceAround")) {
            isCatching = false;
            addAction.addRandomMovement(this.gameObject, false);
        }
        
        if (e.gameObject.name.Contains("Hero")) {
            gameStatusOp.patrolHitHeroAndGameover();
        }
    }
}
