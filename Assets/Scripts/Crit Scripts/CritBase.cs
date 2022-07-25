using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CritData", menuName = "Crit/Create New Crit")]

public class CritBase : ScriptableObject
{
    [SerializeField] public Element type1;
    [SerializeField] public Element type2;
    [SerializeField] public int hpStat;
    [SerializeField] public int attackStat;
    [SerializeField] public int defenseStat;
    [SerializeField] public int spAttackStat;
    [SerializeField] public int spDefenseStat;
    [SerializeField] public int speedStat;
    [SerializeField] public string critName;

    [SerializeField] public Sprite frontSprite;
    [SerializeField] public Sprite backSprite;
    [SerializeField] public Sprite frontSpriteShiny;
    [SerializeField] public Sprite backSpriteShiny;

    [SerializeField] public CritBase evolution;
    [SerializeField] public int evolLevel = 101;
    [SerializeField] public LvlType lvlSpeed;
    [SerializeField] public int baseXp;


    [TextArea]
    [SerializeField] string description;

    [SerializeField] public List<LearnMove> learnableMoves;




}
public enum Stat{
    Attack,
    Defense,
    Speed,
    SpAttack,
    SpDefense
}