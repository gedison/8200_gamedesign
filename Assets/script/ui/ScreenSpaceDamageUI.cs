using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ScreenSpaceDamageUI : MonoBehaviour {

    public Canvas canvas;
    public GameObject damagePrefab;

    public float offset = 1.0f;
    private float heightIncreasePerSecond = 2f;
    private float height = 0;

    private List<float> heights = new List<float>();
    private List<GameObject> damageList = new List<GameObject>();



    public void createDamageText(string damageString) {
        GameObject damage = Instantiate(damagePrefab) as GameObject;
        damage.transform.SetParent(canvas.transform, false);
        Text damageText = damage.GetComponent<Text>();
        damageText.text = damageString;
        damageText.color = new Color(1, 0, 0);

        damageList.Add(damage);
        heights.Add(0);
    }

    public void createHealText(string damageString) {
        GameObject damage = Instantiate(damagePrefab) as GameObject;
        damage.transform.SetParent(canvas.transform, false);
        Text damageText = damage.GetComponent<Text>();
        damageText.text = damageString;
        damageText.color = new Color(0, 1, 0);

        damageList.Add(damage);
        heights.Add(0);
    }

    void LateUpdate() {
        for(int i = damageList.Count-1; i>=0; i--) {
            heights[i] +=  heightIncreasePerSecond * Time.deltaTime;     
            if (damageList[i] != null) {
                if (heights[i] > 5) {
                    GameObject toBeDestroyed = damageList[i];
                    damageList.Remove(toBeDestroyed);
                    heights.RemoveAt(i);
                    Destroy(toBeDestroyed);
                }else { 
                    Text myDamageText = damageList[i].GetComponent<Text>();
                    Vector3 worldPos = new Vector3(transform.position.x, transform.position.y + offset + heights[i], transform.position.z);
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
                    myDamageText.transform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z);
                }
            } else {
                damageList.RemoveAt(i);
                heights.RemoveAt(i);
            }
        }
    }

    public void destroyObject() {
        for (int i = damageList.Count - 1; i >= 0; i--) {
            Destroy(damageList[i]);
        }
    }
    

    
}
