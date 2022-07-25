using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public bool isMoving = false;
    public Direction currentDirection = Direction.Down;
    public LayerMask solidLayer;
    private Vector2 input;
    public LayerMask grassLayer;
    public LayerMask characterLayer;
    private SpriteRenderer spriteRenderer;
    public CritTeam critTeam;

    private bool notInDefault = false;
    private float interactCD = 0.5f;
    private float nextInteract;



    [SerializeField] float moveSpeed = 5;
    [SerializeField] CritBase baseCrit;
    [SerializeField] CritBase ghostTemp;
    [SerializeField] Animator animCtrl;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        animCtrl.SetBool("isMoving",isMoving);
        critTeam = new CritTeam();
        critTeam.addCrit(new Crit(baseCrit,50));
        critTeam.addCrit(new Crit(baseCrit,30));
        Crit shiny = new Crit(baseCrit,100);
        shiny.isShiny = true;
        critTeam.addCrit(shiny);
        critTeam.addCrit(new Crit(ghostTemp,5));
        nextInteract = Time.time;

    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.getGameState() == GameState.Default){
            if(notInDefault){
                notInDefault = false;
                nextInteract = Time.time + interactCD;
            }
            HandleMovement();
            if(!isMoving){
                HandleInteract();
            }
        } else {
            notInDefault = true;
            animCtrl.SetBool("isMoving",isMoving);

        }
        
        
        
    }
    private void HandleMovement(){
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
            if (input.y != 0){
                input.x = 0;
            }
            if (input != Vector2.zero)
            {
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;
                Direction oldDirection = currentDirection;
                 if(input.x != 0){
                    if(input.x > 0){
                        SetDirection(Direction.Right);
                    } else {
                        SetDirection(Direction.Left);
                    }
                } else {
                    if(input.y > 0){
                        SetDirection(Direction.Up);
                    } else {
                        SetDirection(Direction.Down);
                    }
                }
                if(oldDirection == currentDirection){
                    if(IsWalkable(targetPos)){
                        StartCoroutine(Move(targetPos));
                    } else {
                        animCtrl.SetBool("isMoving",isMoving);
                    }
                } else {
                    animCtrl.SetBool("isMoving",isMoving);
                }

            } else {
                animCtrl.SetBool("isMoving",isMoving);
            }
        }
    }
    private void HandleInteract(){
        if(Time.time > nextInteract){
            if(Input.GetKeyDown(KeyCode.E)){
                Interact();
            }
        }
    }
    private void Interact(){

        var targetPos = transform.position;
        switch(currentDirection){
            case(Direction.Up):
                    targetPos.y += 1;
                    break;
                case(Direction.Down):
                    targetPos.y -= 1;
                    break;
                case(Direction.Right):
                    targetPos.x += 1;
                    break;
                case(Direction.Left):
                    targetPos.x -= 1;
                    break;
                default:
                    break;
        }
        var collider = Physics2D.OverlapCircle(targetPos, 0.3f, characterLayer);
        if (collider != null){
            Debug.Log("Trying to interact");
            collider.GetComponent<Interactable>()?.Interact();
        } else {
            Debug.Log("Cant interact");
        }
        Debug.DrawLine(transform.position, targetPos, Color.green, 0.5f);

    }
    private void SetDirection(Direction direction){
        if(direction != currentDirection){
            if(currentDirection == Direction.Right){
                spriteRenderer.flipX = false;
            }
            if(direction == Direction.Right){
                spriteRenderer.flipX = true;
            }
            this.currentDirection = direction;
            switch(direction){
                case(Direction.Up):
                    animCtrl.SetTrigger("Up");
                    break;
                case(Direction.Down):
                    animCtrl.SetTrigger("Down");
                    break;
                case(Direction.Right):
                    animCtrl.SetTrigger("Right");
                    break;
                case(Direction.Left):
                    animCtrl.SetTrigger("Left");
                    break;
                default:
                    break;
            }
        }

    }
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        animCtrl.SetBool("isMoving",isMoving);
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon) {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
        
        
        CheckForEncounters();
        
    }
    private bool IsWalkable(Vector3 targetPos){
        bool isWalkable = true;
        if (Physics2D.OverlapCircle(targetPos, 0.3f, solidLayer | characterLayer) != null){
            isWalkable =  false;
        }
        // if (Physics2D.OverlapCircle(targetPos, 0.3f, characterLayer) != null){
        //     isWalkable =  false;
        // }
        return isWalkable;
    }
    private void CheckForEncounters()
    {
        Collider2D collider =Physics2D.OverlapCircle(transform.position, 0.1f, grassLayer);
        if (collider!= null){
            if (Random.Range(1,101) <= 10){
                Crit spawnedCrit = collider.GetComponent<SpawnWildCrit>().createWildCrit();
                Debug.Log("Wild Pokemon jumped out!");
                Debug.Log(spawnedCrit.nickname + ":" + spawnedCrit.level);
                GameManager.startWildCritFight(spawnedCrit);
            

                
            }
        }
    }
}
