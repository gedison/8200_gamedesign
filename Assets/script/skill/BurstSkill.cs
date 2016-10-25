using UnityEngine;
using System.Collections;

public class BurstSkill : Skill {

    private int burstSize;
    public BurstSkill(string skillName, string skillDescription, CharacterController.CharacterAttribute attribute, CharacterController.CharacterAttribute versus, int diceNum, int diceSides, int burstSize) : base(skillName, skillDescription, attribute, versus, diceNum, diceSides) {
        this.burstSize = burstSize;
    }

    override
    public bool isTileWithinRangeOfSkill(int boardWidth, int boardHeight, int skillOrigin, int playerOrigin) {
        return true;
    }

    override
    public int[] getTilesAffectedBySkillFromOrigin(int boardWidth, int boardHeight, int skillOrigin, int playerOrigin) {
        return null;
    }
}
