using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Mygame;

public class FirstController : MonoBehaviour, SceneController, UserAction {

	readonly Vector3 water_pos = new Vector3(0,0.5F,0);
	UserGUI userGUI;

	public CoastController fromCoast;
	public CoastController toCoast;
	public BoatController boat;
	private MyCharacterController[] characters;

	public CCActionManager actionManager;
	private bool ifGameover;

	void Awake() {
		Director director = Director.getInstance ();
		director.currentSceneController = this;
		userGUI = gameObject.AddComponent <UserGUI>() as UserGUI;
		characters = new MyCharacterController[6];

		actionManager = gameObject.AddComponent<CCActionManager>() as CCActionManager;
		ifGameover = false;

		loadResources ();
	}

	//create river
	public void loadResources() {
		GameObject water = Instantiate (Resources.Load ("Perfabs/Water", typeof(GameObject)), water_pos, Quaternion.identity, null) as GameObject;
		water.name = "water";

		fromCoast = new CoastController ("from");
		toCoast = new CoastController ("to");
		boat = new BoatController ();

		loadCharacter ();
	}

	private void loadCharacter() {
		for (int i = 0; i < 3; i++) {
			//create priests
			MyCharacterController pr = new MyCharacterController ("priest");
			pr.setName ("priest" + i);
			pr.setPosition (fromCoast.getEmptyPosition ());
			pr.getOnCoast (fromCoast);
			fromCoast.getOnCoast (pr);
			characters [i] = pr;
		}
		for (int i = 0; i < 3; i++) {
			//create devils
			MyCharacterController de = new MyCharacterController ("devil");
			de.setName("devil" + i);
			de.setPosition (fromCoast.getEmptyPosition ());
			de.getOnCoast (fromCoast);
			fromCoast.getOnCoast (de);

			characters [i+3] = de;
		}

	}


	public void moveBoat() {
		if (ifGameover || boat.isEmpty()) return;
		//boat.Move ();
		userGUI.status = check_game_over ();
		actionManager.moveBoat(boat.getGameobj(), boat.get_to_or_from());
		boat.changeToOrFrom();
		ifGameover = userGUI.status != 0 ? true : false;
	}

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

			actionManager.moveCharacter(characterCtrl.getGameObject(), whichCoast.getEmptyPosition());
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

			actionManager.moveCharacter(characterCtrl.getGameObject(), boat.getEmptyPosition());
			characterCtrl.getOnBoat (boat);
			boat.GetOnBoat (characterCtrl);
		}
		userGUI.status = check_game_over ();
		ifGameover = userGUI.status != 0 ? true : false;
	}

	int check_game_over() {
		int from_priest = 0,  from_devil = 0,  to_priest = 0,  to_devil = 0;

		int[] fromCount = fromCoast.getCharacterNum ();
		from_priest += fromCount[0];
		from_devil += fromCount[1];

		int[] toCount = toCoast.getCharacterNum ();
		to_priest += toCount[0];
		to_devil += toCount[1];

		if (to_priest + to_devil == 6)
			return 2;//WIN THE GAME
		if (from_priest < from_devil && from_priest > 0)
			return 1;//LOSE THE GAME	
		if (to_priest < to_devil && to_priest > 0) 
			return 1;//LOSE THE GAME
		return 0;//CONTINUE
	}

	public void restart() {
		boat.reset ();
		fromCoast.reset ();
		toCoast.reset ();
		for (int i = 0; i < characters.Length; i++) {
			characters [i].reset ();
		}
		ifGameover = false;
	}
}
