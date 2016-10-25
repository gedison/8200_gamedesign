using System;

public class Die {
    private int numberOfSidesOnDie;
    
    public Die(int numberOfSidesOnDie) {
        this.numberOfSidesOnDie = numberOfSidesOnDie;
    }

    public int getDiceRoll() {
        Random r = new Random();
        return r.Next(1, numberOfSidesOnDie);
    }
}
