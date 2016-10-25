public abstract class Skill {

    private string skillName, skillDescription;
    private int numberOfDice, numberOfSidesOnDie;

    public Skill(string skillName, string skillDescription, int numberOfDice, int numberOfSidesOnDie) {
        this.skillName = skillName;
        this.skillDescription = skillDescription;
        this.numberOfDice = numberOfDice;
        this.numberOfSidesOnDie = numberOfSidesOnDie;
    }

    public string getSkillName() {
        return skillName;
    }

    public string getSkillDescription() {
        return skillDescription;
    }

    public int getAttackDamage() {
        Die myDie = new Die(numberOfSidesOnDie);

        int damage = 0;
        for (int i = 0; i < numberOfDice; i++) damage += myDie.getDiceRoll();
        return damage;
    }

    public abstract bool isTileWithinRangeOfSkill(int boardWidth, int boardHeight, int skillOrigin, int playerOrigin);

    public abstract int[] getTilesAffectedBySkillFromOrigin(int boardWidth, int boardHeight, int skillOrigin, int playerOrigin);
}
