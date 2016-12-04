using UnityEngine;

public class EnemyPrototype : CharacterController {

    private int delay = 1;
    private float counter = 0;

    new void Start() {
        base.Start();
        setCurrentCharacterState(CharacterState.IDLE);
    }

	void Update () {

		WorldController ctrl = WorldController.instance;
   
 
        if (this.getCurrentCharacterState() == CharacterState.MOVE) {

            //Apply Condition at start of turn
            if (this.GetComponent<Condition>() != null) {
                Condition currentCondition = this.GetComponent<Condition>();
                currentCondition.doConditionActionOnSelf();
                this.GetComponent<ScreenSpaceDamageUI>().createDamageText(currentCondition.getName());
            }

            int playerTile = ctrl.player.GetComponent<CharacterPosition>().getTileID();
            if (isTileWithinRangeOfCurrentSkill(playerTile)) {
                Debug.Log("NPC IN RANGE: SWITCH TO ATTACK");
                setCurrentCharacterState(CharacterState.ATTACK);
            } else {
                Debug.Log("NPC OUT OF RANGE MOVE");
                ctrl.onTileHover(playerTile);
                ctrl.onTileSelect(playerTile);
                GetComponent<CharacterMovementController>().setUpdateTraversalMapToTrue();
            }

            if (getCurrentActionPoints() < 2) {
                counter += Time.deltaTime;
                if (counter >= delay) {
                    counter = 0;
                    endTurn();
                }
            }
        }else if(this.getCurrentCharacterState() == CharacterState.ATTACK) {
            if (ctrl.player != null) {
                int playerTile = ctrl.player.GetComponent<CharacterPosition>().getTileID();
                Debug.Log("NPC ATTACK");
                ctrl.onTileHover(playerTile);
                ctrl.onTileSelect(playerTile);
            } else setActionPointsToZero();

            

            if (getCurrentActionPoints() < 3) {
                counter += Time.deltaTime;
                if (counter >= delay) {
                    counter = 0;
                    endTurn();
                }
            }
        }
	}
}

