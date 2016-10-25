public class Weapon : Item {

    public int numberOfDice;
    public int numberOfSidesOnDie;

    public int getAttackDamage(){
        Die myDie = new Die(numberOfSidesOnDie);

        int damage = 0;
        for(int i=0; i<numberOfDice; i++)damage += myDie.getDiceRoll();
        return damage;
    }

    override
    public string toString() {
        string ret = "";
        ret += itemName + "\n";
        ret += itemDescription + "\n";
        ret += "Base Damage: " + numberOfDice+" D-"+numberOfSidesOnDie;
        return ret;
    }
}
