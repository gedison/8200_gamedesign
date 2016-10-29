
public class Normal : Tile {

    override
    public int getMovementModifier() {
        return 1 + ((tileIsOccupied) ? 30 : 0);
    }

    override
    public string toString() {
        return "Normal";
    }
}
