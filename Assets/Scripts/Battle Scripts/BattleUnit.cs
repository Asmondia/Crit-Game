using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class BattleUnit : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] CritBase critBase;
    [SerializeField] int level;
    [SerializeField] bool isPlayerCrit;

    [SerializeField] Image characterSprite;
    [SerializeField] Image critSprite;
    private Color intitialColour;
    private Vector3 initialPosCrit;
    private Vector3 initialPosChar;



    public Crit crit;
    [SerializeField] bool isPlayerSide;
    public void StartEnterAnimation(){
        if (isPlayerSide){
            characterSprite.transform.localPosition = new Vector3(-500f, initialPosChar.y);
            critSprite.transform.localPosition = new Vector3(-500f, initialPosCrit.y);       
        } else {
            characterSprite.transform.localPosition = new Vector3(500f, initialPosChar.y); 
            critSprite.transform.localPosition = new Vector3(500f, initialPosCrit.y);
        }
        characterSprite.transform.DOLocalMoveX(initialPosChar.x, 1f);
        critSprite.transform.DOLocalMoveX(initialPosCrit.x, 1f);
    }
    public void AttackAnimation(){
        var sequence = DOTween.Sequence();
        if (isPlayerSide){
            sequence.Append(critSprite.transform.DOLocalMoveX(initialPosCrit.x + 50f, 0.2f));
        } else {
            sequence.Append(critSprite.transform.DOLocalMoveX(initialPosCrit.x - 50f, 0.2f));
        }
        sequence.Append(critSprite.transform.DOLocalMoveX(initialPosCrit.x, 0.3f));
    }

    public void HitAnimation(){
        var sequence = DOTween.Sequence();
        var sequenceMove = DOTween.Sequence();
        if (isPlayerSide){
            sequenceMove.Append(critSprite.transform.DOLocalMoveX(initialPosCrit.x - 30f, 0.1f));
        } else {
            sequenceMove.Append(critSprite.transform.DOLocalMoveX(initialPosCrit.x + 30f, 0.1f));
        }
        Color damagedColour = new Color(0.1f,0.1f,0.1f,1);
        sequence.Append(critSprite.DOColor(damagedColour, 0.1f));
        sequence.Append(critSprite.DOColor(intitialColour, 0.1f));
        sequence.Append(critSprite.DOColor(damagedColour, 0.15f));
        sequence.Append(critSprite.DOColor(intitialColour, 0.15f));
        sequenceMove.Append(critSprite.transform.DOLocalMoveX(initialPosCrit.x, 1f));

    }
    public void FaintAnimation(){
        var sequence = DOTween.Sequence();
        sequence.Append(critSprite.transform.DOLocalMoveY(initialPosCrit.y - 75f, 1f));
        sequence.Join(critSprite.DOFade(0f,1f));
    }
    public void SetUp(Crit crit){
        if(initialPosChar == new Vector3(0,0,0)){
            initialPosChar = characterSprite.transform.localPosition;
        }
        if(initialPosCrit == new Vector3(0,0,0)){
            initialPosCrit = critSprite.transform.localPosition;
        }
        if(intitialColour == new Color(0f,0f,0f,0f)){
            intitialColour = critSprite.color;
        }
        

        Debug.Log(intitialColour);
        
        
        this.crit = crit;
        if (isPlayerCrit){
            critSprite.sprite = crit.getBackSprite();

        } else {
            critSprite.sprite = crit.getFrontSprite();
        }
        critSprite.color = intitialColour;
    }
    public void setWild(){
        characterSprite.gameObject.SetActive(false);
    }
    public void setTrainer(){
        characterSprite.gameObject.SetActive(true);
    }
    public void setCharacterSprite(Sprite sprite){
        characterSprite.sprite = sprite;
    }
}
