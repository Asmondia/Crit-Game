using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
Written by Ben Maddison - Based on learning of Dota's Lua modding API
**/
public abstract class Modifier
{
    // Constructor for modifier
    protected Crit source;
    protected ModifierType type;
    public Modifier(Crit source, ModifierType type)
    {
        this.source = source;
        this.type = type;
    }
    /**
    All of the properties this modifier should respond to
    **/
    public virtual List<ModifierProperties> GetProperties(){
        List<ModifierProperties> properties = new List<ModifierProperties>();
        return properties;
    }

    /**
    Called at the end of the turn
    **/
    public virtual void EndOfTurn(){

    }
    /**
    Called after damage is taken
    **/
    public virtual void DamageTaken(){

    }
    /**
    Called at the start of the turn
    **/
    public virtual void StartOfTurn(){

    }
    /**
    Called after the user uses a move
    **/
    public virtual void AfterUseMove(){

    }
    /**
    Affects the damage. Called when the user of the modifier deals damage.
    **/
    public virtual DamageDetails AffectDamageTarget(Crit user, Crit target,DamageDetails damageDets){
        return damageDets;
    }
    /**
    Affects the damage. Called when the source of the modifier is affected.
    **/
    public virtual DamageDetails AffectDamageUser(Crit user, Crit target,DamageDetails damageDets){
        return damageDets;
    }
    /**
    Any text this modifier should return to be output 
    **/
    public virtual string ModifierString(){
        return "";
    }
    /**
    Called when damage is dealt. Damage details is not return so this is for after the damage has happened.
    **/ 
    public virtual void OnDamageDealt(DamageDetails damageDets, Crit user, Crit target){

    }
    /**
    Triggers when a status move is used
    **/ 
    public virtual void StatusEffect(Crit user, Crit target){

    }
    /**
    Creates a modifier from the name (Allows moves to be assigned the modifier name in editor) and the name of the person who is using the modifier
    **/
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
/**
An enum of all of the different properties a modifier can react to
**/
public enum ModifierProperties{
    DamageTaken, EndOfTurn, OnDamageDealt , StartOfTurn, OnHit, AfterUseMove,UseStatusMove, AffectDamageTarget, AffectDamageUser
}
/**
The type of modifier
**/
public enum ModifierType{
    MoveDebuff, MoveBuff, Item, Ability
}
/**
Names of all of the modifier as an  enum. Should move to own class
**/
public enum ModifierName{
    None, Takedown, Protect
}