using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CombatUIController : MonoBehaviour {

    public GameObject player;
    public Button movementButton;
    public Button endTurnButton;
    public GameObject skillPanel;

    public Color skillIsAvailable = new Color(1, 1, .45f);
    public Color skillIsUnavailable = new Color(.66f, .67f, .7f);

    private CharacterController myCharacterController;
    private ArrayList skills = new ArrayList();
    private bool setSkillNames = false;

    void Start () {
        myCharacterController = player.GetComponent<CharacterController>();
        movementButton.onClick.AddListener(setPlayerToMove);
        endTurnButton.onClick.AddListener(endPlayerTurn);

        Button[] buttons = skillPanel.GetComponentsInChildren<Button>();
        for(int i = 0; i < buttons.Length; i++) {
            int tempInt = i;
            buttons[i].onClick.AddListener(() => setPlayerToAttackUsingSkill(tempInt));
        }
    }

    private void setPlayerToAttackUsingSkill(int skillID) {
        if (player != null && WorldController.instance.isPlayersTurn) {
            myCharacterController.setSkillAtIndexToActive(skillID);
        }
    }

    private void setPlayerToMove() {
        if (player != null && WorldController.instance.isPlayersTurn) myCharacterController.setCurrentCharacterState(CharacterController.CharacterState.MOVE);
    }

    private void endPlayerTurn() {
        if (player != null) myCharacterController.endTurn();
    }

    public void setMovementSelected() {
        movementButton.Select();
    }

    public void Update() {
        if (!setSkillNames) {
            Button[] buttons = skillPanel.GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++) {
                Skill skill = myCharacterController.getSkillAtIndex(i);
                if (skill != null) {
                    Debug.Log(skill.getSkillName());
                    buttons[i].GetComponentInChildren<Text>().text = skill.getSkillName();
                    buttons[i].GetComponent<ToolTipTrigger>().setSkill(skill);
                    setSkillNames = true;
                }
            }


            //Set Move ToolTip
            movementButton.GetComponent<ToolTipTrigger>().setMove(myCharacterController.getMovementSpeed());
            endTurnButton.GetComponent<ToolTipTrigger>().setTipToValues(new string[] { "End Turn", "N/A", "You're finished with your turn or you've ran out of AP, press this button to allow the rest of the encounter to procede.", "No Damage"});
        }

        if(player!=null && myCharacterController.getCurrentActionPoints() == 5) {
            ColorBlock temp = movementButton.colors;
            temp.normalColor = skillIsAvailable;
            movementButton.colors = temp;

            Button[] buttons = skillPanel.GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++) {
                temp = buttons[i].colors;
                Skill skill = myCharacterController.getSkillAtIndex(i);
                if (skill.hasSkillBeenUsedUpForEncounter()) temp.normalColor = skillIsUnavailable;
                else temp.normalColor = skillIsAvailable;
                buttons[i].colors = temp;
            }
        }

        if (player != null && myCharacterController.getCurrentActionPoints() < 3) {
            Button[] buttons = skillPanel.GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++) {
                ColorBlock temp = buttons[i].colors;
                temp.normalColor = skillIsUnavailable;
                buttons[i].colors = temp;
            }
        }

        if(player != null && myCharacterController.getCurrentActionPoints() < 2) {
            ColorBlock temp = movementButton.colors;
            temp.normalColor = skillIsUnavailable;
            movementButton.colors = temp;
        }





    }
}
