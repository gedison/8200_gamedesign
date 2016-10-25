using System;

public class Die {
    private int numberOfSidesOnDie;
    private Random r = new Random();

    public Die(int numberOfSidesOnDie) {
        this.numberOfSidesOnDie = numberOfSidesOnDie;
    }

    public int getDiceRoll() {
        return r.Next(1, numberOfSidesOnDie);
    }
}
