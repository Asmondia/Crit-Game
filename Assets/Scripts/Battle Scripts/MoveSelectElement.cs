using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MoveSelectElement : MonoBehaviour
{
    [SerializeField] Text moveText;

    public bool canSelect;

    public Move storedMove;
    public void setMoveText(string text){
        moveText.text = text;
    }
    
    public void setInactive(){
        moveText.text = "-";
        canSelect = false;

    }
    public void setMove(Move move){
        this.storedMove = move;
        canSelect = true;
        setMoveText(storedMove.baseMove.moveName);
    }
    public void changeColour(Color colour){
        moveText.color = colour;
    }
}
