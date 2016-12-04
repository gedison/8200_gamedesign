using UnityEngine;
using System.Collections;


/* A blast skill on the other hand affects a tiles in an x by y formation n units away from the player, the hardcoded values are here again, because I don't like 
 * math
 */
public class BlastSkill : Skill {

    private int blastSize, maxDistanceFromPlayer;
    public BlastSkill(string skillName, string skillDescription, CharacterController.CharacterAttribute attribute, CharacterController.CharacterAttribute versus, int diceNum, int diceSides, int blastSize, int maxDistanceFromPlayer) : base(skillName, skillDescription, attribute, versus, diceNum, diceSides) {
        if (blastSize > 3) blastSize = 3;
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
        if (distance <= maxDistanceFromPlayer && distance>0) return true;
        else return false;
    }

    override
    public ArrayList getTilesAffectedBySkillFromOrigin(int boardWidth, int boardHeight, int skillOrigin, int playerOrigin) {
        ArrayList ret = new ArrayList();
      
        //The if statements cascade into each other increasing the size of the blast with the radius
        if (blastSize >= 1) {
            ret.Add(skillOrigin);
        }

        if (blastSize >= 2) {
            //Right
            int temp = skillOrigin + 1;
            if (temp % boardWidth != 0) ret.Add(temp);
            //Bottom
            temp = skillOrigin - boardWidth;
            if (temp >= 0) ret.Add(temp);
            //Bottom-Right
            temp = (skillOrigin - boardWidth)+1;
            if (temp>0 && temp % boardWidth != 0) ret.Add(temp);
        }

        if (blastSize >= 3) {
            //Bottom-Left
            int temp = skillOrigin - boardWidth - 1;
            if (temp>=0 && skillOrigin % boardWidth != 0) ret.Add(temp);    
            //Left
            temp = skillOrigin - 1;
            if (skillOrigin % boardWidth != 0) ret.Add(temp);
            //Top-Left
            temp = skillOrigin + boardWidth - 1;
            if (temp >= 0 && skillOrigin % boardWidth != 0) ret.Add(temp);
            //Top
            temp = skillOrigin + boardWidth;
            if (temp >= 0) ret.Add(temp);
            //Top-Right
            temp = skillOrigin + boardWidth + 1;
            if (temp >= 0 && temp % boardWidth != 0) ret.Add(temp);
        }

        return ret;
       
    }
}
