using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


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
    private bool onPath; // quand on est on path on est sur le chemin vers le currentSpot mais on est pas encore sur ce spot à ce tour
    public bool onFight;

    private Stack<Action> stackAction = new Stack<Action>();
    private bool canceled; // si il est canceled il ne peut par faire sont action de la actionQueue

    //LIFE
    public bool isDying; //pour le fight
    public bool isDead;

    //FIGHT
    public CombatManager currentCombatManager;
    public List<Character> selectedCharacters = new List<Character>();
    public SkillData currentLoadedSkill;
    public MyObject currentLoadedObject;
    public int nbGarde;

    //INVENTORY
    public List<MyObject> objectInventory = new List<MyObject>();
    [HideInInspector] public List<Armor> armorsEquiped = new List<Armor>();
    [HideInInspector] public List<Weapon> weaponHands = new List<Weapon>();
    private int pointerHands = 0;

    //SHOP
    public List<MyObject> objectToSell = new List<MyObject>();

    //RELATION
    public Dictionary<Character, Relation> charactersEncountered = new Dictionary<Character, Relation>();

    //LEADER
    public Character leaderCharacter;
    public List<Character> followersCharacters;

    //STATS  [CODE REFACTOT] Faut refaire ça en list peut etre jsp
    //      s_XXXXX -> stats fixe
    //      c_XXXXX -> curent stat
    public int s_VITALITY, s_ENDURANCE, s_STRENGHT, s_DEXTERITY, s_FAITH;
    public int c_VITALITY, c_ENDURANCE, c_STRENGHT, c_DEXTERITY, c_FAITH;
    public int xp, lvl=1, lvlPoint, gold;

    //EFFECT
    public List<Buff> listBuff = new List<Buff>();

    //ASTAR
    //public List<List<GameObject>> paths = new List<List<GameObject>>();
    private bool AStarSucces;

    public virtual void Start()
    {
        if(!characterData.inLocation)
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

        gold +=       characterData.init_GOLD;
    }

    void Update() // [CODE PRUDENCE] faire attention au modification de valeur dans cette update
    {
        if (GameManager.instance.playerCharacter?.onFight == false)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target + (Vector3)offSetOnSpot, ref velocity, smoothTime);
        }

        if (isDead || characterData.inLocation)
            Visible(false);
    }

    //
    //      ACTION
    //
    // [CODI BUG]
    // Quand on clic sur la même case ça fait quand meme passer un tour, à réparer
    public List<List<GameObject>> paths = new List<List<GameObject>>();

    public virtual void Move(GameObject spot)
    {
        List<GameObject> adjSpot = currentSpot.GetComponent<Spot>().GetAdjacentSpots();
        

        if (isHide)
        {
            onPath = true;
            GameManager.instance.effectList.Add(new EffectOnPath(this, this));
        }

        if (adjSpot.Contains(spot) || GameManager.instance.debugTeleport || spot == currentSpot)
            Teleport(spot);
        else
        {
            paths = new List<List<GameObject>>();
            List<GameObject> bestPath = new List<GameObject>();
            int bestNbSpot = 1000;
            List<GameObject> way = new List<GameObject>();
            way.Add(currentSpot);
            AStarSucces = false;
            AStarMove(spot,currentSpot, way);
            if(AStarSucces)
            {
                foreach (List<GameObject> path in paths)
                    if (path.Count < bestNbSpot)
                    {
                        bestPath = path;
                        bestNbSpot = path.Count;
                    }

                bestPath.RemoveAt(0);
                Teleport(bestPath[0]);
                bestPath.RemoveAt(0);
                bestPath.Reverse();
                foreach (GameObject _spot in bestPath)
                    stackAction.Push(new ActionMove(this, _spot));
            }
            else
            {
                Debug.Log("AStarMove TO DEEP");
            }

            
        }
    }


    private void AStarMove(GameObject destinationSpot, GameObject currentSpot, List<GameObject> spots)
    {
        if(spots.Count > GameManager.instance.AstarDeep)
        {
            return;
        }

        if(destinationSpot == currentSpot)
        {
            paths.Add(spots);
            AStarSucces = true;
            return;
        }
        else
        {
            foreach (GameObject adjSpot in currentSpot.GetComponent<Spot>().GetAdjacentSpots())
            {
                if(spots.Contains(adjSpot) == false)
                {
                    List<GameObject> _spots = new List<GameObject>();

                    foreach (GameObject spot in spots)
                        _spots.Add(spot);

                    _spots.Add(adjSpot);
                    AStarMove(destinationSpot, adjSpot, _spots);
                }

            }

        }

    }


    public void Teleport(GameObject spot)
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
        if(gold > 0 && c_ENDURANCE != s_ENDURANCE)
        {
            lastSpot = currentSpot;
            c_ENDURANCE++;
            if (c_ENDURANCE > s_ENDURANCE) c_ENDURANCE = s_ENDURANCE;
            gold--;
            GameManager.instance.UpdateTmpInfo();
        }
    }

    public void Search()
    {
        StartCoroutine(_Search());
    }

    public IEnumerator _Search()
    {
        lastSpot = currentSpot;
        Spot _currentSpot = currentSpot.GetComponent<Spot>();
        string dialog = "TAKE ALL ?\n\n";

        foreach (MyObject obj in _currentSpot.objectsOnSpot)
            dialog += "-" + obj.objectData.name + "\n";

        //BLOC pour les DialogBox
        GameManager.instance.OpenDialogWindow(dialog);
        yield return new WaitWhile(() => GameManager.instance.dialogAnswer == AnswerButton.WAIT);
        GameManager.instance.CloseDialogWindow();

        if (GameManager.instance.dialogAnswer == AnswerButton.YES)
            for (int i = 0; i < _currentSpot.objectsOnSpot.Count+1; i++)
                this.AddObject(_currentSpot.TakeObject());

        GameManager.instance.dialogAnswer = AnswerButton.WAIT;
    }

    public void Hire()
    {
        StartCoroutine(_Hire());
    }

    public IEnumerator _Hire()
    {
        lastSpot = currentSpot;
        if (isPlayer())
        {
            HireUI.instance.OpenHireUI();
            yield return new WaitWhile(() => GameManager.instance.dialogAnswer == AnswerButton.WAIT);
            GameManager.instance.dialogAnswer = AnswerButton.WAIT;
        }
        else
        {
            //TODO IA HIRE
        }
    }

    public bool HireCharacter(Character characterToHire)
    {
        if (characterToHire.characterData.workCost <= gold)
        {
            gold -= characterToHire.characterData.workCost;
            AddFollower(this, characterToHire);
            characterToHire.CancelAction();
            GameManager.instance.effectList.Add(new EffectHireEnd(characterToHire, this, 10)); //[CODE WARNING] valeur en dur, dupliqué
            GameManager.instance.UpdateTmpInfo();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddFollower(Character leader, Character follower)
    {
        follower.leaderCharacter = leader;
        followersCharacters.Add(follower);
        leader.SpreadRelation();
    }

    public void AddFollowers(Character leader, List<Character> followers)
    {
        foreach (Character follower in followers)
            AddFollower(leader, follower);
    }

    public void Trade()
    {
        StartCoroutine(_Trade());
    }

    public IEnumerator _Trade()
    {
        lastSpot = currentSpot;
        if (isPlayer())
        {
            ShopUI.instance.OpenShopUI();
            yield return new WaitWhile(() => GameManager.instance.dialogAnswer == AnswerButton.WAIT);
            GameManager.instance.dialogAnswer = AnswerButton.WAIT;
        }
        else
        {
            //TODO IA HIRE
        }
    }

    public bool BuyItem(MyObject myObject)
    {
        if (myObject.objectData.price <= gold)
        {
            gold -= myObject.objectData.price;
            if (myObject.isArmor())
                armorsEquiped.Add((Armor)myObject);
            else
                objectInventory.Add(myObject);
            return true;
        }
        else
        {
            return false;
        }
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
    public void MettingOnPath() //Utiliser dans GameManager dans ExecuteActionQueue
    {
        if (GameManager.instance.debugName == gameObject.name)
        {
            ;
        }

        if (currentSpot != lastSpot)
        {
            List<Character> charactersOnPath = new List<Character>();
            List<Character> allCharactersInTwoSpot = new List<Character>();
            bool wantBattle = false;

            allCharactersInTwoSpot.AddRange(lastSpot.GetComponent<Spot>().GetAllCharactersAliveOnMapInSpot());
            allCharactersInTwoSpot.AddRange(currentSpot.GetComponent<Spot>().GetAllCharactersAliveOnMapInSpot());

            
            //BUG AVEC DES DOUBLON 

            foreach (Character characterOnPath in allCharactersInTwoSpot)
            {
                if ((characterOnPath.lastSpot == currentSpot &&
                     characterOnPath.currentSpot == lastSpot &&
                     characterOnPath.lastSpot != characterOnPath.currentSpot)
                    ||
                    (characterOnPath.lastSpot == lastSpot &&
                     characterOnPath.currentSpot == currentSpot &&
                     characterOnPath.lastSpot != characterOnPath.currentSpot))
                {
                    charactersOnPath.Add(characterOnPath);

                    if (charactersEncountered.ContainsKey(characterOnPath) == false)
                        charactersEncountered.Add(characterOnPath, GameManager.instance.relationshipsBoard[(characterData.race, characterOnPath.characterData.race)]);

                    if (WantToFight(characterOnPath))
                        wantBattle = true;
                }
            }

            if (isPlayer())
                Debug.Log(charactersOnPath.Count);

            if (wantBattle && leaderCharacter == null)
            {
                StartCoroutine(GameManager.instance.StartFight(charactersOnPath, this, true));
            }
        }
        
        
    }

    public void MettingOnSpot()
    {
        foreach (Character characterOnSpot in currentSpot.GetComponent<Spot>().GetAllCharactersAliveOnMapInSpot())
        {
            if (charactersEncountered.ContainsKey(characterOnSpot) == false)
            {
                charactersEncountered.Add(characterOnSpot, GameManager.instance.relationshipsBoard[(this.characterData.race, characterOnSpot.characterData.race)]);
            }
        }
    }

    //Relation
    public void SpreadRelation()
    {
        if(leaderCharacter != null)
        {
            leaderCharacter.RelationInfluence(this);
            leaderCharacter.LeaderSpreadRelation();
        }
        else
        {
            LeaderSpreadRelation();
        }
    }

    public void LeaderSpreadRelation()
    {
        foreach (Character character in followersCharacters)
            character.RelationInfluence(this);
    }

    public void RelationInfluence(Character influencer)
    {
        foreach (var item in influencer.charactersEncountered)
            charactersEncountered[item.Key] = item.Value;
    }

    public bool WantToFight(Character character)
    {
        if (character == this)
            return false;

        if(charactersEncountered.ContainsKey(character))
        {
            if (charactersEncountered[character] == Relation.HOSTIL ||
                charactersEncountered[character] == Relation.ENNEMY)
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

    public virtual void CommandWait()
    {
        stackAction.Push(new ActionMove(this, currentSpot));
    }

    public virtual void CommandMove(GameObject spot)
    {
        if(isHide)
            stackAction.Push(new ActionEmpty(this));
        
        stackAction.Push(new ActionMove(this, spot));

        foreach (Character character in followersCharacters)
        {
            character.stackAction.Clear();
            character.stackAction.Push(new ActionMove(character, spot));
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

    public virtual void CommandSearch()
    {
        stackAction.Push(new ActionSearch(this));
    }

    public virtual void CommandHire()
    {
        stackAction.Push(new ActionHire(this));
    }

    public virtual void CommandeTrade()
    {
        stackAction.Push(new ActionTrade(this));
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

    public bool isEquipedObject(MyObject obj)
    {
        if (armorsEquiped.Contains(obj)) return true;
        if (weaponHands.Contains(obj)) return true;
        return false;
    }

    public void EquipArmor(Armor armor)
    {
        ArmorData firstArmorDataEquiped = null;
        ArmorData armorDataToEquip = (ArmorData)armor.objectData;

        if (armorsEquiped.Count == 0)
        {
            armorsEquiped.Add(armor);
        }
        else
        {
            firstArmorDataEquiped = (ArmorData)armorsEquiped[0].objectData;
            if(firstArmorDataEquiped.isAdditive == false)
            {
                if (armorDataToEquip.isAdditive == false)
                    armorsEquiped[0] =armor;
                else
                    armorsEquiped.Insert(0, armor);
            }
            else
            {
                if (armorDataToEquip.isAdditive == false)
                    armorsEquiped.Insert(0, armor);
                else
                    armorsEquiped.Add(armor);
            }
        }
    }

    public void EquipWeapon(Weapon weapon)
    {
        WeaponData weaponData = (WeaponData)weapon.objectData;
        if (weaponData.nbHand == 2)
        {
            weaponHands[0] = weapon;
            weaponHands[1] = weapon;
        }
        else
        {
            if(weaponHands[1] == weaponHands[0])
            {
                weaponHands[0] = null;
                weaponHands[1] = null;
            }
            
            weaponHands[pointerHands] = weapon;
            pointerHands = (pointerHands + 1) % 2;
        }
    }

    public void AutoEquip()
    {

        if(this == GameManager.instance.playerCharacter)
        {
            ;
        }

        foreach (MyObject obj in objectInventory)
        {
            if (obj.isArmor())
                EquipArmor((Armor)obj);

            if (obj.isWeapon())
                EquipWeapon((Weapon)obj);
        }
    }

    public void AddObject(MyObject obj) //[CODE DELETE] Methode a supprimé ne marche pas car il n'y a pas de cast sur obj
    {
        if (obj == null) return;

        objectInventory.Add(obj);

        if (obj.isArmor())
            armorsEquiped.Add((Armor)obj);
    }

    public void AddXp(int amount)
    {
        xp += amount;
        if (xp >= lvl * 5 && xp !=0)
        {
            lvl++;
            lvlPoint += 5;
        }
        GameManager.instance.UpdateTmpInfo();
    }

    //      FIGHT
    public void TakeDamage(Character characterAttacker, int amount, DamageType damageTypeAttack, Element elementAttack = Element.NONE)
    {
        charactersEncountered[characterAttacker] = Relation.ENNEMY;
        SpreadRelation();
        
        int amountRest = amount;
        int amountModified;
        while (amountRest > 0)
        {
            if (armorsEquiped.Count != 0)
            {
                if (damageTypeAttack == DamageType.ELEM)
                    amountModified = (int)(GameManager.instance.typeChart[(elementAttack.ToString(), armorsEquiped[armorsEquiped.Count - 1].objectData.material)] * amountRest);
                else
                    amountModified = (int)(GameManager.instance.typeChart[(damageTypeAttack.ToString(), armorsEquiped[armorsEquiped.Count - 1].objectData.material)] * amountRest);

                amountRest =  amountModified - armorsEquiped[armorsEquiped.Count - 1].c_STATE;
                armorsEquiped[armorsEquiped.Count - 1].c_STATE -= amountModified;

                if (armorsEquiped[armorsEquiped.Count - 1].c_STATE <= 0)
                    armorsEquiped.RemoveAt(armorsEquiped.Count - 1);
            }
            else
            {
                if (damageTypeAttack == DamageType.ELEM)
                    amountModified = (int)(GameManager.instance.typeChart[(elementAttack.ToString(), characterData.shape)] * amountRest);
                else
                    amountModified = (int)(GameManager.instance.typeChart[(damageTypeAttack.ToString(), characterData.shape)] * amountRest);

                amountRest = 0;
                c_VITALITY -= amountModified;
                if (c_VITALITY <= 0)
                {
                    if (characterAttacker.isPlayer())
                    {
                        characterAttacker.AddXp(characterData.drop_XP);
                        characterAttacker.gold += gold;
                    }
                    Die();
                }
            }
        }
        


        
        
    }

    public void TakeHeal(int amount)
    {
        c_VITALITY += amount;
        if (c_VITALITY > s_VITALITY)
            c_VITALITY = s_VITALITY;
    }

    public void TakeBuff(SkillBuffData skillBuffData)
    {
        Buff buff = new Buff(this, skillBuffData.nbRound, skillBuffData.nbTurn);
    }

    public bool RequirementSkill(SkillData skillData)
    {
        if (c_VITALITY  >= skillData.req_VITALITY  &&
            c_ENDURANCE >= skillData.req_ENDURANCE &&
            c_DEXTERITY >= skillData.req_DEXTERITY &&
            c_FAITH     >= skillData.req_FAITH     &&
            c_STRENGHT  >= skillData.req_STRENGHT)
            return true;
        else
            return false;
    }

    public bool ParryableAttack(SkillAttackData skillAttackDataTaken)
    {
        if (currentLoadedSkill == null ||
            currentLoadedSkill.skillType != SkillType.PARRY)
            return false;

        SkillParryData skillParryData = (SkillParryData)currentLoadedSkill;

        if (nbGarde > 0)
            if (skillParryData.parryDamageType.Contains(skillAttackDataTaken.damageType))
                return true;

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
            if(isPlayer()) GameManager.instance.inputBlock = true;
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
    public void AI_MakeCrew()
    {
        if (leaderCharacter != null) return;

        foreach (Character character in currentSpot.GetComponent<Spot>().GetAllCharactersAliveOnMapInSpot())
        {
            if(character != this && 
               character.characterData.name == this.characterData.name &&
               followersCharacters.Contains(character) == false)
            {
                AddFollower(this, character);
            }
        } 
    }

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

    public void AI_SetRandomLoadedSkill(List<Character> allCharacterInFight) //TODO à ameliorer
    {
        Weapon rngWeapon = AI_TakeRandomWeapon();
        if(rngWeapon == null) Debug.LogWarning("AI_SetRandomLoadedSkill: PNJ without weapon");
        WeaponData wd = (WeaponData)rngWeapon.objectData;
        currentLoadedObject = rngWeapon;

        int countWeapon = wd.skills.Count;
        currentLoadedSkill = wd.skills[Random.Range(0, countWeapon)];

        selectedCharacters = AI_SelectTargets(allCharacterInFight, currentLoadedSkill.nbTarget);
    }

    public List<Character> AI_SelectTargets(List<Character> allCharacterInFight, int maxTarget)
    {
        List<Character> charactersTarget = new List<Character>();
        List<Character> res = new List<Character>();

        if(leaderCharacter != null)
            RelationInfluence(leaderCharacter);

        foreach (Character characterTargetable in allCharacterInFight)
        {
            if (WantToFight(characterTargetable))
                charactersTarget.Add(characterTargetable);
        }

        if (charactersTarget.Count == 0)
            return null;

        List<int> rngTarget = new List<int>();

        for (int i = 0; i < maxTarget; i++)
        {
            int rng = Random.Range(0, charactersTarget.Count);
            while (rngTarget.Contains(rng))
                rng = Random.Range(0, charactersTarget.Count);

            rngTarget.Add(rng);
        }

        for (int i = 0; i < maxTarget; i++)
            res.Add(charactersTarget[rngTarget[i]]);
        return res;
    }



    public void AI_Command()
    {
        if (gameObject.name == GameManager.instance.debugName) 
        {
            ;
        }

        if (isDead == true)
        {
            CommandEmpty();
        }
        else
        {
            bool fightFound = false;
            List<Character> charactersInSpot = currentSpot.GetComponent<Spot>().GetAllCharactersAliveOnMapInSpot();
            foreach (Character _characterInSpot in charactersInSpot)
            {
                if (WantToFight(_characterInSpot))
                {
                    fightFound = true;
                }
            }

            if (fightFound && leaderCharacter == null)
            {
                CommandFight();
            }
            else
            {
                if (leaderCharacter == null)
                {
                    switch (characterData.currentMood)
                    {
                        case MoodAI.STATIC:
                            CommandEmpty();
                            break;
                        case MoodAI.MOVING_AREA:

                            float rng2 = Random.Range(0f, 1f);
                            
                            if(rng2<0.3f)
                            {
                                TileType startTileType = currentSpot.GetComponent<Spot>().parrentTile.GetTileData().tileType;
                                List<GameObject> adjSpot = GetCurrentSpot().GetComponent<Spot>().GetAdjacentSpots();
                                bool find = false;
                                int rng = 0;
                                while (find == false)
                                {
                                    rng = Random.Range(0, adjSpot.Count);
                                    if (adjSpot[rng].GetComponent<Spot>().parrentTile.GetTileData().tileType == startTileType)
                                        find = true;
                                }
                                CommandMove(adjSpot[rng]);
                            }
                            else
                            {
                                CommandWait();
                            }

                            
                            break;
                        case MoodAI.CHASE:
                            break;
                    }
                }
                else
                {
                    //character.CommandMove(character.leader.GetCurrentSpot());
                    if (leaderCharacter.isDead)
                    {
                        leaderCharacter = null;
                        AI_Command();
                    }
                }
            }
        }
    }

    //
    //      GET & SET
    //
    public void SetUp(CharacterData cd, GameObject spot)
    {
        characterData = cd;
        currentSpot = spot;
        lastSpot = spot;
        Transform t_spot = spot.transform;
        SetTarget(new Vector3(t_spot.position.x, t_spot.position.y, t_spot.position.z));
        UpdateSmoothTime();
        Teleport(spot);

        weaponHands.Add(null);
        weaponHands.Add(null);

        if (!isPlayer())
            AI_Command();
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
