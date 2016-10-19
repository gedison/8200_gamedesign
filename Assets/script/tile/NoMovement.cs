
public class NoMovement : Tile {

    override
    public int getMovementModifier() {
        return 10000;
    }

    override
    public string toString() {
        return "No Movement";
    }
}
