using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction{ Left, Right, Up, Down};

public enum Action{Fight, Swap, Bag, Flee}
public class BattleOptions : MonoBehaviour
{
    [SerializeField] BattleDialogBoxElement fightOption;
    [SerializeField] BattleDialogBoxElement critSwapOption;
    [SerializeField] BattleDialogBoxElement bagOption;
    [SerializeField] BattleDialogBoxElement fleeOption;

    [SerializeField] Color baseColour;
    [SerializeField] Color selectedColour;

    private Action currentAction = Action.Fight;

    public Action resetAction(){
        this.currentAction = Action.Fight;
        selectOption(currentAction);
        return currentAction;
    }

    public void selectOption(Action action){
        if (action != Action.Fight){
            fightOption.changeColour(baseColour);
        } else {
            fightOption.changeColour(selectedColour);
        }
        if (action != Action.Swap){
            critSwapOption.changeColour(baseColour);
        }else {
            critSwapOption.changeColour(selectedColour);
        }
        if (action != Action.Flee){
            fleeOption.changeColour(baseColour);
        }else {
            fleeOption.changeColour(selectedColour);
        }
        if (action != Action.Bag){
            bagOption.changeColour(baseColour);
        }else {
            bagOption.changeColour(selectedColour);
        }
    }
    

    public Action moveSelected(Direction direction){
        Action action = currentAction;
        switch(action){
            case (Action.Fight):
            if (direction == Direction.Down){
                currentAction = Action.Bag;
                selectOption(currentAction);
            } else if(direction == Direction.Right){
                currentAction = Action.Swap;
                selectOption(currentAction);
            }
            break;
            case (Action.Swap):
            if (direction == Direction.Down){
                currentAction = Action.Flee;
                selectOption(currentAction);
            } else if(direction == Direction.Left){
                currentAction = Action.Fight;
                selectOption(currentAction);
            }
            break;
            case (Action.Bag):
            if (direction == Direction.Up){
                currentAction = Action.Fight;
                selectOption(currentAction);
            } else if(direction == Direction.Right){
                currentAction = Action.Flee;
                selectOption(currentAction);
            }
            break;
            case (Action.Flee):
            if (direction == Direction.Left){
                currentAction = Action.Bag;
                selectOption(currentAction);
            } else if(direction == Direction.Up){
                currentAction = Action.Swap;
                selectOption(currentAction);
            }
            break;

        }



        return currentAction;
    }
    
    
    
}
