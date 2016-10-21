
public class NoMovement : Tile {

    override
    public int getMovementModifier() {
        return 30;
    }

    override
    public string toString() {
        return "No Movement";
    }
}
