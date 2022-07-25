using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectModifier : Modifier
{
    private bool succeeded;
    public ProtectModifier(Crit source) :base(source, ModifierType.MoveBuff){
        succeeded = true;

    }
    public override List<ModifierProperties> GetProperties(){
        List<ModifierProperties> properties = new List<ModifierProperties>();
        properties.Add(ModifierProperties.UseStatusMove);
        return properties;
    }
    public override void StatusEffect(Crit user, Crit target){

        // Debug.Log("Does user have protect cd: " +user.modifiers.Contains(cooldown) );
        ProtectModifierCoolDown cooldown = new ProtectModifierCoolDown(user);
        bool hasCd = false;
        List<Modifier> list = new List<Modifier>(user.modifiers);
        foreach(Modifier mod in list){
            Debug.Log(mod.GetType());
            if(Object.ReferenceEquals(mod.GetType(), cooldown.GetType())){
                hasCd = true;
                user.RemoveModifier(mod);
            };
        }
        Debug.Log("Does user have protect cd: " +hasCd);
        if (hasCd){
            Debug.Log("User had protected before");
            if (Random.Range(1,4) == 1){
                user.AddModifier(new ProtectModifierShield(user));  
            } else{
                succeeded = false;
            }
        } else {
            user.AddModifier(new ProtectModifierShield(user));  
        }
        user.AddModifier(new ProtectModifierCoolDown(user));
    }
    public override string ModifierString(){
        string text;
        if(succeeded){
            text = source.nickname + " protected itself";
        } else {
            text = "but it failed!";
        }
        return text;
    }
}
