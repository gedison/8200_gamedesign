using UnityEngine;
using System.Collections;

public class EnemyPrototype : CharacterController {
	private int[] tilesEffectedByPlayerSkill;


    new void Start() {
        base.Start();
        setCurrentCharacterState(CharacterState.IDLE);
    }

	void Update () {

		WorldController ctrl = WorldController.instance;


        int playerTileInstanceID = WorldController.instance.player.GetComponent<CharacterPosition>().getCurrentInstanceID();
        int playerTile = WorldController.instance.getTileIndexFromID(playerTileInstanceID);
        if (this.getCurrentCharacterState() == CharacterState.MOVE) {
            if (isTileWithinRangeOfCurrentSkill(playerTile)) {
                Debug.Log("NPC IN RANGE: SWITCH TO ATTACK");
                setCurrentCharacterState(CharacterState.ATTACK);
            } else {
                Debug.Log("NPC OUT OF RANGE MOVE");
                WorldController.instance.onTileHover(playerTileInstanceID);
                WorldController.instance.onTileSelect(playerTileInstanceID);
            }
            if (getCurrentActionPoints() < 2) setActionPointsToZero();
        }else if(this.getCurrentCharacterState() == CharacterState.ATTACK) {
            Debug.Log("NPC ATTACK");
            WorldController.instance.onTileHover(playerTileInstanceID);
            WorldController.instance.onTileSelect(playerTileInstanceID);
            if (getCurrentActionPoints() < 3) setActionPointsToZero();
        }
            /*
			int ploc = WorldController.instance.getTileIndexFromID (WorldController.instance.player.GetComponent<CharacterPosition> ().getCurrentInstanceID ());
			int enemyPosition = ctrl.getTileIndexFromID(ctrl.player.GetComponent<CharacterPosition>().getCurrentInstanceID());
			tilesEffectedByPlayerSkill = getTilesEffectedByCurrentSkill(ploc);
			for (int i = 0; i < tilesEffectedByPlayerSkill.Length; i++) {
				if (tilesEffectedByPlayerSkill[i] == enemyPosition) {
					setCurrentCharacterState (CharacterState.ATTACK);
				}
			}
            */

			//WorldController.instance.onTileHover (WorldController.instance.player.GetComponent<CharacterPosition> ().getCurrentInstanceID ());
			//WorldController.instance.onTileSelect (WorldController.instance.player.GetComponent<CharacterPosition> ().getCurrentInstanceID ());

			//if (getCurrentActionPoints() < 3) setCurrentCharacterState(CharacterState.ATTACK);
		}

       // this.setActionPointsToZero();


    }

