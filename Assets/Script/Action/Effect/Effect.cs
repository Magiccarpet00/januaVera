

public abstract class Effect
{
    protected Character user;
    protected Character target;
    protected int priority;
    protected int deltaTurn;
    public bool toDelete = false;

    public Effect(Character u, Character t, int prio, int delta)
    {
        user = u;
        target = t;
        priority = prio;
        deltaTurn = delta;
    }

    // Cette methode verifie si l'effet dois se lancer à ce tour
    // et renvoie true si il a été lancé
    public void Clock() //[CODE ACADEMIE] trouver un autre nom a la methode??????????????
    {
        deltaTurn--;
        if (deltaTurn == 0)
        {
            PerfomEffect();
            toDelete = true;
        }
    }

    public abstract void PerfomEffect();

    public Character GetTarget()
    {
        return target;
    }
}
