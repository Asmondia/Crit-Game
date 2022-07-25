using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapElement : MonoBehaviour
{
    [SerializeField] Image critSprite;
    [SerializeField] Text critName;
    [SerializeField] Text critLvl;
    [SerializeField] Text critCurrentHealth;
    [SerializeField] Text critMaxHealth;
    [SerializeField] Image critHealthBar;
    [SerializeField] Image background;

    public bool canSelect = true;

    private Color baseColour;
    public Crit storedCrit;

    public void Setup(Crit crit){
        this.baseColour = background.color;
        this.storedCrit = crit;
        float currentHP = crit.HP;
        float maxHp = crit.getHP();
        critSprite.sprite = crit.getFrontSprite();
        critName.text = crit.nickname;
        critLvl.text = "Lvl " + crit.level.ToString();
        critCurrentHealth.text = currentHP.ToString();
        critMaxHealth.text = maxHp.ToString();
        float normalHp= currentHP/maxHp;
        critHealthBar.transform.localScale = new Vector3(normalHp,1f);
    }

    public void Selected(){
        background.color = new Color(0.4f,0.4f,0.4f,1);
        
    }
    public void Unselected(){
        background.color = baseColour;
    }

}
