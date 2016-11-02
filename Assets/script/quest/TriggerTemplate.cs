using System.Collections;

/* This class will act as an abstract base class for any trigger. It can take a quest
 * to be started whenever the trigger is activated.
 */

public abstract class TriggerTemplate {
	// Activates the trigger; does nothing if it is not triggerable
	public abstract void activate ();

	// Queries whether or not the trigger can activate
	public abstract bool triggerable ();

	// Set whether or not the trigger can actually activate
	public abstract void canBeTriggered (bool val);
}
