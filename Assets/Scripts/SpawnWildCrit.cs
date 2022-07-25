using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWildCrit: MonoBehaviour
{
    [SerializeField] SpawnData[] spawnableCrit;


    public Crit createWildCrit(){
        int totalWeight = 0;
        foreach(SpawnData spawn in spawnableCrit){
            totalWeight += spawn.spawnWeight;
        }
        int randomValue = Random.Range(1,totalWeight);
        int currentPos = 0;
        CritBase selectedBase = null;
        int level = 0;
        while(randomValue > 0 ){
            
            randomValue -= spawnableCrit[currentPos].spawnWeight;
            if (randomValue <= 0){
                selectedBase = spawnableCrit[currentPos].critBase;
                level = Random.Range(spawnableCrit[currentPos].minLevel,spawnableCrit[currentPos].maxLevel+1);
            }
            currentPos += 1;

        }
        if(selectedBase != null && level != 0){
            return new Crit(selectedBase,level);
        } else{
            return null;
        }

    }

}
