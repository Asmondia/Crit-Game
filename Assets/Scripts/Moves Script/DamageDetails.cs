using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDetails
{
    public bool didHit;
    public float effectiveness;
    public bool didCrit;
    public int damageDealt;
    public List<string> storedString;
    public bool blocked;
    
    public DamageDetails(bool hit, bool didCrit, float effectiveness,int damage){
        this.didHit = hit;
        this.effectiveness = effectiveness;
        this.didCrit = didCrit;
        this.damageDealt = damage;
        this.storedString = new List<string>();
        blocked = false;
    }
}
