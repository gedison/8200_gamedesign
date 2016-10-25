public class Armor : Item {
    public int armorClass;

    override
    public string toString() {
        string ret = "";
        ret += itemName+"\n";
        ret += itemDescription + "\n";
        ret += "Armor Class: " + armorClass;
        return ret;
    }
}
