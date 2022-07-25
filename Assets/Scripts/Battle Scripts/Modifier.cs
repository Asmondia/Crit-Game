using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Modifier
{
    // Start is called before the first frame update
    protected Crit source;
    protected ModifierType type;
    public Modifier(Crit source, ModifierType type)
    {
        this.source = source;
        this.type = type;
    }
    //List<ModifierProperties> properties = new List<ModifierProperties>();
    public virtual List<ModifierProperties> GetProperties(){
        List<ModifierProperties> properties = new List<ModifierProperties>();
        return properties;
    }
    public virtual void AfterUseMove(Crit user, Crit target){

    }
    public virtual void EndOfTurn(){

    }
    public virtual void DamageTaken(){

    }
    public virtual void StartOfTurn(){

    }
    public virtual void AfterUseMove(){

    }
    public virtual DamageDetails AffectDamageTarget(Crit user, Crit target,DamageDetails damageDets){
        return damageDets;
    }
    public virtual DamageDetails AffectDamageUser(Crit user, Crit target,DamageDetails damageDets){
        return damageDets;
    }
    public virtual string ModifierString(){
        return "";
    }
    public virtual void OnDamageDealt(DamageDetails damageDets, Crit user, Crit target){

    }
    public virtual void StatusEffect(Crit user, Crit target){

    }
    public static Modifier GetModifierObject(ModifierName modName, Crit source){
        switch(modName){
            case(ModifierName.None):
                return null;
            case(ModifierName.Takedown):
                return new TakedownModifer(source);
            case(ModifierName.Protect):
                return new ProtectModifier(source);
            default:
                Debug.Log("Modifier name not found");
                return null;
        }
    }
}
public enum ModifierProperties{
    DamageTaken, EndOfTurn, OnDamageDealt , StartOfTurn, OnHit, AfterUseMove,UseStatusMove, AffectDamageTarget, AffectDamageUser
}
public enum ModifierType{
    MoveDebuff, MoveBuff, Item, Ability
}

public enum ModifierName{
    None, Takedown, Protect
}