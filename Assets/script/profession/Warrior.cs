public class Warrior : Profession {

    public int getBaseHealth() {
        return 20;
    }

    public int getBaseAttribute(CharacterController.CharacterAttribute attribute) {
        int attributeValue = 0;
        switch (attribute) {
            case CharacterController.CharacterAttribute.STRENGTH: attributeValue = 16;  break;
            case CharacterController.CharacterAttribute.DEXTERITY: attributeValue = 6; break;
            case CharacterController.CharacterAttribute.INTELLIGENCE: attributeValue = 2; break;
            case CharacterController.CharacterAttribute.WISDOM: attributeValue = 2; break;
        }

        return attributeValue;
    }

    public int getBaseMovement(){
        return 6;
    }

    public string toString(){
        return "Warrior";
    }
}
