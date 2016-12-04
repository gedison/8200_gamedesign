using UnityEngine;

public class AudioController : MonoBehaviour {

    public float audioFadePerFrame = .4f;
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

    private float timeOut;
    private float timeIn = 0.0f;

    void Start () {
        timeOut = getSecondsToGetMaxVolume(maxAudio);

        currentAudio = outdoorMusic;
        nextAudio = outdoorMusic;

        audioSource = this.GetComponent<AudioSource>();
        audioSource.clip = currentAudio;
        audioSource.loop = true;
        audioSource.volume = maxAudio;
        audioSource.Play();
    }

    private float getVolumeAtXSeconds(float seconds) {
        //Gives the volume fade a parabola shape, fades out quickly at first and then slows down as it approaches zero
        return Mathf.Pow(Mathf.Asin(seconds), 2);
    }

    private float getSecondsToGetMaxVolume(float maxVolume) {
        return Mathf.Sqrt(Mathf.Sin(maxVolume));
    }
    
    private void fadeAudioOut() {
        audioSource.volume = getVolumeAtXSeconds(timeOut);
        timeOut -= Time.deltaTime*audioFadePerFrame;
        if(timeOut <= 0) {
            timeOut = getSecondsToGetMaxVolume(maxAudio);
            currentAudio = nextAudio;
            audioSource.clip = currentAudio;
            audioSource.Play();
        }
    }

    private void fadeAudioIn() {
        audioSource.volume = getVolumeAtXSeconds(timeIn);
        timeIn += Time.deltaTime * audioFadePerFrame;
    }
	
	void Update () {
        if (!currentAudio.Equals(nextAudio)) {
            fadeAudioOut();
        } else if (audioSource.volume <= maxAudio) {
            fadeAudioIn();
        } else {
            timeIn = 0.0f;
        }

        //The world's audio is determined by where the player is in the dungeon and if they are in combat or not, because
        //of the way our dungeon is constructed this can be determined by the z-coordinate exclusively
        float zPosition = 0;
        if (musicFollowsMyZCoordinate != null) {
            zPosition = musicFollowsMyZCoordinate.transform.position.z;
        }

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
                    nextAudio = caveMusic;
                }
                break;
            case WorldController.GameState.IN_COMBAT:
                //Not Boss
                if (zPosition < bossStartZCoordinate) {
                    nextAudio = fightMusic;
                //Boss
                } else {
                    nextAudio = fightMusic;
                }
                break;
        }
	}
}
