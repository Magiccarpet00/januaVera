

public abstract class Effect
{
    private Character user;
    private Character target;
    private int priority;
    private int deltaTurn;    

    public Effect(Character u, Character t, int prio, int delta)
    {
        user = u;
        target = t;
        priority = prio;
        deltaTurn = delta;
    }


    // Cette methode verifie si l'effet dois se lancer � ce tour
    // et renvoie true si il a �t� lanc�
    public bool Clock() //[CODE ACADEMIE] trouver un autre nom a la methode??????????????
    {
        deltaTurn--;
        if (deltaTurn == 0)
        {
            PerfomEffect();
            return true;
        }
        else
        {
            return false;
        }
    }

    public abstract void PerfomEffect();

    public Character GetTarget()
    {
        return target;
    }
}
