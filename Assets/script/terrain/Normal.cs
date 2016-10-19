
public class Normal : Tile {

    override
    public int getMovementModifier() {
        return 1;
    }

    override
    public string toString() {
        return "Normal";
    }
}
