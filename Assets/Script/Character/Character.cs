using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    public CharacterData characterData;

    // REAL TIME MOVE
    private Vector3 target;
    private float smoothTime;
    private Vector3 velocity = Vector3.zero;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D collider2d;

    public Animator animator;

    [SerializeField] private GameObject currentSpot;
    [SerializeField] private bool isHide;
    private bool onPath; // quand on est on path on est sur le chemin vers le currentSpot mais on est pas encore sur ce spot à ce tour

    private Stack<Action> stackAction = new Stack<Action>();

    //GROUPE
    private Character leader; //si le character A un leader alors il suis certaine action de son leader;
    private List<Character> crewmates = new List<Character>(); //si le character EST un leader alors il a un crew
    private int idCrew = 0; //la place dans le groupe. 0 = etre leader



    //VIE ET CORPS
    private List<Member> corps = new List<Member>();



    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);

        // Deplacement des crewmate
        if (!isLeader() && Vector3.Distance(transform.position, target) < 0.2f)
            Visible(false);
        else
            Visible(true);
    }




    //
    //      ACTION
    //

    // [CODI BUG]
    // Quand on clic sur la même case ça fait quand meme passer un tour, à réparer
    public virtual void Move(GameObject spot)
    {
        List<GameObject> adjSpot = currentSpot.GetComponent<Spot>().GetAdjacentSpots();
    
        if (isHide)
        {
            onPath = true;
            GameManager.instance.effectList.Add(new EffectOnPath(this, this));
        }

        if (adjSpot.Contains(spot))
        {
            if (currentSpot != null)
                currentSpot.GetComponent<Spot>().RemoveCharacterInSpot(this);
            
            currentSpot = spot;

            currentSpot.GetComponent<Spot>().AddCharacterInSpot(this);

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
        UpdateSmoothTime();
    }

    public void UnHide()
    {
        isHide = false;
        animator.SetBool("hide", isHide);
        UpdateSmoothTime();
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
    //      FX
    //
    void OnMouseEnter()
    {
        if(isLeader())
            GameManager.instance.CreateInfoGridLayoutGroupe(transform.position, this);
    }

    void OnMouseExit()
    {
        GameManager.instance.DestroyInfoGridLayoutGroupe();
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


        //[CODE DOUTEUX] pas sur de la place de l'ordre
        if(isLeader())
        {
            foreach (Character character in crewmates)
            {
                character.CommandMove(spot);
            }
        }
    }

    public virtual void CommandHide()
    {
        stackAction.Push(new ActionHide(this));
    }

    public virtual void CommandRest()
    {
        stackAction.Push(new ActionRest(this));
    }

    public virtual void CommandFight()
    {
        stackAction.Push(new ActionFight(this));
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

    public void createHumanCorps()
    {
        corps.Add(new Member(MemberType.HEAD, Element.SKIN, 1, true));

        corps.Add(new Member(MemberType.BODY, Element.SKIN, 3, true));

        corps.Add(new Member(MemberType.ARM, Element.SKIN, 2, false));
        corps.Add(new Member(MemberType.ARM, Element.SKIN, 2, false));

        corps.Add(new Member(MemberType.LEG, Element.SKIN, 2, false));
        corps.Add(new Member(MemberType.LEG, Element.SKIN, 2, false));
    }





    //  
    //      CREW
    //
    public void UpdateSmoothTime()
    {
        if (isHide)
            smoothTime = GlobalConst.HIDE_SMOOTHTIME + GlobalConst.DELTA_SMOOTH_CREW * idCrew;
        else
            smoothTime = GlobalConst.BASIC_SMOOTHTIME + GlobalConst.DELTA_SMOOTH_CREW * idCrew;
    }

    // Rend visible les characater du group pendant le move du leader
    public void Visible(bool enable)
    {
        spriteRenderer.enabled = enable;
        collider2d.enabled = enable;
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

    public void SetCrewmates(List<Character> c)
    {
        crewmates = c;
    }

    public void SetLeader(Character l)
    {
        leader = l;
    }

    public bool isLeader()
    {
        if (leader == null)
            return true;
        else
            return false;
    }

    public void SetIdCrew(int i)
    {
        idCrew = i;
    }

    public List<Character> GetSquad()
    {
        List<Character> squad = new List<Character>();
        if (isLeader())
        {
            squad.Add(this);
            foreach (Character mates in crewmates)
            {
                squad.Add(mates);
            }
            return squad;
        }
        else
        {
            Debug.LogError("GetSquad only use by a leader");
            return null;
        }
        
    }
}
