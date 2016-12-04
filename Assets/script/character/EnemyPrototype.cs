using UnityEngine;

public class EnemyPrototype : CharacterController {

    new void Start() {
        base.Start();
        setCurrentCharacterState(CharacterState.IDLE);
    }

    //Timer to delay each enemy's attack so their actions don't appear to happen simultaneously
    private int delay = 1;
    private float counter = 0;
    private void endTurnWithDelay() {
        counter += Time.deltaTime;
        if (counter >= delay) {
            counter = 0;
            endTurn();
        }
    }

	void Update () {

		WorldController ctrl = WorldController.instance;
   
        //At the begining of their turn the enemy will be set to the MOVE state
        if (this.getCurrentCharacterState() == CharacterState.MOVE) {

            //Apply Condition at start of turn
            if (this.GetComponent<Condition>() != null) {
                Condition currentCondition = this.GetComponent<Condition>();
                currentCondition.doConditionActionOnSelf();
                this.GetComponent<ScreenSpaceDamageUI>().createDamageText(currentCondition.getName());
            }

            if (ctrl.player != null) {
                //Get the players position
                int playerTile = ctrl.player.GetComponent<CharacterPosition>().getTileID();

                //If the NPC is within range of Player switch to attack
                if (isTileWithinRangeOfCurrentSkill(playerTile)) {
                    setCurrentCharacterState(CharacterState.ATTACK);
                //else move towards player
                } else {
                    ctrl.onTileHover(playerTile);
                    ctrl.onTileSelect(playerTile);
                    GetComponent<CharacterMovementController>().setUpdateTraversalMapToTrue();
                }
            }else setActionPointsToZero();

            //If the NPC has moved twice end their turn
            if (getCurrentActionPoints() < 2) endTurnWithDelay();
            
        //If the NPC is within range of the player they will be set to the ATTACK state
        }else if(this.getCurrentCharacterState() == CharacterState.ATTACK) {
            if (ctrl.player != null) {
                int playerTile = ctrl.player.GetComponent<CharacterPosition>().getTileID();
                ctrl.onTileHover(playerTile);
                ctrl.onTileSelect(playerTile);
            } else setActionPointsToZero();

            //If the NPC has attacked end their turn
            if (getCurrentActionPoints() < 3) endTurnWithDelay();
        }
	}
}

