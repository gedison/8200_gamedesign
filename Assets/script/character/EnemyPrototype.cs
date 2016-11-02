using UnityEngine;
using System.Collections;

public class EnemyPrototype : CharacterController {

	
	

	void Update () {
        if(this.getCurrentCharacterState() == CharacterState.MOVE) {
            WorldController.instance.onTileHover(WorldController.instance.player.GetComponent<CharacterPosition>().getCurrentInstanceID());
            WorldController.instance.onTileSelect(WorldController.instance.player.GetComponent<CharacterPosition>().getCurrentInstanceID());

            if (getCurrentActionPoints() <=1 ) setCurrentCharacterState(CharacterState.ATTACK);
        }

        this.setActionPointsToZero();


    }
}
