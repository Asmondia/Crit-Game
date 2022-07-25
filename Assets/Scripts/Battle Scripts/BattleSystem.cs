using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy, PlayerSwap}
public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleHud playerHUD;

    [SerializeField] BattleUnit enemyUnit;
    [SerializeField] BattleHud enemyHUD;
    [SerializeField] BattleDialogBox dialogSystem;
    

    Action currentAction;
    Move currentMove;
    BattleState state;
    public CritTeam playerTeam;

    public CritTeam enemyTeam;

    public Crit currentPlayerCrit;
    public Crit currentEnemyCrit;

    public Move chosenMove;


   

    IEnumerator SetupBattle()
    {
        currentPlayerCrit = nextPlayerCrit();
        

        currentEnemyCrit = nextEnemyCrit();
        enemyUnit.SetUp(currentEnemyCrit);
        enemyUnit.StartEnterAnimation();
        enemyHUD.SetData(currentEnemyCrit);
        playerUnit.SetUp(currentPlayerCrit);
        playerUnit.StartEnterAnimation();
        playerHUD.SetData(currentPlayerCrit);

        // playerUnit.gameObject.SetActive(false);
        // playerHUD.gameObject.SetActive(false);
        yield return dialogSystem.setDialog($"A Wild {currentEnemyCrit.nickname} Appeared!");
        yield return new WaitForSeconds(1f);
         yield return dialogSystem.setDialog($"Go " + currentPlayerCrit.nickname + "!");
        yield return new WaitForSeconds(1f);
        
        // playerUnit.gameObject.SetActive(true);
        // playerHUD.gameObject.SetActive(true);

        PlayerAction();

    }

    void PlayerAction(){
        state = BattleState.PlayerAction;
        currentAction = dialogSystem.resetPlayerAction();
        StartCoroutine(dialogSystem.setActionDialog("Choose an Action"));
    }



    public void SetupWildPokemonFight(CritTeam playerTeam, Crit wildCrit){
        this.playerTeam = playerTeam;
        this.enemyTeam = new CritTeam();
        enemyTeam.addCrit(wildCrit);
        enemyUnit.setWild();
        StartCoroutine(SetupBattle());

    }
    public void SetupTrainerCritFight(CritTeam playerTeam, CritTeam enemyTeam){
        this.playerTeam = playerTeam;
        this.enemyTeam = enemyTeam;
        enemyUnit.setTrainer();
        StartCoroutine(SetupBattle());

    }
    public Crit nextPlayerCrit(){
        return playerTeam.getNextCrit();
    }
    public Crit nextEnemyCrit(){
        return enemyTeam.getNextCrit();
    }
    private void Update(){
        if (state == BattleState.PlayerAction){
            HandleActionSelection();
        } else if(state == BattleState.PlayerMove){
            HandleMoveSelection();
        } else if(state == BattleState.PlayerSwap){
            HandleCritSelection();
        }
    }
    private void PlayerMove(){
        state = BattleState.PlayerMove;
        dialogSystem.SetActiveMove();
        dialogSystem.SetMoves(currentPlayerCrit);
        dialogSystem.setCurrentMove(currentMove);
        currentMove = dialogSystem.getSelectedMove();
    }
    IEnumerator NotEnoughUsage(){
        state = BattleState.Busy;
        yield return dialogSystem.setDialog("You cant use that move anymore!");
        yield return new WaitForSeconds(0.5f);
        dialogSystem.SetActiveMove();
        state = BattleState.PlayerMove;
    }


    IEnumerator CritUseMove(Crit crit, Crit target, Move move, bool isFoe){
        yield return dialogSystem.setDialog(crit.nickname + " used " + move.baseMove.moveName + "!");
        yield return new WaitForSeconds(1f);

        DamageDetails damageDets;
        if(move.baseMove.contactType != ContactType.Status){
            if (isFoe){
                    enemyUnit.AttackAnimation();
                } else {
                    playerUnit.AttackAnimation(); 
                }
            yield return new WaitForSeconds(0.2f);
            damageDets = Crit.HitWithMove(crit,move, target);
            
            if (damageDets.didHit && damageDets.effectiveness != 0){
                if (isFoe){
                    playerUnit.HitAnimation();
                } else {
                enemyUnit.HitAnimation(); 
                }
                
            }
            yield return new WaitForSeconds(1f);
            if(damageDets.didHit){
                if(move.baseMove.damage > 0){
                    if (damageDets.didCrit && damageDets.effectiveness != 0 ){
                        yield return  dialogSystem.setDialog("A critical hit!");
                        yield return new WaitForSeconds(1f);
                    }
                    if (0 < damageDets.effectiveness && damageDets.effectiveness < 1){
                        yield return dialogSystem.setDialog("Its not very effective");
                    } else if (damageDets.effectiveness> 1){
                        yield return dialogSystem.setDialog("Its super effective!");
                    } else if (damageDets.effectiveness == 0){
                        yield return dialogSystem.setDialog("It doesnt effect " + target.nickname + "...");
                    }
                    yield return new WaitForSeconds(1f);
                }
            } else if (!damageDets.blocked){
                yield return  dialogSystem.setDialog(crit.nickname + " missed its attack!");
                yield return new WaitForSeconds(1f);
            }
            
        } else {
            bool didHit;
            if (Random.Range(1,101) <= move.baseMove.accuracy || move.baseMove.cantMiss ){
                Debug.Log("Status move hit");
                didHit = true;
            } else {
                Debug.Log("Status move missed");
                didHit = false;
            }

            Modifier moveMod = Modifier.GetModifierObject(move.baseMove.modName, crit);
            if(didHit && 
            moveMod != null && 
            moveMod.GetProperties().Contains(ModifierProperties.UseStatusMove)){
                Debug.Log("Status effect method called");
                moveMod.StatusEffect(crit, target);
                string modString = moveMod.ModifierString();
                if (modString.Length > 0){
                    yield return dialogSystem.setDialog(modString);
                    yield return new WaitForSeconds(1f);
                }
            }
            damageDets = new DamageDetails(didHit, false, 1f, 0);

        }
        foreach(string text in damageDets.storedString){
            yield return  dialogSystem.setDialog(text);
            yield return new WaitForSeconds(1f);
        }
        
        foreach (StatBoost statBoost in move.baseMove.effects.getBoosts()){
            yield return ApplyBoost(statBoost,isFoe,move);
        }
        if(damageDets.didHit){
            List<Modifier> modList = crit.getModifiersByProperty(ModifierProperties.AfterUseMove);
            foreach(Modifier mod in modList){
                mod.AfterUseMove();
                string modString = mod.ModifierString();
                if (modString.Length > 0){
                    yield return dialogSystem.setDialog(modString);
                    yield return new WaitForSeconds(1f);
                }
                
            }
        }
        crit.hasActed = true;
        yield return new WaitForSeconds(1f);
    }
    public Move EnemyMove(){
        float bestDamage = 0;
        Move bestMove = null;
        foreach (Move move in currentEnemyCrit.moveList){
            Debug.Log("Move " + move.baseMove.moveName + " could deal " + Crit.CalcDamage(currentEnemyCrit,move,currentPlayerCrit));
            if (bestMove == null){
                bestMove = move;
                bestDamage = Crit.CalcDamage(currentEnemyCrit,move,currentPlayerCrit);
            } else if (Crit.CalcDamage(currentEnemyCrit,move,currentPlayerCrit) > bestDamage){
                bestMove = move;
                bestDamage = Crit.CalcDamage(currentEnemyCrit,move,currentPlayerCrit);
            }
        }
        Debug.Log("Chose to use " + bestMove.baseMove.moveName);
        return bestMove;
    }
    IEnumerator ApplyBoost(StatBoost statBoost, bool isEnemy, Move move){
        Crit target;
        if (isEnemy == true){
            if (move.baseMove.effectTarget == MoveTarget.Self){
                target = currentEnemyCrit;
            } else {
                target = currentPlayerCrit;
            }
        } else {
            if (move.baseMove.effectTarget == MoveTarget.Self){
                target = currentPlayerCrit;
            } else {
                target = currentEnemyCrit;
            }
        }
        target.AffectBoost(statBoost.stat, statBoost.boost);
        yield return dialogSystem.setDialog(target.nickname + "'s " + Crit.StatString(statBoost.stat) + " rose by " + statBoost.boost);

        yield return new WaitForSeconds(1f);
    }


    IEnumerator handleEnemyFaint(){
        yield return dialogSystem.setDialog("The opposing " + currentEnemyCrit.nickname +  " fainted!");
        enemyUnit.FaintAnimation();
        yield return new WaitForSeconds(2f);

        GameManager.backToGame();
    }
    IEnumerator handlePlayerFaint(){
        yield return dialogSystem.setDialog(currentPlayerCrit.nickname +  " fainted!");
        playerUnit.FaintAnimation();
        yield return new WaitForSeconds(2f);
        GameManager.backToGame();
    }

    private void giveXp(){
        currentPlayerCrit.GiveXp(105000);
        playerHUD.updateLevel();

    }
    IEnumerator TurnManager(){
        List<CritMoveObject> critList = new List<CritMoveObject>();
        critList.Add(new CritMoveObject(currentPlayerCrit, currentEnemyCrit, currentMove, false));
        critList.Add(new CritMoveObject(currentEnemyCrit, currentPlayerCrit, chosenMove, true));
        bool allActed = true;
        foreach( CritMoveObject critObj in critList){
            if(critObj.crit.hasActed == false){
                allActed = false;
            }
        }
        while (!allActed){
            Debug.Log(allActed);
            yield return NextMove(critList);
            allActed = true;
            foreach( CritMoveObject critObj in critList){
                if(critObj.crit.hasActed == false && critObj.crit.HP > 0){
                    allActed = false;
                }
            }
        }
        foreach( CritMoveObject critObj in critList){
            critObj.crit.hasActed = false;
        }
        yield return endOfTurn();

    }
    IEnumerator NextMove(List<CritMoveObject> critList){
        bool nextCritNotFound = true;
        int currentPrio = 6;
        List<CritMoveObject> inPrio = new List<CritMoveObject>();
        while (nextCritNotFound){
            Debug.Log("Checking priority " + currentPrio);
            foreach( CritMoveObject critObj in critList){
                if(critObj.crit.hasActed == false && critObj.move.baseMove.priority == currentPrio && critObj.crit.HP > 0){
                   inPrio.Add(critObj);
                }
            }
            if(inPrio.Count > 0 ){
                nextCritNotFound = false;
            } 
            else {
                currentPrio -= 1;
            }
        }
        // Random rng = new Random();
        // currentPrio = currentPrio.OrderBy(a => rng.Next()).ToList();
        CritMoveObject critObjChosen = inPrio.First();
        int highestSpeed = critObjChosen.crit.getSpeed();;
        foreach( CritMoveObject critObj in inPrio){
            if (critObj.crit.getSpeed() > highestSpeed){
                critObjChosen = critObj;
                highestSpeed = critObj.crit.getSpeed();
            } else if (critObj.crit.getSpeed() == highestSpeed){
                if (Random.Range(1,3) == 1){
                    critObjChosen = critObj;
                    highestSpeed = critObj.crit.getSpeed();
                } 
            } 
        }
        yield return CritUseMove(critObjChosen.crit,  critObjChosen.target, critObjChosen.move,critObjChosen.isFoe);



    }
    // IEnumerator playerChosenMove(){
    //     bool playerGoFirst = true;
    //     Move chosenMove = EnemyMove();
    //     if (currentPlayerCrit.getSpeed() > currentEnemyCrit.getSpeed()){
    //         playerGoFirst = true;
    //     } else if (currentPlayerCrit.getSpeed() > currentEnemyCrit.getSpeed()){
    //         playerGoFirst = false;
    //     } else {
    //         if (Random.Range(1,3) == 1){
    //             playerGoFirst = true;
    //         } else {
    //             playerGoFirst = false;
    //         }
    //     }

    //     if (playerGoFirst){
    //         yield return CritUseMove(currentPlayerCrit,  currentEnemyCrit, currentMove,false);
    //     } else {
    //         yield return CritUseMove(currentEnemyCrit, currentPlayerCrit, chosenMove,true);
    //     } 

    //     if(currentPlayerCrit.HP > 0 && currentEnemyCrit.HP > 0){
    //         if (playerGoFirst){
    //             yield return CritUseMove(currentEnemyCrit, currentPlayerCrit, chosenMove,true);
    //         } else {
    //             yield return CritUseMove(currentPlayerCrit,  currentEnemyCrit, currentMove,false);
    //         } 
            
    //     } 
    //     Debug.Log("Checking once again");
        
    //     yield return endOfTurn();
    // }

    IEnumerator checkFaint(){
        if(currentPlayerCrit.HP > 0 && currentEnemyCrit.HP > 0){
            PlayerAction();
        } else if (currentPlayerCrit.HP > 0 && currentEnemyCrit.HP == 0){
            yield return handleEnemyFaint();
        } else if (currentPlayerCrit.HP == 0 && currentEnemyCrit.HP > 0){
            yield return handlePlayerFaint();
        } else if (currentPlayerCrit.HP == 0 && currentEnemyCrit.HP == 0){
            yield return handlePlayerFaint();
        }
    }
    IEnumerator endOfTurn(){

        currentEnemyCrit.addCondition(StatusEffect.Burn);
        currentEnemyCrit.addCondition(StatusEffect.Poison);

        yield return checkConditions(currentPlayerCrit);
        yield return checkConditions(currentEnemyCrit);
        yield return checkFaint();

    }
    IEnumerator checkConditions(Crit crit){
        Debug.Log("Checking Conditions");
        if (crit.GetConditionBool(StatusEffect.Burn) && crit.HP > 0){
            int burnDamage = Mathf.FloorToInt(crit.getHP()/8);
            crit.takeDamage(burnDamage);
            yield return dialogSystem.setDialog(crit.nickname + " was hurt by its burn");
            yield return new WaitForSeconds(1f);

        }
        if (crit.GetConditionBool(StatusEffect.Poison) && crit.HP > 0){
            int poisonDamage = Mathf.FloorToInt(crit.getHP()/8);
            crit.takeDamage(poisonDamage);
            yield return dialogSystem.setDialog(crit.nickname + " was hurt by posion");
            yield return new WaitForSeconds(1f);

        }
        foreach(Modifier mod in crit.getModifiersByProperty(ModifierProperties.EndOfTurn)){
            Debug.Log("Type of : " + mod.GetType());
            mod.EndOfTurn();
        }



    }
    public void trySwapCrit(Crit crit){
        if(crit != currentPlayerCrit){
            Debug.Log("Swapped to " + crit.nickname);
            StartCoroutine(playerSwappedCrit(crit));
        } else {
            Debug.Log("This Crit is already selected!");
        }
    }
    IEnumerator playerSwappedCrit(Crit newPlayerCrit){
        chosenMove = EnemyMove();
        currentPlayerCrit = newPlayerCrit;
        playerUnit.SetUp(currentPlayerCrit);
        playerHUD.SetData(currentPlayerCrit);
        currentPlayerCrit.hasActed = true;
        
        yield return TurnManager();

    }
    public void selectedAction(){
        if (currentAction == Action.Swap){
            state = BattleState.PlayerSwap;
            dialogSystem.SetActiveSwapScreen();
            dialogSystem.setSwapScreenTeam(playerTeam);
        } else if (currentAction == Action.Fight){
            PlayerMove();
        }

    }
    public void TrySelectMove(){
        if (currentMove.usage > 0){
            state = BattleState.Busy;
            chosenMove = EnemyMove();
            StartCoroutine(TurnManager());
        } else {
            StartCoroutine(NotEnoughUsage());
        }
    }
    void HandleActionSelection(){
        if(Input.GetKeyDown(KeyCode.S)){
            currentAction = dialogSystem.changeSelectedAction(Direction.Down);
        } else if(Input.GetKeyDown(KeyCode.W)){
            currentAction = dialogSystem.changeSelectedAction(Direction.Up);
        } else if(Input.GetKeyDown(KeyCode.A)){
            currentAction = dialogSystem.changeSelectedAction(Direction.Left);
        } else if(Input.GetKeyDown(KeyCode.D)){
            currentAction = dialogSystem.changeSelectedAction(Direction.Right);
        }  else if(Input.GetKeyDown(KeyCode.E)){
            Debug.Log("Selected" + currentAction.ToString());
            selectedAction();
        }
    
    }
    void HandleMoveSelection(){
        if(Input.GetKeyDown(KeyCode.S)){
            currentMove = dialogSystem.changeSelectedMove(Direction.Down);
        } else if(Input.GetKeyDown(KeyCode.W)){
            currentMove = dialogSystem.changeSelectedMove(Direction.Up);
        } else if(Input.GetKeyDown(KeyCode.A)){
            currentMove = dialogSystem.changeSelectedMove(Direction.Left);
        } else if(Input.GetKeyDown(KeyCode.D)){
            currentMove = dialogSystem.changeSelectedMove(Direction.Right);
        } else if(Input.GetKeyDown(KeyCode.E)){
            TrySelectMove();            
        } else if(Input.GetKeyDown(KeyCode.Q)){
            PlayerAction();
        } else if(Input.GetKeyDown(KeyCode.Z)){
  
            giveXp();
        } 
  
    }
    void HandleCritSelection(){
        if(Input.GetKeyDown(KeyCode.S)){
            dialogSystem.changeSelectedCrit(Direction.Down);
        } else if(Input.GetKeyDown(KeyCode.W)){
            dialogSystem.changeSelectedCrit(Direction.Up);
        } else if(Input.GetKeyDown(KeyCode.A)){
            dialogSystem.changeSelectedCrit(Direction.Left);
        } else if(Input.GetKeyDown(KeyCode.D)){
            dialogSystem.changeSelectedCrit(Direction.Right);
        } else if (Input.GetKeyDown(KeyCode.E)){
            trySwapCrit(dialogSystem.getSwapCrit());
        } else if (Input.GetKeyDown(KeyCode.Q)){
            PlayerAction();
        }


    }
}
