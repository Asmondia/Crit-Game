using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] GameObject health;
    private int lastKnownHealth;
    private bool changing = false;
    
    public void SetHP(float hpNormal){
        health.transform.localScale = new Vector3(hpNormal,1f);
        lastKnownHealth = Mathf.FloorToInt(hpNormal);
        changing = false;
    }
}
