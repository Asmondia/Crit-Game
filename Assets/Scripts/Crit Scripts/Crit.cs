using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Crit
{
    public CritBase baseData;
    public int level;
    public string nickname;
    public int HP;
    public List<Move> moveList;
    public Dictionary<Stat, int> Stats;
    public Dictionary<Stat, int> StatBoost;
    public Dictionary<StatusEffect, bool> Conditions;
    public List<Modifier> modifiers;
    public bool isShiny = false;
    public int totalXp = 0;
    private int maxHp;
    public bool hasActed = false;


    public static DamageDetails HitWithMove(Crit attackingCrit, Move move, Crit defendingCrit){
        bool didCrit = false;
        int chanceToCritical = 24;
        float criticalMult = 1;
        if (Random.Range(1,chanceToCritical + 1) == 1){
            didCrit = true;
            criticalMult = 2;
        }
        int randomDamage = Mathf.FloorToInt(Crit.CalcDamage( attackingCrit,  move,  defendingCrit) * (Random.Range(85,101)/100f)* criticalMult);
        bool didHit;
        randomDamage= Mathf.Min(randomDamage, defendingCrit.HP);
        if (Random.Range(1,101) <= move.baseMove.accuracy || move.baseMove.cantMiss ){
           didHit = true;
        } else {
            didHit = false;
        }
        DamageDetails damageDets = new DamageDetails( didHit, didCrit, defendingCrit.getWeakness(move.baseMove.moveElement), randomDamage);
        List<Modifier> moveMods = new List<Modifier>();
        foreach(Modifier mod in attackingCrit.getModifiersByProperty(ModifierProperties.AffectDamageUser)){
     
            damageDets =  mod.AffectDamageUser(attackingCrit, defendingCrit, damageDets);

        }
        foreach(Modifier mod in defendingCrit.getModifiersByProperty(ModifierProperties.AffectDamageTarget)){
     
            damageDets =  mod.AffectDamageTarget(attackingCrit, defendingCrit, damageDets);
        }
      
        if (didHit){ 
            defendingCrit.takeDamage(damageDets.damageDealt);
        }
        Modifier moveMod = Modifier.GetModifierObject(move.baseMove.modName, attackingCrit);
        if(damageDets.didHit && 
        moveMod != null && 
        moveMod.GetProperties().Contains(ModifierProperties.OnDamageDealt)){
            moveMod.OnDamageDealt(damageDets, attackingCrit, defendingCrit);
        }


        if(damageDets.didHit && moveMod != null){
            List<Modifier> modList = attackingCrit.getModifiersByProperty(ModifierProperties.OnDamageDealt);
            foreach(Modifier mod in modList){
                mod.OnDamageDealt(damageDets, attackingCrit, defendingCrit);
            }
        }
        
        move.usage -= 1;
        Debug.Log("Dealt " + randomDamage + " damage!");
        return damageDets;

    }

    public static float CalcDamage(Crit attackingCrit, Move move, Crit defendingCrit){
        float damageFirstPart = 0;
        if (move.baseMove.contactType == ContactType.Physical){
            damageFirstPart = ((((2f*attackingCrit.level)/5f) + 2) * move.baseMove.damage * attackingCrit.getAttack()/defendingCrit.getDefense());
            Debug.Log("Attack: " + attackingCrit.getAttack());
            Debug.Log("Defense: " + defendingCrit.getDefense());
        } else if (move.baseMove.contactType == ContactType.Special){
            damageFirstPart = ((((2f*attackingCrit.level)/5f) + 2) * move.baseMove.damage * attackingCrit.getSpAttack()/defendingCrit.getSpDefense());
            Debug.Log("SpAttack: " + attackingCrit.getSpAttack());
            Debug.Log("SpDefense: " + defendingCrit.getSpDefense());
        }
        float stabMultiplier = 1f;
        if (attackingCrit.hasElement(move.baseMove.moveElement)){
            stabMultiplier = 1.5f;
        }
        float weaknessMultiplier = defendingCrit.getWeakness(move.baseMove.moveElement);
        float damage = ((damageFirstPart/50f)+2)*weaknessMultiplier*stabMultiplier;
        Debug.Log("Damage first part damage: " + damageFirstPart);
        Debug.Log("Calc damage: " + damage);
        return damage;
    }
    public void AddModifier(Modifier mod){
        modifiers.Add(mod);
    }
    public void RemoveModifier(Modifier mod){
        modifiers.Remove(mod);
    }
    public Crit(CritBase baseData, int level)
    {
        this.baseData = baseData;
        this.level = level;
        
        this.moveList = new List<Move>();
        this.nickname = baseData.critName;
        generateMoves();
        totalXp = XpNeeded.XpToLevel(baseData.lvlSpeed, level);

        modifiers = new List<Modifier>();
    
        StatBoost = new Dictionary<Stat, int>();
        Stats = new Dictionary<Stat, int>();
        Conditions = new Dictionary<StatusEffect, bool>();

        ResetStatBoost();
        CalculateStats();
        ResetStatusConditions();

        this.HP = getHP();
    
    }
    public void ResetStatusConditions(){
        Conditions.Add(StatusEffect.Burn,false);
        Conditions.Add(StatusEffect.Poison,false);
    }
    public void ResetStatBoost(){
        StatBoost.Add(Stat.Attack, 0);
        StatBoost.Add(Stat.Defense, 0);
        StatBoost.Add(Stat.SpAttack, 0);
        StatBoost.Add(Stat.SpDefense, 0);
        StatBoost.Add(Stat.Speed, 0);
    }
    void CalculateStats()
    {
        Stats.Add(Stat.Attack, Mathf.FloorToInt((baseData.attackStat * 2 * level) / 100f) + 5);
        Stats.Add(Stat.Defense, Mathf.FloorToInt((baseData.defenseStat * 2 * level) / 100f) + 5);
        Stats.Add(Stat.SpAttack, Mathf.FloorToInt((baseData.spAttackStat * 2 * level) / 100f) + 5);
        Stats.Add(Stat.SpDefense, Mathf.FloorToInt((baseData.spDefenseStat * 2 * level) / 100f) + 5);
        Stats.Add(Stat.Speed, Mathf.FloorToInt((baseData.speedStat * 2 * level) / 100f) + 5);
        maxHp = Mathf.FloorToInt((2* baseData.hpStat * level) / 100f) + level + 10;
    }
    public void takeDamage(int damage){
        this.HP -= damage;
        if (this.HP < 0){
            this.HP = 0;
        }
    }
    public bool hasElement(Element element){
        return (this.baseData.type1 == element || this.baseData.type2 == element);
    }
    
    public float getWeakness(Element element){
        return Type.Weakness(element,this.baseData.type1) * Type.Weakness(element,this.baseData.type2); 
    }
    public List<Modifier> getModifiersByProperty(ModifierProperties modProp){
        List<Modifier> modList = new List<Modifier>();
        Debug.Log(modifiers.Count);
        foreach (Modifier mod in modifiers){
            if (mod.GetProperties().Contains(modProp)){
                modList.Add(mod);
            }
        }
        return modList;
    }

    public void generateMoves()
    {
        if (baseData.learnableMoves.Count < 5)
        {
            foreach(LearnMove move in baseData.learnableMoves)
            {
                moveList.Add(new Move(move.moveBase));
            }
        } else
        {
            int lowestLevel = 101;
            List<LearnMove> tempMoves = new List<LearnMove>();
            foreach(LearnMove move in baseData.learnableMoves)
            {
                if (tempMoves.Count < 4)
                {
                    if (move.levelLearnt <= this.level)
                    {
                        tempMoves.Add(move);
                        if (move.levelLearnt < lowestLevel)
                        {
                            lowestLevel = move.levelLearnt;
                        }
                    }
                }
                else
                {
                    if (move.levelLearnt <= this.level)
                    {
                        tempMoves = removeLowestLevelMove(tempMoves, move);
                    }
                }
            }
            foreach (LearnMove move in tempMoves)
            {
                moveList.Add(new Move(move.moveBase));
            }
        }
    }
    private List<LearnMove> removeLowestLevelMove(List<LearnMove> learnMoves, LearnMove newMove)
    {
        learnMoves.Add(newMove);
        LearnMove tempMove = newMove;
        int lowestValue = 101;
        foreach (LearnMove move in learnMoves)
        {
            if(move.levelLearnt < lowestValue)
            {
                lowestValue = move.levelLearnt;
                tempMove = move;
            }
        }
        learnMoves.Remove(tempMove);
        return learnMoves;

    }

    public void GiveXp(int newXp){
        
        if (this.level < 100){
            this.totalXp += newXp;
            bool canLevelUp = true;
            while (canLevelUp){
                if(XpNeeded.XpToLevel(this.baseData.lvlSpeed,level+1) < this.totalXp){
                    LevelUp();
                } else {
                    canLevelUp = false;
                }
                if (this.level == 100){
                    canLevelUp = false;
                }
            }
        }
        

    }
    void LevelUp(){
        
        int missingHp = getHP() - this.HP;
        this.level += 1;
        CalculateStats();
        this.HP = getHP() - missingHp;
        
    }
    int GetStat(Stat stat){
        int statBoostValue = StatBoost[stat];
        float statChange =Mathf.Max(2, 2 + statBoostValue)/Mathf.Max(2, 2 - statBoostValue);
        int effectiveStat = Mathf.FloorToInt(statChange * Stats[stat]);
        return effectiveStat;
    }
    public bool GetConditionBool(StatusEffect status){
        bool temp = false;
        if (Conditions[status] == true){
            temp = true;
        }
        return temp;
    }
    public void addCondition(StatusEffect effect){
        Conditions[effect] = true;
    }
    public void removeCondition(StatusEffect effect){
        Conditions[effect] = false;
    }
    public int getHP()
    {
        return maxHp;
    }
    public int getAttack()
    {
        return GetStat(Stat.Attack);
    }
    public int getDefense()
    {
        return GetStat(Stat.Defense);
    }
    public int getSpAttack()
    {
        return GetStat(Stat.SpAttack);
    }
    public int getSpDefense()
    {
        return GetStat(Stat.SpDefense);
    }
    public int getSpeed()
    {
        return GetStat(Stat.Speed);
    }
    public void AffectBoost(Stat stat, int value){
        int currentValue = StatBoost[stat];
        int newValue = currentValue + value;
        StatBoost[stat] = newValue;
    }

    public Sprite getFrontSprite()
    {
        if (isShiny)
        {
            return baseData.frontSpriteShiny;
        }
        else
        {
            return baseData.frontSprite;
        }
    }

    public Sprite getBackSprite()
    {
        if (isShiny)
        {
            return baseData.backSpriteShiny;
        }
        else
        {
            return baseData.backSprite;
        }
    }
    public static string StatString(Stat stat){
        switch(stat){
            case(Stat.Attack):
                return "Attack";
            case(Stat.Defense):
                return "Defense";
            case(Stat.SpDefense):
                return "Special Defense";
            case(Stat.SpAttack):
                return "Special Attack";
            case(Stat.Speed):
                return "Speed";  
            default:
                return "Error finding stat";      

        }

    }
}


