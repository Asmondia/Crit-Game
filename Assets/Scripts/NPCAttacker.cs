using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAttacker : MonoBehaviour,Interactable
{
    public bool beingTalkedTo = false;
    public bool hasAttacked = false;
    public bool hasBattled = false;
    [SerializeField] public List<string> textConvo;
    [SerializeField] public List<string> textConvoAfterBattle;
    [SerializeField] public int searchDistance;
    private int listPos;
    [SerializeField] public Direction directionFacing;
    public LayerMask playerLayer;
    [SerializeField] public List<Crit> critTeamList;
    public CritTeam critTeam;


    public void Start(){
        critTeam = new CritTeam();
        foreach (Crit crit in critTeamList){
            critTeam.addCrit(crit);
        }

    }
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
        } else if (!hasAttacked){
            CheckForPlayer();
        }
        
    }
    private void HandleInput(){
        if(Input.GetKeyDown(KeyCode.E)){
            DisplayNextText();

        }

    }
    private void DisplayNextText(){
        if(!hasBattled){
            if(listPos >= textConvo.Count){
                Debug.Log("Ending Convo");
                GameManager.CloseDialogText();
                StartBattle();
            } else {
                Debug.Log("Displaying Next Text");
                GameManager.SetDialogText(textConvo[listPos]);
                listPos += 1;
            } 
        } else {
            if(listPos >= textConvoAfterBattle.Count){
                Debug.Log("Ending Convo");
                GameManager.CloseDialogText();
                EndConvo();
            } else {
                Debug.Log("Displaying Next Text");
                GameManager.SetDialogText(textConvoAfterBattle[listPos]);
                listPos += 1;
            } 
        }
        
    }
    private void EndConvo(){
        beingTalkedTo = false;
        GameManager.SetGameState(GameState.Default);
    }
    private void CheckForPlayer(){
        
        Vector3 direction = new Vector3(0,-1 * searchDistance,0);

        var collider = Physics2D.Raycast(transform.position, direction,searchDistance, playerLayer).collider;
        if (collider != null){
            Debug.Log("Trying to interact");
            Debug.Log(collider.name);
            if(!collider.GetComponent<PlayerController>().isMoving){
                hasAttacked = true;
                beingTalkedTo = true;
                Interact();
            }
        }
        Debug.DrawRay(transform.position, direction, Color.green);
    }
    private void StartBattle(){

    }
}
