using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum SelectedMove{One,Two,Three,Four};
public class MoveSelect : MonoBehaviour
{
    [SerializeField] MoveSelectElement moveOne;
    [SerializeField] MoveSelectElement moveTwo;
    [SerializeField] MoveSelectElement moveThree;
    [SerializeField] MoveSelectElement moveFour;
    [SerializeField] Color baseColour;
    [SerializeField] Color selectedColour;

    private SelectedMove currentMove;
    public Move SetUp(List<Move> critMoves){
        currentMove = SelectedMove.One;
        int numberOfMoves = 0;
        foreach(Move move in critMoves){
            if (numberOfMoves == 0){
                moveOne.setMove(move);
            } else if (numberOfMoves == 1){
                moveTwo.setMove(move);
            } else if (numberOfMoves == 2){
                moveThree.setMove(move);
            } else if (numberOfMoves == 3){
                moveFour.setMove(move);
            } 
            numberOfMoves += 1;
        }
        int emptySpaces = 0;
        while (numberOfMoves < 4){
            if (emptySpaces == 0){
                Debug.Log("4 Inactive");
                moveFour.setInactive();
            } else if (emptySpaces == 1){
                moveThree.setInactive();
            } else if (emptySpaces == 2){
                moveTwo.setInactive();
            }
            numberOfMoves += 1;
            emptySpaces += 1;

        }
        selectOption(currentMove);

        return getCurrentSelectedMove();
    }

    public Move getCurrentSelectedMove(){
        switch(currentMove){
            case(SelectedMove.One):
                return moveOne.storedMove;
                
            case(SelectedMove.Two):
                return moveTwo.storedMove;
                
            case(SelectedMove.Three):
                return moveThree.storedMove;
                
            case(SelectedMove.Four):
                return moveFour.storedMove;
                
            default:
                return null;
        }
    }
    public void selectOption(SelectedMove moveSelect){
        if (moveSelect != SelectedMove.One){
            moveOne.changeColour(baseColour);
        } else {
            moveOne.changeColour(selectedColour);
        }
        if (moveSelect != SelectedMove.Two){
            moveTwo.changeColour(baseColour);
        }else {
            moveTwo.changeColour(selectedColour);
        }
        if (moveSelect != SelectedMove.Three){
            moveThree.changeColour(baseColour);
        }else {
            moveThree.changeColour(selectedColour);
        }
        if (moveSelect != SelectedMove.Four){
            moveFour.changeColour(baseColour);
        }else {
            moveFour.changeColour(selectedColour);
        }
    }
    public Move changeSelectedMove(Direction direction){
        switch(currentMove){
            case(SelectedMove.One):
                if(direction == Direction.Right && moveTwo.canSelect){
                    currentMove = SelectedMove.Two;
                } else if(direction == Direction.Down && moveThree.canSelect){
                    currentMove = SelectedMove.Three;
                }
                break;
            case(SelectedMove.Two):
                if(direction == Direction.Left && moveOne.canSelect){
                    currentMove = SelectedMove.One;
                } else if(direction == Direction.Down && moveFour.canSelect){
                    currentMove = SelectedMove.Four;
                }
                break;
            case(SelectedMove.Three):
                if(direction == Direction.Up && moveOne.canSelect){
                    currentMove = SelectedMove.One;
                } else if(direction == Direction.Right && moveFour.canSelect){
                    currentMove = SelectedMove.Four;
                }
                break;
            case(SelectedMove.Four):
                if(direction == Direction.Left && moveThree.canSelect){
                    currentMove = SelectedMove.Three;
                } else if(direction == Direction.Up && moveTwo.canSelect){
                    currentMove = SelectedMove.Two;
                }
                break;
        }
        selectOption(currentMove);
        Debug.Log("Now Selecting " + getCurrentSelectedMove().baseMove.moveName);
        return getCurrentSelectedMove();

    }
    public void TrySetMove(Move move){
        if (moveOne.storedMove == move){
            currentMove = SelectedMove.One;
        } else if (moveTwo.storedMove == move){
            currentMove = SelectedMove.Two;
        } else if (moveThree.storedMove == move){
            currentMove = SelectedMove.Three;
        } else if (moveFour.storedMove == move){
            currentMove = SelectedMove.Four;
        } else {
            currentMove = SelectedMove.One;
        }
        selectOption(currentMove);
    }

    
}
