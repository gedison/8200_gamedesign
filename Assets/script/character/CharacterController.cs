﻿using UnityEngine;


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

    public int totalActionPoints = 5;
    private int currentActionPoints;

	private int prevTileID;

    private CharacterState currentCharacterState = CharacterState.MOVE;
  

    public enum CharacterState { MOVE, ATTACK, IDLE, NOT_IN_COMBAT, DEAD };
    public enum CharacterAttribute { STRENGTH, DEXTERITY, INTELLIGENCE, WISDOM, ARMOR_CLASS };

	int getPrevTile() {
		return prevTileID;
	}

	void setPrevTile(int id) {
		prevTileID = id;
	}

    void Start() {
        switch (professionString) {
            case "Warrior": myProfession = new Warrior(); break;
            default: myProfession = new Warrior(); break;
        }


        myHealth = this.gameObject.AddComponent<Health>() as Health;
        myHealth.totalHealth = myProfession.getBaseHealth();

        currentActionPoints = totalActionPoints;
        currentSkill = new BlastSkill("Melee Attack", "Basic Melee Attack", CharacterAttribute.STRENGTH, CharacterAttribute.ARMOR_CLASS, 1, 3, 1, 1);
    }

    public void endTurn() {
        currentActionPoints = totalActionPoints;
        currentCharacterState = CharacterState.IDLE;
    }

    public int roleD20UsingAttributeAsModifier(CharacterAttribute attribute) {
        if (attribute == CharacterAttribute.ARMOR_CLASS) return getArmorClass();
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
        int characterPosition = WorldController.instance.getTileIndexFromID(GetComponent<CharacterPosition>().getCurrentInstanceID());
        return currentSkill.isTileWithinRangeOfSkill(WorldController.instance.tileWidth, WorldController.instance.tileHeight, tileIndex, characterPosition);
    }

    public int[] getTilesEffectedByCurrentSkill(int tileIndex) {
        if (isTileWithinRangeOfCurrentSkill(tileIndex)) {
            int characterPosition = WorldController.instance.getTileIndexFromID(GetComponent<CharacterPosition>().getCurrentInstanceID());
            return currentSkill.getTilesAffectedBySkillFromOrigin(WorldController.instance.tileWidth, WorldController.instance.tileHeight, tileIndex, characterPosition);
        } else return new int[] { };
    }

    public int getDamageFromCurrentSkill() {
        return equipedWeapon.GetComponent<Weapon>().getAttackDamage() + currentSkill.getAttackDamage();
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

    void Update () {

	}
}
