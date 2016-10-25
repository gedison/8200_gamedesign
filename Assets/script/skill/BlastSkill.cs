using UnityEngine;
using System.Collections;

public class BlastSkill : Skill {

    private int blastSize, maxDistanceFromPlayer;
    public BlastSkill(string skillName, string skillDescription, int diceNum, int diceSides, int blastSize, int maxDistanceFromPlayer) : base(skillName, skillDescription, diceNum, diceSides) {
        this.blastSize = blastSize;
        this.maxDistanceFromPlayer = maxDistanceFromPlayer;
    }

    override
    public bool isTileWithinRangeOfSkill(int boardWidth, int boardHeight, int skillOrigin, int playerOrigin) {
        int skillX, skillY, playerX, playerY;
        skillX = skillOrigin % boardWidth;
        skillY = skillOrigin / boardWidth;

        playerX = playerOrigin % boardWidth;
        playerY = playerOrigin / boardWidth;

        int distance = (int)Mathf.Sqrt(Mathf.Pow((playerX - skillX), 2) + Mathf.Pow((playerY - skillY), 2));
        if (distance <= maxDistanceFromPlayer) return true;
        else return false;
    }


    //Only works for blast size = 1;
    override
    public int[] getTilesAffectedBySkillFromOrigin(int boardWidth, int boardHeight, int skillOrigin, int playerOrigin) {
        return new int[]{skillOrigin};
    }
}
