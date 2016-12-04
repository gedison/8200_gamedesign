
public class Normal : Tile {

    override
    public int getMovementModifier() {
        //If tile is occupied set it's weight to impassable
        return 1 + ((tileIsOccupied) ? 30 : 0);
    }

    override
    public string toString() {
        return "Normal";
    }
}
