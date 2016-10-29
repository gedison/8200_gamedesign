
public class Difficult : Tile {

    override
    public int getMovementModifier() {
        return 2 + ((tileIsOccupied) ? 30 : 0);
    }

    override
    public string toString() {
        return "Difficult";
    }

}
