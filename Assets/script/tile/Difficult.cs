
public class Difficult : Tile {

    override
    public int getMovementModifier() {
        //If tile is occupied set it's weight to impassable
        return 2 + ((tileIsOccupied) ? 30 : 0);
    }

    override
    public string toString() {
        return "Difficult";
    }

}
