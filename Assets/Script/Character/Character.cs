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
    public Vector2 offSetOnSpot;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D collider2d; //[CODE INUTILE]
    private bool inFight;

    public Animator animator;

    [SerializeField] private GameObject currentSpot;
    [SerializeField] private GameObject lastSpot;
    [SerializeField] private bool isHide;
    private bool onPath; // quand on est on path on est sur le chemin vers le currentSpot mais on est pas encore sur ce spot � ce tour

    private Stack<Action> stackAction = new Stack<Action>();
    private bool canceled; // si il est canceled il ne peut par faire sont action de la actionQueue

    //LIFE
    public bool isDying; //pour le fight
    public bool isDead; 

    //FIGHT
    public List<Character> selectedCharacters = new List<Character>();
    public SkillData currentLoadedSkill;
    public int nbGarde;

    //INVENTORY
    public List<MyObject> objectInventory = new List<MyObject>();
    public Stack<Armor> armorsEquiped = new Stack<Armor>();

    //RELATION
    public Dictionary<Character, Relation> charactersEncountered = new Dictionary<Character, Relation>();

    //STATS 
    //      s_XXXXX -> stats fixe
    //      c_XXXXX -> curent stat
    public int s_VITALITY, s_ENDURANCE, s_STRENGHT, s_DEXTERITY, s_FAITH;
    public int c_VITALITY, c_ENDURANCE, c_STRENGHT, c_DEXTERITY, c_FAITH;


    public virtual void Start()
    {
        spriteRenderer.sprite = characterData.spriteMap;
        s_VITALITY =  characterData.init_VITALITY;
        s_ENDURANCE = characterData.init_ENDURANCE;
        s_STRENGHT =  characterData.init_STRENGHT;
        s_DEXTERITY = characterData.init_DEXTERITY;
        s_FAITH =     characterData.init_FAITH;

        c_VITALITY =  s_VITALITY;
        c_ENDURANCE = s_ENDURANCE;
        c_STRENGHT =  s_STRENGHT;
        c_DEXTERITY = s_DEXTERITY;
        c_FAITH =     s_FAITH;
    }

    void Update() // [CODE PRUDENCE] faire attention au modification de valeur dans cette update
    {
        if(CombatManager.instance.playerOnFight == false)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target+(Vector3)offSetOnSpot, ref velocity, smoothTime);
        }

        if(isDead)
            Visible(false);
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
            onPath = true;
            GameManager.instance.effectList.Add(new EffectOnPath(this, this));
        }

        if (adjSpot.Contains(spot))
        {
            if (currentSpot != null)
            {
                currentSpot.GetComponent<Spot>().RemoveCharacterInSpot(this);
                lastSpot = currentSpot;
            }
            
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

    public void Search()
    {
        //TODO Search
        //Pour l'instant on prend le 1er object

        this.AddObject(currentSpot.GetComponent<Spot>().TakeObject());
    }

    public void UpdateSmoothTime()
    {
        if (isHide)
            smoothTime = GlobalConst.HIDE_SMOOTHTIME;
        else
            smoothTime = GlobalConst.BASIC_SMOOTHTIME;
    }


    //
    //      RELATION
    //
    public IEnumerator MettingOnPath()
    {
        List<Character> charactersOnPath = new List<Character>();
        List<Character> allCharactersInTwoSpot = new List<Character>();
        bool wantBattle = false;

        
        allCharactersInTwoSpot.AddRange(lastSpot.GetComponent<Spot>().GetAllCharactersInSpot());
        allCharactersInTwoSpot.AddRange(currentSpot.GetComponent<Spot>().GetAllCharactersInSpot());

        foreach (Character characterOnPath in allCharactersInTwoSpot)
        {
            if((characterOnPath.lastSpot    == this.currentSpot &&
                characterOnPath.currentSpot == this.lastSpot &&
                characterOnPath.isDead      == false) 
                    ||                
               (characterOnPath.currentSpot == this.currentSpot &&
                characterOnPath.lastSpot    == this.lastSpot &&
                characterOnPath.isDead      == false))
            {
                charactersOnPath.Add(characterOnPath);

                if (charactersEncountered.ContainsKey(characterOnPath) == false)
                    charactersEncountered.Add(characterOnPath, GameManager.instance.GetRelationRace(this.characterData.race, characterOnPath.characterData.race));

                if (WantToFight(characterOnPath))
                    wantBattle = true;

            }
        }

        if (wantBattle)
        {
            //Debug.Log(this.gameObject.name + " start fight with count: " + charactersOnPath.Count + "  wantBattle = " + wantBattle);
            yield return new WaitForSeconds(0.3f);
            GameManager.instance.StartFight(charactersOnPath);
        }
    }

    public void MettingOnSpot()
    {
        foreach (Character characterOnSpot in currentSpot.GetComponent<Spot>().GetAllCharactersInSpot())
        {
            if (charactersEncountered.ContainsKey(characterOnSpot) == false)
            {
                charactersEncountered.Add(characterOnSpot, GameManager.instance.GetRelationRace(this.characterData.race, characterOnSpot.characterData.race));
            }
        }
    }


    public bool WantToFight(Character character)
    {
        if (isDead == true || character == this || character.isDead == true)
            return false;

        if(charactersEncountered.ContainsKey(character))
        {
            if (charactersEncountered[character] == Relation.HOSTIL ||
            charactersEncountered[character] == Relation.ENNMY)
                return true;
            else
                return false;
        }
        else
        {
            return false;
        }
        
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
    //void OnMouseEnter()
    //{
    //    GameManager.instance.CreateInfoGridLayoutGroupe(transform.position, this);
    //}

    //void OnMouseExit()
    //{
    //    GameManager.instance.DestroyInfoGridLayoutGroupe();
    //}


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

    public virtual void CommandFight()
    {
        stackAction.Push(new ActionFight(this));
    }

    public virtual void CommandSearch()
    {
        stackAction.Push(new ActionSearch(this));
    }

    public void CancelAction()
    {
        stackAction.Clear();
        canceled = true;
    }

    public void CancelReset()
    {
        canceled = false;
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

    public void AddObject(MyObject obj)
    {
        if (obj == null) return;

        objectInventory.Add(obj);

        if (obj.isArmor())
            armorsEquiped.Push((Armor)obj);
    }

    //      FIGHT
    public void TakeDamage(int amount)
    {
        //TODO A REFAIRE POUR ARMOR
        if(armorsEquiped.Count != 0)
        {
            armorsEquiped.Peek().currentState -= amount;
            if (armorsEquiped.Peek().currentState <= 0)
                armorsEquiped.Pop();
        }
        else
        {
            c_VITALITY -= amount;
            if (c_VITALITY <= 0)
            {
                Die();
            }
        }
    }

    public void TakeHeal(int amount)
    {
        c_VITALITY += amount;
        if (c_VITALITY > s_VITALITY)
            c_VITALITY = s_VITALITY;
    }

    public bool ParryableAttack(SkillData skillDataTaken)
    {
        //if(nbGarde > 0)
        //    if(currentLoadedSkill.damageTypeParryable == skillDataTaken.damageType)
        //       return true;

        return false;
    }

    public void Die() //[CODI BE CARFULL] Il faut appeler la fonction Die 2 fois pour tuer un character
    {
        if (isDying == false)
        {
            //Debug.Log(this + " dying");
            isDying = true;
        }
        else
        {
            //Debug.Log(this + " dead");
            isDead = true;
            //DieForMap();
        }
    }

    private void DieForMap()
    {
        spriteRenderer.gameObject.SetActive(false);
    }

    public void Visible(bool enable)
    {
        spriteRenderer.enabled = enable;
        collider2d.enabled = enable;
    }

    //      IA
    public Weapon AI_TakeRandomWeapon()
    {
        int countInventory = objectInventory.Count;
        int rng = Random.Range(0, countInventory);
        for (int i = rng; i < objectInventory.Count + rng; i++)
        {
            if(objectInventory[i% objectInventory.Count].isWeapon())
            {
                return (Weapon)objectInventory[i % objectInventory.Count];
            }
        }

        Debug.LogWarning("AI_TakeRandomWeapon: zero weapon in inventory");
        return null;
    }

    public void AI_SetRandomLoadedSkill(List<Character> allCharacterInFight) //TODO � ameliorer
    {
        Weapon rngWeapon = AI_TakeRandomWeapon();
        WeaponData wd = (WeaponData)rngWeapon.objectData;

        int countWeapon = wd.skills.Count;
        currentLoadedSkill = wd.skills[Random.Range(0, countWeapon)];

        selectedCharacters = AI_SelectTargets(allCharacterInFight, currentLoadedSkill.nbTarget);
    }

    public List<Character> AI_SelectTargets(List<Character> allCharacterInFight, int maxTarget)
    {
        List<Character> charactersTarget = new List<Character>();
        List<Character> res = new List<Character>();

        foreach (Character characterTargetable in allCharacterInFight)
        {
            if (WantToFight(characterTargetable))
                charactersTarget.Add(characterTargetable);
        }

        if (charactersTarget.Count == 0)
            return null;

        for (int i = 0; i < maxTarget; i++)
        {
            res.Add(charactersTarget[i]);
        }

        return res;
        

    }











    //
    //      GET & SET
    //
    public void SetUp(CharacterData characterData, GameObject spot)
    {
        this.characterData = characterData;
        this.currentSpot = spot;
        this.lastSpot = spot;
        Transform t_spot = spot.transform;
        this.SetTarget(new Vector3(t_spot.position.x, t_spot.position.y, t_spot.position.z));
        this.UpdateSmoothTime();
    }

    public List<Weapon> GetWeaponsInventory()
    {
        List<Weapon> weaponsInventory = new List<Weapon>();
        for (int i = 0; i < objectInventory.Count; i++)
            if (objectInventory[i].isWeapon())
                weaponsInventory.Add((Weapon)objectInventory[i]);
        return weaponsInventory;
    }

    public List<ActiveObject> GetActiveObjectInventory()
    {
        List<ActiveObject> activeObjectInventory = new List<ActiveObject>();
        for (int i = 0; i < objectInventory.Count; i++)
            if (objectInventory[i].isActiveObject())
                activeObjectInventory.Add((ActiveObject)objectInventory[i]);
        return activeObjectInventory;
    }

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
        if (stackAction.Count == 0)
            return null;

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

    public List<Character> GetAllCharactersInSpot()
    {
        return currentSpot.GetComponent<Spot>().GetAllCharactersInSpot();
    }

    public bool isCanceled()
    {
        return canceled;
    }

    public bool isInFight()
    {
        return inFight;
    }

    public void setInFight(bool b) 
    {
        inFight = b;
    }
}
