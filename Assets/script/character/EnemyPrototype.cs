using UnityEngine;
using System.Collections;

public class EnemyPrototype : CharacterController {

    new void Start() {
        base.Start();
        setCurrentCharacterState(CharacterState.IDLE);
    }

	void Update () {

		WorldController ctrl = WorldController.instance;
        int playerTileInstanceID = ctrl.player.GetComponent<CharacterPosition>().getCurrentInstanceID();
        int playerTile = ctrl.getTileIndexFromID(playerTileInstanceID);

        if (this.getCurrentCharacterState() == CharacterState.MOVE) {
            if (isTileWithinRangeOfCurrentSkill(playerTile)) {
                Debug.Log("NPC IN RANGE: SWITCH TO ATTACK");
                setCurrentCharacterState(CharacterState.ATTACK);
            } else {
                Debug.Log("NPC OUT OF RANGE MOVE");
                ctrl.onTileHover(playerTileInstanceID);
                ctrl.onTileSelect(playerTileInstanceID);
            }

            if (getCurrentActionPoints() < 2) setActionPointsToZero();
        }else if(this.getCurrentCharacterState() == CharacterState.ATTACK) {
            Debug.Log("NPC ATTACK");
            ctrl.onTileHover(playerTileInstanceID);
            ctrl.onTileSelect(playerTileInstanceID);

            if (getCurrentActionPoints() < 3) setActionPointsToZero();
        }
	}
}

