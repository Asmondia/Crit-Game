using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleDialogBox : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] BattleDialogBoxElement dialogBox;
    [SerializeField] MoveSelect moveSelectBox;
    [SerializeField] MoveInfo moveInfoBox;

    [SerializeField] BattleDialogBoxElement smallDialogBox;
    [SerializeField] BattleOptions actions;
    
    [SerializeField] SwapScreen swapScreen;
    

    public void Start(){
        SetActiveDialog();

    }

    public void SetActiveDialog(){
        dialogBox.gameObject.SetActive(true);
        moveSelectBox.gameObject.SetActive(false);
        moveInfoBox.gameObject.SetActive(false);
        smallDialogBox.gameObject.SetActive(false);
        actions.gameObject.SetActive(false);
        swapScreen.gameObject.SetActive(false);
    }
    public void SetActiveMove(){
        dialogBox.gameObject.SetActive(false);
        moveSelectBox.gameObject.SetActive(true);
        moveInfoBox.gameObject.SetActive(true);
        smallDialogBox.gameObject.SetActive(false);
        actions.gameObject.SetActive(false);
        swapScreen.gameObject.SetActive(false);
    }
    public void SetActiveOptions(){
        smallDialogBox.gameObject.SetActive(true);
        actions.gameObject.SetActive(true);
        dialogBox.gameObject.SetActive(false);
        moveSelectBox.gameObject.SetActive(false);
        moveInfoBox.gameObject.SetActive(false);
        swapScreen.gameObject.SetActive(false);

    }
    public void SetActiveSwapScreen(){
        dialogBox.gameObject.SetActive(false);
        moveSelectBox.gameObject.SetActive(false);
        moveInfoBox.gameObject.SetActive(false);
        smallDialogBox.gameObject.SetActive(false);
        actions.gameObject.SetActive(false);
        swapScreen.gameObject.SetActive(true);

    }
    public IEnumerator setDialog(string text){
        SetActiveDialog();
        yield return dialogBox.SetTextAnimated(text);
    }

    public IEnumerator setActionDialog(string text){
        SetActiveOptions();
        yield return smallDialogBox.SetTextAnimated(text);
    }
    public Action resetPlayerAction(){
        return actions.resetAction();
    }
    public Action changeSelectedAction(Direction direction){
        return actions.moveSelected(direction);
    }

    public void SetMoves(Crit crit){
        Debug.Log(crit.nickname);
        moveSelectBox.SetUp(crit.moveList);
        setMoveInfo();
    }
    public Move getSelectedMove(){
        return moveSelectBox.getCurrentSelectedMove();

    }
    public void setMoveInfo(){
        Move currentMove = moveSelectBox.getCurrentSelectedMove();
        moveInfoBox.setType(currentMove.baseMove.moveElement.ToString());
        moveInfoBox.setUsage(currentMove.usage.ToString(), currentMove.baseMove.maxUsage.ToString() );
    }
    public Move changeSelectedMove(Direction direction){
        moveSelectBox.changeSelectedMove(direction);
        setMoveInfo();
        return moveSelectBox.getCurrentSelectedMove();
    }
    public void setCurrentMove(Move move){
        moveSelectBox.TrySetMove(move);
    }
    public void setSwapScreenTeam(CritTeam team){
        swapScreen.Setup(team);
    }
    public void changeSelectedCrit(Direction direction){
        swapScreen.ChangeSelected(direction);
    }
    public Crit getSwapCrit(){
        return swapScreen.getSelected();
    }
}
