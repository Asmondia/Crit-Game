using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DialogBox : MonoBehaviour
{
    [SerializeField] Text textField;

    private IEnumerator coroutine;
    public float lettersPerSecond  = 10f;

    public void SetText(string text){
        if(coroutine != null){
            StopCoroutine(coroutine);
        }
        coroutine = SetTextAnimated(text);
        StartCoroutine(coroutine);
    }
    public void Clear(){
        if(coroutine != null){
            StopCoroutine(coroutine);
        }
        textField.text = "";
    }
    private IEnumerator SetTextAnimated(string text){
        textField.text = "";

        foreach(var letter in text.ToCharArray()){
            textField.text += letter;
            yield return new WaitForSeconds(1f/lettersPerSecond);
        }
    }
    
}
