using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LvlType{Erratic, Fast, MedFast, MedSlow, Slow, Fluctuating}
public static class XpNeeded
{
    public static int XpToLevel(LvlType lvlSpeed, int level){
        if(lvlSpeed == LvlType.Fast){
            return Mathf.FloorToInt((4f/5f) * Mathf.Pow(level,3)); 
        } else if( lvlSpeed == LvlType.Erratic){
            if (level < 50){
                return Mathf.FloorToInt((Mathf.Pow(level,3)*(100-level+1))/50);
            } else if (level >= 50 && level < 68) {
                return Mathf.FloorToInt((Mathf.Pow(level,3)*(150-level))/100);
            } else if (level >= 68 && level < 98) {
                return Mathf.FloorToInt((Mathf.Pow(level,3)*((1911-(10*level))/3))/500);
            } else if (level >= 98 && level <= 100) {
                return Mathf.FloorToInt((Mathf.Pow(level,3)*(160-level))/100);
            } else {
                return 0;
            }

        
        
        } else {
            return 0;
        }
    }
    
}
