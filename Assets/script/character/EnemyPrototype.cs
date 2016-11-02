using UnityEngine;
using System.Collections;

public class EnemyPrototype : CharacterController {
	private int[] tilesEffectedByPlayerSkill;
	void Update () {
		WorldController ctrl = WorldController.instance;
		if (this.getCurrentCharacterState () == CharacterState.MOVE) {
			int ploc = WorldController.instance.getTileIndexFromID (WorldController.instance.player.GetComponent<CharacterPosition> ().getCurrentInstanceID ());
			int enemyPosition = ctrl.getTileIndexFromID(ctrl.player.GetComponent<CharacterPosition>().getCurrentInstanceID());
			tilesEffectedByPlayerSkill = getTilesEffectedByCurrentSkill(ploc);
			for (int i = 0; i < tilesEffectedByPlayerSkill.Length; i++) {
				if (tilesEffectedByPlayerSkill[i] == enemyPosition) {
					setCurrentCharacterState (CharacterState.ATTACK);
				}
			}
			WorldController.instance.onTileHover (WorldController.instance.player.GetComponent<CharacterPosition> ().getCurrentInstanceID ());
			WorldController.instance.onTileSelect (WorldController.instance.player.GetComponent<CharacterPosition> ().getCurrentInstanceID ());

			//if (getCurrentActionPoints() < 3) setCurrentCharacterState(CharacterState.ATTACK);
		}

        this.setActionPointsToZero();


    }
}
