using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleDialogBoxElement : MonoBehaviour
{
    [SerializeField] Text textField;
    [SerializeField] float lettersPerSecond = 30f;

    public void SetText(string text){
        textField.text = text;
    }
    // public void SetTextAnimated(string text){
    //     StartCoroutine(TextAnimation(text));
    // }

    public IEnumerator SetTextAnimated(string text){
        textField.text = "";

        foreach(var letter in text.ToCharArray()){
            textField.text += letter;
            yield return new WaitForSeconds(1f/lettersPerSecond);
        }
    }
    
    public void changeColour(Color colour){
        textField.color = colour;
    }


}
