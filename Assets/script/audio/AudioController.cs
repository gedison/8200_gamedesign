using UnityEngine;
using System.Collections;

public class AudioController : MonoBehaviour {

    public float audioFade = .4f;
    public float maxAudio = .5f;

    public AudioClip outdoorMusic;
    public AudioClip fightMusic;
    public AudioClip caveMusic;
    public AudioClip bossMusic;

    public GameObject musicFollowsMyZCoordinate;

    private AudioSource audioSource;
    private AudioClip currentAudio;
    private AudioClip nextAudio;
    private float caveStartZCoordinate = 10.5f;
    private float bossStartZCoordinate = 30.5f;
    private bool wierdCombatStartBugFix = true;

    private float timeOut;
    private float timeIn = 0.0f;

    void Start () {
        timeOut = Mathf.Sqrt(Mathf.Sin(maxAudio));

        audioSource = this.GetComponent<AudioSource>();
        currentAudio = outdoorMusic;
        nextAudio = outdoorMusic;
        audioSource.clip = currentAudio;
        audioSource.loop = true;
        audioSource.volume = maxAudio;
        audioSource.Play();
    }

    
    private void fadeAudioOut() {
        audioSource.volume = Mathf.Pow(Mathf.Asin(timeOut),2);
        timeOut -= Time.deltaTime*audioFade;
        if(timeOut <= 0) {
            timeOut = Mathf.Sqrt(Mathf.Sin(maxAudio));
            currentAudio = nextAudio;
            audioSource.clip = currentAudio;
            audioSource.Play();
        }
    }

    private void fadeAudioIn() {
        audioSource.volume = Mathf.Pow(Mathf.Asin(timeIn), 2);
        timeIn += Time.deltaTime * audioFade;
    }
	
	void Update () {
        if (!currentAudio.Equals(nextAudio)) {
            fadeAudioOut();
        } else if (audioSource.volume <= maxAudio) {
            fadeAudioIn();
        } else {
            timeIn = 0.0f;
        }

        float zPosition = musicFollowsMyZCoordinate.transform.position.z;
        switch (WorldController.instance.currentState) {
            case WorldController.GameState.IDLE:
                //Outside
                if (zPosition < caveStartZCoordinate) {
                    nextAudio = outdoorMusic;
                //Cave
                } else if (zPosition < bossStartZCoordinate) {
                    nextAudio = caveMusic;
                //Boss
                } else {
                    nextAudio = bossMusic;
                }
                break;
            case WorldController.GameState.IN_COMBAT:
                //Not Boss
                if (zPosition < bossStartZCoordinate) {
                    if (wierdCombatStartBugFix) wierdCombatStartBugFix = false;
                    else nextAudio = fightMusic;
                //Boss
                } else {
                    nextAudio = bossMusic;
                }
                break;
        }
	}
}
