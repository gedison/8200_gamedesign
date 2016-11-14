using UnityEngine;
using System.Collections;


public class CharacterController : MonoBehaviour {

    public string professionString = "Warrior";
    private Profession myProfession;

    private int level = 1;
    private int experience = 0;
    public Health myHealth;

    private Die d20 = new Die(20);

    public GameObject equipedArmor;
    public GameObject equipedWeapon;
    private Skill currentSkill;
    private ArrayList characterSkills = new ArrayList();

    public int visibilityRange = 5;
    public int totalActionPoints = 5;
    private int currentActionPoints;

    private CharacterState currentCharacterState = CharacterState.MOVE;

    public enum CharacterState { MOVE, ATTACK, IDLE, TURN_OVER};
    public enum CharacterAttribute { STRENGTH, DEXTERITY, INTELLIGENCE, WISDOM, ARMOR_CLASS, HEALTH, NONE};

    public void Start() {
        switch (professionString) {
            case "Warrior": myProfession = new Warrior(); break;
            case "Minion": myProfession = new Minion(); break;
            default: myProfession = new Warrior(); break;
        }

        myHealth = this.gameObject.AddComponent<Health>() as Health;
        myHealth.totalHealth = myProfession.getBaseHealth();

        currentActionPoints = totalActionPoints;

        switch (professionString) {
            case "Warrior":
                characterSkills.Add(new BlastSkill("Melee Attack", "Basic Melee Attack", CharacterAttribute.STRENGTH, CharacterAttribute.ARMOR_CLASS, 2, 3, 1, 1));
                characterSkills.Add(new BurstSkill("Area Attack", "Attack all enemies in a one burst radius", CharacterAttribute.STRENGTH, CharacterAttribute.ARMOR_CLASS, 1, 2, 1));
                BlastSkill temp = new BlastSkill("Daze Enemy", "Dazes one enemy within a one square radius", CharacterAttribute.STRENGTH, CharacterAttribute.DEXTERITY, 1, 3, 1, 1);
                temp.setCondition(new Dazed());
                temp.setUsesPerEncounter(1);
                characterSkills.Add(temp);
               

                currentSkill = (Skill)characterSkills[2];
                break;

            case "Minion":
                characterSkills.Add(new BlastSkill("Melee Attack", "Basic Melee Attack", CharacterAttribute.STRENGTH, CharacterAttribute.ARMOR_CLASS, 1, 3, 1, 1));
                currentSkill = (Skill)characterSkills[0];
                break;
        }
       
    }

    public void startTurn() {
        currentActionPoints = totalActionPoints;
        currentCharacterState = CharacterState.MOVE;
    }

    public void endTurn() {
        currentActionPoints = 0;
        currentCharacterState = CharacterState.TURN_OVER;
    }

    public int roleD20UsingAttributeAsModifier(CharacterAttribute attribute) {
        if (attribute == CharacterAttribute.NONE) return 0;
        else if (attribute == CharacterAttribute.HEALTH) return GetComponent<Health>().getCurrentHealth();
        else if (attribute == CharacterAttribute.ARMOR_CLASS) return getArmorClass();
        else return d20.getDiceRoll() + getModifierOfAttribute(attribute);
    }

    public int getModifierOfAttribute(CharacterAttribute attribute) {
        return (myProfession.getBaseAttribute(attribute) + level) / 2;
    }

    private int getArmorClass() {
        return equipedArmor.GetComponent<Armor>().armorClass;
    }

    public int roleD20ForCurrentSkill() {
        return roleD20UsingAttributeAsModifier(currentSkill.getSkillAttribute());
    }

    public bool isTileWithinRangeOfCurrentSkill(int tileIndex) {
        int characterPosition = GetComponent<CharacterPosition>().getTileID();
        return currentSkill.isTileWithinRangeOfSkill(WorldController.instance.tileWidth, WorldController.instance.tileHeight, tileIndex, characterPosition);
    }

    public int[] getTilesEffectedByCurrentSkill(int tileIndex) {
        if (isTileWithinRangeOfCurrentSkill(tileIndex)) {
            int characterPosition = GetComponent<CharacterPosition>().getTileID();
            return (int[])currentSkill.getTilesAffectedBySkillFromOrigin(WorldController.instance.tileWidth, WorldController.instance.tileHeight, tileIndex, characterPosition).ToArray(typeof(int));
        } else return new int[] { };
    }

    public int getDamageFromCurrentSkill() {
        return equipedWeapon.GetComponent<Weapon>().getAttackDamage() + currentSkill.getAttackDamage();
    }

    public Condition getConditionFromCurrentSkill() {
        return currentSkill.getCondition();
    }

    public CharacterAttribute getCurrentSkillVersus() {
        return currentSkill.getSkillVersus();
    }

    public void setCurrentCharacterState(CharacterState state) {
        currentCharacterState = state;
    }

    public CharacterState getCurrentCharacterState() {
        return currentCharacterState;
    }

    public int getCurrentActionPoints() {
        return currentActionPoints;
    }

    public void decrementActionPointsByAttack() {
        currentActionPoints -= 3;
    }

    public void decrementActionPointsByMovement() {
        currentActionPoints -= 2;
    }

    public void decrementActionPointsByOne() {
        currentActionPoints--;
    }

    public void setActionPointsToZero() {
        currentActionPoints = 0;
    }

    public void setSkillAtIndexToActive(int index) {
        if(index >= 0 && index < characterSkills.Count) {
            if (((Skill)characterSkills[index]).hasSkillBeenUsedUpForEncounter()!=true) {
                currentSkill = (Skill)characterSkills[index];
                setCurrentCharacterState(CharacterController.CharacterState.ATTACK);
            }
        }
    }

    public Skill getSkillAtIndex(int index) {
        if (index >= 0 && index < characterSkills.Count) {
            return (Skill)characterSkills[index];
        } else return null;
    }

    public void resetSkillsPerEncounter() {
        for(int i=0; i<characterSkills.Count; i++) {
            ((Skill)characterSkills[i]).resetTimesUsed();
        }
    }

    public void incrementSkillUsage() {
        currentSkill.incrementTimesUsed();
    }

    public int getMovementSpeed() {
        return myProfession.getBaseMovement();
    }

    void Update () {

	}
}
