using UnityEngine;
using System.Collections;

public class ExperienceManager : MonoBehaviour {
	int[] experiences = {200, 500, 1000, 2000};

	// Determine what level the character might be
	public bool hasLeveledUp(int level, int experience) {
		bool result = false;
		for(int i = 0; i < experiences.Length; ++i) {
			if (level == i && experience > experiences [i]) {
				result = true;
			}
		}

		return result;
	}

	public void setExperience(int[] nExperiences) {
		experiences = nExperiences;
	}

	public void alterExperienceAt(int value, int loc) {
		experiences [loc] = value;
	}
}
