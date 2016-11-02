using UnityEngine;
using System.Collections;

public class EnemyPrototype : CharacterController {

	void Update () {
		WorldController ctrl = WorldController.instance;
		if (this.getCurrentCharacterState () == CharacterState.MOVE) {
			int ploc = WorldController.instance.getTileIndexFromID (WorldController.instance.player.GetComponent<CharacterPosition> ().getCurrentInstanceID ());
			int enemyPosition = ctrl.getTileIndexFromID(ctrl.player.GetComponent<CharacterPosition>().getCurrentInstanceID());
			for (int i = 0; i < tilesEffectedByPlayerSkill.Length; i++) {
				if (tilesEffectedByPlayerSkill[i] == enemyPosition) {
					enemyHit = true;
					int playerRole = myCharacterController.roleD20ForCurrentSkill();
					if (player.GetComponent<CharacterController>().roleD20UsingAttributeAsModifier(versus) < playerRole) {
						player.GetComponent<CharacterController>().myHealth.decrementCurrentHealthByX(skillDamage);
					}
				}
			}
			WorldController.instance.onTileHover (WorldController.instance.player.GetComponent<CharacterPosition> ().getCurrentInstanceID ());
			WorldController.instance.onTileSelect (WorldController.instance.player.GetComponent<CharacterPosition> ().getCurrentInstanceID ());

			//if (getCurrentActionPoints() < 3) setCurrentCharacterState(CharacterState.ATTACK);
		}

        this.setActionPointsToZero();


    }
}
