public abstract class Skill {

    private CharacterController.CharacterAttribute skillAttribute;
    private CharacterController.CharacterAttribute skillVersus;
    private string skillName, skillDescription;
    private int numberOfDice, numberOfSidesOnDie;

    public Skill(string skillName, string skillDescription, CharacterController.CharacterAttribute attribute, CharacterController.CharacterAttribute versus, int numberOfDice, int numberOfSidesOnDie) {
        this.skillName = skillName;
        this.skillDescription = skillDescription;
        this.skillAttribute = attribute;
        skillVersus = versus;
        this.numberOfDice = numberOfDice;
        this.numberOfSidesOnDie = numberOfSidesOnDie;
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
        Die myDie = new Die(numberOfSidesOnDie);

        int damage = 0;
        for (int i = 0; i < numberOfDice; i++) damage += myDie.getDiceRoll();
        return damage;
    }

    public abstract bool isTileWithinRangeOfSkill(int boardWidth, int boardHeight, int skillOrigin, int playerOrigin);

    public abstract int[] getTilesAffectedBySkillFromOrigin(int boardWidth, int boardHeight, int skillOrigin, int playerOrigin);
}
