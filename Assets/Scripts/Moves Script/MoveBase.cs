using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ContactType { Special, Physical, Status}
[CreateAssetMenu(fileName = "MoveData", menuName = "Move/Create New Move")]

public class MoveBase : ScriptableObject
{
    [SerializeField] public string moveName;
    [SerializeField] public Element moveElement;
    [SerializeField] public ContactType contactType;
    [SerializeField] public int damage;
    [SerializeField] public int maxUsage;
    [SerializeField] public int accuracy;
    [SerializeField] public bool cantMiss;
    [SerializeField] public MoveEffects effects;
    [SerializeField] public MoveTarget effectTarget;
    [SerializeField] public ModifierName modName;
    [SerializeField] public int priority;
    [SerializeField] public TargetType target;
}

[System.Serializable]
public class LearnMove
{
    [SerializeField] public MoveBase moveBase;
    [SerializeField] public int levelLearnt;
}
[System.Serializable]
public class MoveEffects
{
    [SerializeField] List<StatBoost> boosts;
    public List<StatBoost> getBoosts(){
        return boosts;
    }
}

[System.Serializable]
public class StatBoost
{
    public Stat stat;
    public int boost;
}
public enum MoveTarget
{
    Foe,Self
}
public enum StatusEffect
{
    Burn, Poison
}
public enum TargetType{
    Enemy,Self
}