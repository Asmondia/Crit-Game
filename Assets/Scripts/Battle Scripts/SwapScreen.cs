using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapScreen : MonoBehaviour
{
    private enum Location{TopLeft,TopRight,MiddleLeft,MiddleRight,BottomLeft,BottomRight}
    [SerializeField] SwapElement Crit1;
    [SerializeField] SwapElement Crit2;
    [SerializeField] SwapElement Crit3;
    [SerializeField] SwapElement Crit4;
    [SerializeField] SwapElement Crit5;
    [SerializeField] SwapElement Crit6;

    private Location currentSelected;

    public void Setup(CritTeam team){
        currentSelected = Location.TopLeft;
        Crit1.gameObject.SetActive(true);
        Crit2.gameObject.SetActive(true);
        Crit3.gameObject.SetActive(true);
        Crit4.gameObject.SetActive(true);
        Crit5.gameObject.SetActive(true);
        Crit6.gameObject.SetActive(true);

        if (team.critOne != null){
            Crit1.Setup(team.critOne);
            if (team.critOne.HP > 0){
                Crit1.canSelect = true;
            } else {
                Crit1.canSelect = false;
            }
        } else {
            Crit1.canSelect = false;
            Crit1.gameObject.SetActive(false);
        }
        if (team.critTwo != null){
            Crit2.Setup(team.critTwo);
            if (team.critTwo.HP > 0){
                Crit2.canSelect = true;
            } else {
                Crit2.canSelect = false;
            }
        }else {
            Crit2.canSelect = false;
            Crit2.gameObject.SetActive(false);
        }
        if (team.critThree != null){
            Crit3.Setup(team.critThree);
            if (team.critThree.HP > 0){
                Crit3.canSelect = true;
            } else {
                Crit3.canSelect = false;
            }
        } else {
            Crit3.canSelect = false;
            Crit3.gameObject.SetActive(false);
        }
        if (team.critFour != null){
            Crit4.Setup(team.critFour);
            if (team.critFour.HP > 0){
                Crit4.canSelect = true;
            } else {
                Crit4.canSelect = false;
            }
        } else {
            Crit4.canSelect = false;
            Crit4.gameObject.SetActive(false);
        }
        if (team.critFive != null){
            Crit5.Setup(team.critFive);
            if (team.critFive.HP > 0){
                Crit5.canSelect = true;
            } else {
                Crit5.canSelect = false;
            }
        }else {
            Crit5.canSelect = false;
            Crit5.gameObject.SetActive(false);
        }
        if (team.critSix != null){
            Crit6.Setup(team.critSix);
            if (team.critSix.HP > 0){
                Crit6.canSelect = true;
            } else {
                Crit6.canSelect = false;
            }
        } else {
            Crit6.canSelect = false;
            Crit6.gameObject.SetActive(false);
        }
        ToggleSelected();
        
    }

    public Crit getSelected(){
        switch(currentSelected){
            case(Location.TopLeft):
                return Crit1.storedCrit;
            case(Location.TopRight):
                return Crit2.storedCrit;
            case(Location.MiddleLeft):
                return Crit3.storedCrit;
            case(Location.MiddleRight):
                return Crit4.storedCrit;
            case(Location.BottomLeft):
                return Crit5.storedCrit;
            case(Location.BottomRight):
                return Crit6.storedCrit;
            
        }
        Debug.Log("Selected Crit Not found");
        return null;
    }
    public void ToggleSelected(){
        if (currentSelected == Location.TopLeft){
            Crit1.Selected();
        } else {
            Crit1.Unselected();
        }
        if (currentSelected == Location.TopRight){
            Crit2.Selected();
        } else {
            Crit2.Unselected();
        }
        if (currentSelected == Location.MiddleLeft){
            Crit3.Selected();
        } else {
            Crit3.Unselected();
        }
        if (currentSelected == Location.MiddleRight){
            Crit4.Selected();
        } else {
            Crit4.Unselected();
        }
        if (currentSelected == Location.BottomLeft){
            Crit5.Selected();
        } else {
            Crit5.Unselected();
        }
        if (currentSelected == Location.BottomRight){
            Crit6.Selected();
        } else {
            Crit6.Unselected();
        }
        
    }
    public void ChangeSelected(Direction direction){
        switch(currentSelected){
            case(Location.TopLeft):
                if(direction == Direction.Right){
                    trySwapCurrentLocation(Location.TopRight);
                } else if (direction == Direction.Down){
                    trySwapCurrentLocation(Location.MiddleLeft);
                }
                break;
            case(Location.TopRight):
                if(direction == Direction.Left){
                    trySwapCurrentLocation(Location.TopLeft);
                } else if (direction == Direction.Down){
                    trySwapCurrentLocation(Location.MiddleRight);
                }
                break;
            case(Location.MiddleLeft):
                if(direction == Direction.Right){
                    trySwapCurrentLocation(Location.MiddleRight);
                } else if (direction == Direction.Down){
                    trySwapCurrentLocation(Location.BottomLeft);
                } else if(direction == Direction.Up){
                    trySwapCurrentLocation(Location.TopLeft);
                }
                break;
            case(Location.MiddleRight):
                if(direction == Direction.Left){
                    trySwapCurrentLocation(Location.MiddleLeft);
                } else if (direction == Direction.Down){
                    trySwapCurrentLocation(Location.BottomRight);
                }else if(direction == Direction.Up){
                    trySwapCurrentLocation(Location.TopRight);
                }
                break;
            case(Location.BottomLeft):
                if(direction == Direction.Right){
                    trySwapCurrentLocation(Location.BottomRight);
                } else if (direction == Direction.Up){
                    trySwapCurrentLocation(Location.MiddleLeft);
                }
                break;
            case(Location.BottomRight):
                if(direction == Direction.Left){
                    trySwapCurrentLocation(Location.BottomLeft);
                } else if (direction == Direction.Up){
                    trySwapCurrentLocation(Location.MiddleRight);
                }
                break;

        }
        ToggleSelected();
    }

    private void trySwapCurrentLocation(Location tryLocation){
        switch(tryLocation){
            case(Location.TopLeft):
                if(Crit1.canSelect){
                    currentSelected = Location.TopLeft;
                }
                break;
            case(Location.TopRight):
                if(Crit2.canSelect){
                    currentSelected = Location.TopRight;
                }
                break;
            case(Location.MiddleLeft):
                if(Crit3.canSelect){
                    currentSelected = Location.MiddleLeft;
                }
                break;
            case(Location.MiddleRight):
                if(Crit4.canSelect){
                    currentSelected = Location.MiddleRight;
                }
                break;
            case(Location.BottomLeft):
                if(Crit5.canSelect){
                    currentSelected = Location.BottomLeft;
                }
                break;
            case(Location.BottomRight):
                if(Crit6.canSelect){
                    currentSelected = Location.BottomLeft;
                }
                break;
            
        }
    }
}
