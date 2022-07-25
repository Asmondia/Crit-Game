using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour,Interactable
{
    public bool beingTalkedTo = false;
    [SerializeField] public List<string> textConvo;
    private int listPos;

    public void Interact(){
        GameManager.SetGameState(GameState.Text);
        listPos = 0;
        beingTalkedTo = true;
        Debug.Log("Interacting with NPC");
        DisplayNextText();
    }
    public void Update(){
        if(beingTalkedTo){
            HandleInput();
        }
    }
    private void HandleInput(){
        if(Input.GetKeyDown(KeyCode.E)){
            DisplayNextText();

        }

    }
    private void DisplayNextText(){
        if(listPos >= textConvo.Count){
            Debug.Log("Ending Convo");
            GameManager.CloseDialogText();
            EndConvo();
        } else {
            Debug.Log("Displaying Next Text");
            GameManager.SetDialogText(textConvo[listPos]);
            listPos += 1;
        }
    }
    private void EndConvo(){
        beingTalkedTo = false;
        GameManager.SetGameState(GameState.Default);
    }
}