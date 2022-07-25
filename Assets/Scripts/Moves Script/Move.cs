using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public MoveBase baseMove;
    public int usage;
    
    public Move(MoveBase baseMove)
    {
        this.baseMove = baseMove;
        this.usage = baseMove.maxUsage;
    }
}
