using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TeamMember{
    public Crit crit;
    public int teamPosition;
    public TeamMember(Crit crit, int teamPosition){
        this.crit = crit;
        this.teamPosition = teamPosition;
    }
}

public class CritTeam
{
    [SerializeField] CritBase critBase;
    public Crit critOne;
    public Crit critTwo;
    public Crit critThree;
    public Crit critFour;
    public Crit critFive;
    public Crit critSix;

    public CritTeam(){
    }

    public void addCrit(Crit crit){
        if (anyEmptySlot()){
            placeInNextEmptySlot(crit);
        }

    }
    public void placeInNextEmptySlot(Crit crit){
        if (critOne == null){
            critOne = crit;
        } else if (critTwo == null) {
            critTwo = crit;
        }else if (critThree == null) {
            critThree = crit;
        } else if (critFour == null){
            critFour = crit;
        } else if (critFive == null) {
            critFive = crit;
        }else if (critThree == null) {
            critSix = crit;
        }
    }

    public bool anyEmptySlot(){
        if (critOne != null && critTwo !=null && critThree != null && critFour != null && critFive !=null && critSix != null){
            return false;
        } else{
            return true;
        }
    }
    
    public Crit getNextCrit(){
        if(critOne != null){
            if (critOne.HP > 0){
                return critOne;
            }
        } else if(critTwo != null){
            if (critTwo.HP > 0){
                return critTwo;
            }
        } else if(critThree != null){
            if (critThree.HP > 0){
                return critThree;
            }
        } else if(critFour != null){
            if (critFour.HP > 0){
                return critFour;
            }
        } else if(critFive != null){
            if (critFive.HP > 0){
                return critFive;
            }
        } else if(critSix != null){
            if (critSix.HP > 0){
                return critSix;
            }
        } 
        return null;
    }
}
