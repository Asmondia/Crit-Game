
public class CritMoveObject 
{
    public Crit crit;
    public Move move;
    public Crit target;
    public bool isFoe;
    public CritMoveObject(Crit crit, Crit target, Move move, bool isFoe){
        this.crit = crit;
        this.target = target;
        this.move = move;
        this.isFoe = isFoe;
    }
}
