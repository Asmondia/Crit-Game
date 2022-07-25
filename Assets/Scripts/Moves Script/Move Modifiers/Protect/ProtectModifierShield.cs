using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectModifierShield : Modifier
{


    // Start is called before the first frame update
    public ProtectModifierShield(Crit source) :base(source, ModifierType.MoveBuff){
    }

    public override List<ModifierProperties> GetProperties(){
        List<ModifierProperties> properties = new List<ModifierProperties>();
        properties.Add(ModifierProperties.AffectDamageTarget);
        properties.Add(ModifierProperties.EndOfTurn);
        return properties;
    }
    public override DamageDetails AffectDamageTarget(Crit user, Crit target, DamageDetails damageDets){
        if (damageDets.didHit){
            damageDets.didHit = false;
            damageDets.damageDealt = 0;
            damageDets.storedString.Add(target.nickname + " blocked the attack with protect!"); 
            damageDets.blocked = true;
            
        }
        
        return damageDets;
    }
    public override void EndOfTurn(){
        Debug.Log("Trying to remove shield");
        if(source.modifiers.Contains(this)){
            Debug.Log("Removed Shield");
            source.RemoveModifier(this);
        }
    }

}
