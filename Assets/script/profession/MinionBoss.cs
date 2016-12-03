public class MinionBoss : Profession {

    public int getBaseHealth() {
        return 12;
    }

    public int getBaseAttribute(CharacterController.CharacterAttribute attribute) {
        int attributeValue = 0;
        switch (attribute) {
            case CharacterController.CharacterAttribute.STRENGTH: attributeValue = 10; break;
            case CharacterController.CharacterAttribute.DEXTERITY: attributeValue = 3; break;
            case CharacterController.CharacterAttribute.INTELLIGENCE: attributeValue = 2; break;
            case CharacterController.CharacterAttribute.WISDOM: attributeValue = 2; break;
        }

        return attributeValue;
    }

    public int getBaseMovement() {
        return 4;
    }

    public string toString() {
        return "Minion Boss";
    }
}
