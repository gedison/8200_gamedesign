﻿using UnityEngine;
using System.Collections;

public class EnemyPrototype : CharacterController {

    new void Start() {
        base.Start();
        setCurrentCharacterState(CharacterState.IDLE);
    }

	void Update () {

		WorldController ctrl = WorldController.instance;
   
 
        if (this.getCurrentCharacterState() == CharacterState.MOVE) {
            int playerTile = ctrl.player.GetComponent<CharacterPosition>().getTileID();

            if (isTileWithinRangeOfCurrentSkill(playerTile)) {
                Debug.Log("NPC IN RANGE: SWITCH TO ATTACK");
                setCurrentCharacterState(CharacterState.ATTACK);
            } else {
                Debug.Log("NPC OUT OF RANGE MOVE");
                ctrl.onTileHover(playerTile);
                ctrl.onTileSelect(playerTile);
            }

            if (getCurrentActionPoints() < 2) endTurn();
        }else if(this.getCurrentCharacterState() == CharacterState.ATTACK) {
            int playerTile = ctrl.player.GetComponent<CharacterPosition>().getTileID();

            Debug.Log("NPC ATTACK");
            ctrl.onTileHover(playerTile);
            ctrl.onTileSelect(playerTile);

            if (getCurrentActionPoints() < 3) endTurn();
        }
	}
}

