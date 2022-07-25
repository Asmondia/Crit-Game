using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakedownSelfDamageModifier : Modifier
{
    int recoilDamage;
    public TakedownSelfDamageModifier(Crit source, int damageDealt) :base(source, ModifierType.MoveDebuff){
        Debug.Log("Damage Dealth True: " + damageDealt);
        int quartDamage = Mathf.FloorToInt(damageDealt/4);
        Debug.Log("Damage Dealth Quart: " + quartDamage);
        recoilDamage = Mathf.Max(quartDamage,1);

    }
    public override List<ModifierProperties> GetProperties(){
        List<ModifierProperties> properties = new List<ModifierProperties>();
        properties.Add(ModifierProperties.AfterUseMove);
        return properties;
    }

    public override void AfterUseMove(){
        source.takeDamage(recoilDamage);
    }
    public override string ModifierString(){
        return source.nickname + " took recoil damage";
    }
  
}
