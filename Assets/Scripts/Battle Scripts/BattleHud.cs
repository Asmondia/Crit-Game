using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleHud : MonoBehaviour
{
    [SerializeField] Text pokemonName;
    [SerializeField] Text levelText;
    [SerializeField] HPBar hpBar;
    [SerializeField] Text hpMax;
    [SerializeField] Text hpCurrent;
    [SerializeField] bool isPlayerHUD;
    
    public Crit crit;
    public int maxHealth;
    public int lastKnownHealth;
    private bool isChanging = false;

    public void SetData(Crit crit){
        pokemonName.text = crit.nickname;
        this.crit = crit;
        levelText.text = "Lvl." + crit.level.ToString();
        if(hpMax != null){
            hpMax.text = crit.getHP().ToString();
        }
        if(hpCurrent != null){
            hpCurrent.text = crit.HP.ToString();
        }
        hpBar.SetHP((float)crit.HP/crit.getHP());
        maxHealth = crit.getHP();
        lastKnownHealth = crit.HP;

    }
    public void updateLevel(){
        levelText.text = "Lvl." + crit.level.ToString();
        

    }
    public void Update(){
        //Debug.Log(crit.getHP());
        if(!isChanging){
            if(lastKnownHealth != crit.HP){
                isChanging = true;
                int oldHealth = lastKnownHealth;
                int newHealth = crit.HP;
                StartCoroutine(HealthChange(oldHealth,newHealth));
            }
        }
    }

    

    IEnumerator HealthChange(int oldHealth, int newHealth){
        if(hpMax != null){
            hpMax.text = crit.getHP().ToString();
            maxHealth = crit.getHP();
        }
        int difference = 1;
        bool isDecreasing = true;
        if (oldHealth > newHealth){
            isDecreasing = true;
            difference = oldHealth - newHealth;
        } else {
            isDecreasing = false;
            difference = newHealth - oldHealth;
        }
        float currentHealthPoint = oldHealth;
        //Debug.Log("Difference: " + difference);
        for (int step = 0; step < 5*difference; step++){
            if(isDecreasing){
                currentHealthPoint -= 1f/5f;
            } else {
                currentHealthPoint += 1/5f;
            }
            hpBar.SetHP((float)(currentHealthPoint)/(float)maxHealth);
            
            if(hpCurrent != null){
                hpCurrent.text = Mathf.FloorToInt(currentHealthPoint).ToString();
            }
            
            yield return new WaitForSeconds(1f/(5f*difference));

        }
        if(hpCurrent != null){
            hpCurrent.text = crit.HP.ToString();
        }
        
        lastKnownHealth = newHealth;
        isChanging = false;

    }


}
