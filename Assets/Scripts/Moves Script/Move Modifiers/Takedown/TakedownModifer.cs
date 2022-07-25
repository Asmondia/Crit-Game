using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakedownModifer : Modifier
{
    public TakedownModifer(Crit source) :base(source, ModifierType.MoveDebuff){

    }
    
    public override List<ModifierProperties> GetProperties(){
        List<ModifierProperties> properties = new List<ModifierProperties>();
        properties.Add(ModifierProperties.OnDamageDealt);
        return properties;
    }
    public override void OnDamageDealt(DamageDetails damageDets, Crit user, Crit target){
        Debug.Log("Take down mod on damage dealt");
        source.AddModifier(new TakedownSelfDamageModifier(user, damageDets.damageDealt)); 
    }
}
