using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour {

    public static TitleController instance = null;
    public GameObject torches;
    public Button startGame;

    private List<GameObject> myTorches = new List<GameObject>();
    private bool putOutTorches = false;
    private int numberOfTorchesPutOut = 0;

    private float delay = .3f;
    private float time = 0.0f;

    void Awake() {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);
        StartMenu();
    }

    private void StartMenu() {
        startGame.onClick.AddListener(StartGame);


        foreach(Transform torch in torches.transform) {
            myTorches.Add(torch.gameObject);
        }
    }

    private void StartGame() {

        startGame.gameObject.SetActive(false);
        putOutTorches = true;
    }

    void Update() {
        if (putOutTorches) {
            if (time >= delay) {
                if (numberOfTorchesPutOut <= 6) {
                    myTorches[numberOfTorchesPutOut].SetActive(false);
                    myTorches[numberOfTorchesPutOut + 1].SetActive(false);
                    numberOfTorchesPutOut += 2;
                } else {
                    SceneManager.LoadScene("prototype");
                }
                time = 0;
            } else time += Time.deltaTime;
        }

    }
}
