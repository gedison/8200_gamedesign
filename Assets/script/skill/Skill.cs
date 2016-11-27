using System.Collections;

public abstract class Skill {

    private CharacterController.CharacterAttribute skillAttribute, skillVersus;
    private string skillName, skillDescription;
    private int numberOfDice, numberOfSidesOnDie;
    private Die myDie;
    private Condition conditionToBeApplied = null;
    private int usesPerEncounter = 0;
    private int timesUsed = 0;

    public Skill(string skillName, string skillDescription, CharacterController.CharacterAttribute attribute, CharacterController.CharacterAttribute versus, int numberOfDice, int numberOfSidesOnDie) {
        this.skillName = skillName;
        this.skillDescription = skillDescription;
        this.numberOfDice = numberOfDice;
        this.numberOfSidesOnDie = numberOfSidesOnDie;
        skillAttribute = attribute;
        skillVersus = versus;

        myDie = new Die(numberOfSidesOnDie);
    }

    public void setUsesPerEncounter(int usesPerEncounter) {
        this.usesPerEncounter = usesPerEncounter;
    }

    public int getNumberOfTimesUsesInEncounter() {
        if (usesPerEncounter == 0) return 0;
        else return timesUsed;
    }

    public void incrementTimesUsed() {
        timesUsed++;
    }

    public void resetTimesUsed() {
        timesUsed = 0;
    }

    public bool hasSkillBeenUsedUpForEncounter() {
        if (usesPerEncounter == 0 || usesPerEncounter > timesUsed) return false;
        else return true;
    }

    public void setCondition(Condition condition) {
        conditionToBeApplied = condition;
    }

    public Condition getCondition() {
        return conditionToBeApplied;
    }

    public string getSkillName() {
        return skillName;
    }

    public string getSkillDescription() {
        return skillDescription;
    }

    public CharacterController.CharacterAttribute getSkillAttribute() {
        return skillAttribute;
    }

    public CharacterController.CharacterAttribute getSkillVersus() {
        return skillVersus;
    }

    public int getAttackDamage() {
        int damage = 0;
        for (int i = 0; i < numberOfDice; i++) damage += myDie.getDiceRoll();
        return damage;
    }

    public string toString() {
        string ret = "Damage: " + numberOfDice + "D" + numberOfSidesOnDie;
        return ret;
    }

    public abstract bool isTileWithinRangeOfSkill(int boardWidth, int boardHeight, int skillOrigin, int playerOrigin);

    public abstract ArrayList getTilesAffectedBySkillFromOrigin(int boardWidth, int boardHeight, int skillOrigin, int playerOrigin);
}
