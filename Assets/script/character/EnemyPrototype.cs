using UnityEngine;
using System.Collections;

public class EnemyPrototype : CharacterController {

	
	

	void Update () {
        if(this.getCurrentCharacterState() == CharacterState.MOVE) {
            WorldController.instance.onTileHover(WorldController.instance.player.GetComponent<CharacterPosition>().getCurrentInstanceID());
            WorldController.instance.onTileSelect(WorldController.instance.player.GetComponent<CharacterPosition>().getCurrentInstanceID());

            if (!this.GetComponent<CharacterMovementController>().isCharacterMoving()) this.endTurn();
        }
        
	
	}
}
