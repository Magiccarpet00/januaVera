using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    public CharacterData characterData;

    // REAL TIME MOVE
    private Vector3 target;
    private float smoothTime;
    private Vector3 velocity = Vector3.zero;

    public Animator animator;


    [SerializeField] private GameObject currentSpot;
    [SerializeField] private bool isHide;
    private bool onPath; // quand on est on path on est sur le chemin vers le currentSpot mais on est pas encore sur ce spot � ce tour


    private Stack<Action> stackAction = new Stack<Action>();


    void Update()
    {
        if (isHide)
            smoothTime = 0.8f;
        else
            smoothTime = 0.2f;

        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
    }




    //
    //      ACTION
    //

    // [CODI BUG]
    // Quand on clic sur la m�me case �a fait quand meme passer un tour, � r�parer
    public virtual void Move(GameObject spot)
    {
        List<GameObject> adjSpot = currentSpot.GetComponent<Spot>().GetAdjacentSpots();
    
        if (isHide)
        {
            smoothTime = (0.2F + GlobalConst.TIME_TURN_SEC) * 2;
            onPath = true;
            GameManager.instance.effectList.Add(new EffectOnPath(this, this));
        }

        if (adjSpot.Contains(spot))
        {
            currentSpot = spot;
            Transform t_spot = currentSpot.transform;
            target = new Vector3(t_spot.position.x, t_spot.position.y, t_spot.position.z);
        }
        else
        {
            AStarMove();
        }

    }

    private void AStarMove()
    {
        //TODO
    }

    public void Hide()
    {
        isHide = true;
        animator.SetBool("hide", isHide);
    }

    public void UnHide()
    {
        isHide = false;
        animator.SetBool("hide", isHide);
    }

    public void Rest()
    {
        //TODO rest
    }
    




    //
    //      EFFECT
    //
    public void ResetOnPath()
    {
        onPath = false;
    }








    //
    //      COMMAND
    //
    public virtual void CommandEmpty()
    {
        stackAction.Push(new ActionEmpty(this));
    }

    public virtual void CommandMove(GameObject spot)
    {
        if(isHide)
            stackAction.Push(new ActionEmpty(this));
        
        stackAction.Push(new ActionMove(this, spot));
    }

    public virtual void CommandHide()
    {
        stackAction.Push(new ActionHide(this));
    }

    public virtual void CommandRest()
    {
        stackAction.Push(new ActionRest(this));
    }





    
    
    
    //
    //      OTHER
    //
    public bool isValideMove(GameObject spot)
    {
        if (spot == currentSpot)
            return false;

        return true;
    }







    //
    //      GET & SET
    //
    public TileType GetCurrentTileType()
    {
        return currentSpot.transform.parent.GetComponent<Tile>().GetTileData().tileType;
    }

    public LocationType GetCurrentLocationType()
    {
        return currentSpot.GetComponent<Spot>().GetLocationInSpot();
    }

    public GameObject GetCurrentSpot()
    {
        if (onPath)
            return null;
        else
            return currentSpot;
    }

    public void SetCurrentSpot(GameObject spot)
    {
        currentSpot = spot;
    }

    public void SetTarget(Vector3 t)
    {
        target = t;
    }

    public List<ButtonType> GetCurrentButtonAction()
    {
        return currentSpot.GetComponent<Spot>().GetActionInSpot();
    }

    public Action PopAction()
    {
        return stackAction.Pop();
    }

    public bool isStackActionEmpty()
    {
        if(stackAction.Count == 0)
            return true;
        else
            return false;
    }

    public virtual bool isPlayer()
    {
        return false;
    }

    public bool GetHide()
    {
        return isHide;
    }

    public bool GetOnPath()
    {
        return onPath;
    }


}