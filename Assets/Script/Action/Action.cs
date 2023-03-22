using UnityEngine;

public abstract class Action
{
    private Character user;
    private int priority;

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

    public void SetPriority(int i)
    {
        priority = i;
    }

    public Character GetUser()
    {
        return user;
    }

}
