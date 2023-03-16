using UnityEngine;

public abstract class Action
{
    private int priority;
    private Character user;

    public Action(int p, Character u)
    {
        priority = p;
        user = u;
    }

    public abstract void PerfomAction();

    public int GetPriority()
    {
        return priority;
    }

    public Character GetUser()
    {
        return user;
    }

}
