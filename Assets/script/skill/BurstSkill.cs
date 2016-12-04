using System.Collections;


/* A burst skill is a Dnd turn that indicates that it effects all the tiles surrounding a player up to a certain radius
 * I'm hard coding the affected tiles because I don't like to do math 
 */
public class BurstSkill : Skill {

    private int burstSize;
    public BurstSkill(string skillName, string skillDescription, CharacterController.CharacterAttribute attribute, CharacterController.CharacterAttribute versus, int diceNum, int diceSides, int burstSize) : base(skillName, skillDescription, attribute, versus, diceNum, diceSides) {
        burstSize = 1;
        this.burstSize = burstSize;
    }

    override
    public bool isTileWithinRangeOfSkill(int boardWidth, int boardHeight, int skillOrigin, int playerOrigin) {
        return true;
    }

    override
    public ArrayList getTilesAffectedBySkillFromOrigin(int boardWidth, int boardHeight, int skillOrigin, int playerOrigin) {
        ArrayList ret = new ArrayList();

        if (burstSize == 1) {
            //Right
            int temp = playerOrigin + 1;
            if (temp % boardWidth != 0) ret.Add(temp);
            //Bottom
            temp = playerOrigin - boardWidth;
            if (temp >= 0) ret.Add(temp);
            //Bottom-Right
            temp = (playerOrigin - boardWidth) + 1;
            if (temp > 0 && temp % boardWidth != 0) ret.Add(temp);
            //Bottom-Left
            temp = playerOrigin - boardWidth - 1;
            if (temp >= 0 && playerOrigin % boardWidth != 0) ret.Add(temp);
            //Left
            temp = playerOrigin - 1;
            if (playerOrigin % boardWidth != 0) ret.Add(temp);
            //Top-Left
            temp = playerOrigin + boardWidth - 1;
            if (temp >= 0 && playerOrigin % boardWidth != 0) ret.Add(temp);
            //Top
            temp = playerOrigin + boardWidth;
            if (temp >= 0) ret.Add(temp);
            //Top-Right
            temp = playerOrigin + boardWidth + 1;
            if (temp >= 0 && temp % boardWidth != 0) ret.Add(temp);
        }
      
        return ret;
    }
}
