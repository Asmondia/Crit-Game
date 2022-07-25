using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritTesting : MonoBehaviour
{
    [SerializeField] CritBase critBase;
    [SerializeField] int level = 1;
    public Crit crit;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        this.crit = new Crit(critBase, level);
        this.crit.generateMoves();
        spriteRenderer.sprite = crit.getFrontSprite();
        foreach (Move move in crit.moveList)
        {
            Debug.Log(move.baseMove.moveName);
        }
        for (int i = 0; i < 100; i++){
            Debug.Log("Xp Test:" + XpNeeded.XpToLevel(LvlType.Erratic,i+1));
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
