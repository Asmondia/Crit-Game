using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectModifierCoolDown : Modifier
{
    private int countdown;
    public ProtectModifierCoolDown(Crit source) :base(source, ModifierType.MoveBuff){
        this.countdown = 1;

    }
    public override void EndOfTurn(){
        if(source.modifiers.Contains(this)){
            if(countdown == 0){
                source.RemoveModifier(this);
            }
            countdown -= 1;
            Debug.Log("This is in " + source.nickname);
        }
    }
     public override List<ModifierProperties> GetProperties(){
        List<ModifierProperties> properties = new List<ModifierProperties>();
        properties.Add(ModifierProperties.EndOfTurn);
        return properties;
    }
}
