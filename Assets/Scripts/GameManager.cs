using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] PlayerController player;

    [SerializeField] BattleSystem battleSystem;
    [SerializeField] GameObject OutOfBattle;
    [SerializeField] DialogBox dialogBox;
    private GameState gameState;
    void Start()
    {
        if (instance == null){
            instance = this;
            instance.gameState = GameState.Default;
            
        } else {
            Destroy(gameObject);
        }
    }

    public static void startWildCritFight(Crit wildCrit){
        instance.battleSystem.gameObject.SetActive(true);
        instance.OutOfBattle.SetActive(false);
        instance.gameState = GameState.Battle;
        instance.battleSystem.SetupWildPokemonFight(instance.player.critTeam,wildCrit);

        //instance.StartCoroutine(instance.backToGame());
    }
    public static void startTrainerFight(CritTeam critTeam){
        instance.battleSystem.gameObject.SetActive(true);
        instance.OutOfBattle.SetActive(false);
        instance.gameState = GameState.Battle;
        instance.battleSystem.SetupTrainerCritFight(instance.player.critTeam,critTeam);

    }
    public static void SetDialogText(string text){
        instance.dialogBox.gameObject.SetActive(true);
        instance.dialogBox.SetText(text);
    }
    public static void CloseDialogText(){
        instance.dialogBox.Clear();
        instance.dialogBox.gameObject.SetActive(false);
    }
    public static void backToGame(){
        instance.battleSystem.gameObject.SetActive(false);
        instance.gameState = GameState.Default;
        instance.OutOfBattle.SetActive(true);
    }
    public static GameState getGameState(){
        return instance.gameState;
    }
    public static void SetGameState(GameState gameState){
        instance.gameState = gameState;
    }
}
public enum GameState{
    Default, Text, Battle
}
