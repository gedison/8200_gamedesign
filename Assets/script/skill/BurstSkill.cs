using UnityEngine;
using System.Collections;

public class BurstSkill : Skill {

    private int burstSize;
    public BurstSkill(string skillName, string skillDescription, int diceNum, int diceSides, int burstSize) : base(skillName, skillDescription, diceNum, diceSides) {
        this.burstSize = burstSize;
    }

    override
    public bool isTileWithinRangeOfSkill(int boardWidth, int boardHeight, int skillOrigin, int playerOrigin) {
        bool isTileWithinRange = false;
        return isTileWithinRange;
    }

    override
    public int[] getTilesAffectedBySkillFromOrigin(int boardWidth, int boardHeight, int skillOrigin, int playerOrigin) {
        return null;
    }
}
