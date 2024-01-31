using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    void Awake()
    {
        instance = this;
    }

    // DEBUG
    [Header("DEGUB")]
    public GameObject CircleTest;
    //public GameObject prefabTmpEnemy;

    // GLOBAL VAR
    [Header("GLOBAL VAR")]
    public GameObject cam;
    public GameObject cam_fight;
    public GameObject prefabPlayer;
    public GameObject prefabCharacter;
    public GameObject prefabObjectOnMap;

    [HideInInspector] public GameObject player;
    public Player playerCharacter;

    public WorldBuilder worldBuilder;
    public LocaleSelector localeSelector; 

    private int turn = 0;
    public bool inputBlock;

    public List<Action> actionQueue = new List<Action>(); // liste de chaque action de tout les characters du current turn
    public List<Character> characterList = new List<Character>(); // liste de tout les characters en jeu
    public List<Effect> effectList = new List<Effect>();

    // UI
    [Header("UI")]
    public Text nbTurn;
    public Image imgLandscape;
    [SerializeField] private Sprite[] allImgLandscape_tile;
    [HideInInspector] public Dictionary<TileType, Sprite> dic_imgLandscape_tile = new Dictionary<TileType, Sprite>();

    [SerializeField] private GameObject btn_grid;
    [SerializeField] private GameObject[] allButton;
    [HideInInspector] public Dictionary<ButtonType, GameObject> dic_button = new Dictionary<ButtonType, GameObject>();
    private List<GameObject> currentButtons = new List<GameObject>();

    [SerializeField] private Transform worldCanvas;
    [SerializeField] private GameObject actionCanvas;
    [SerializeField] private Vector3 offSetInfoGrid;
    [SerializeField] private GameObject prefabInfoGridLayoutGroupe;
    private GameObject currentInfoGridLayoutGroupe;
    [SerializeField] private GameObject prefabInfoCharacter;

    // NAME ENUM
    public Dictionary<WeaponStyle, string> dic_weaponStyle = new Dictionary<WeaponStyle, string>();
    public Dictionary<Element, string> dic_element = new Dictionary<Element, string>();



    //RACISME
    public List<RelationLine> relationshipsBoard = new List<RelationLine>();

    public void Start()
    {
        SetUpUI();
        SetUpDicEnum();
        SetUpRacisme();

        //--------SEED RNG---------
        //SeedRandom((int)Random.Range(0, 9999999));
        string seed = "Miriamo54";        

        SeedRandom(seed.GetHashCode());
        //-------------------------

        worldBuilder.StartWorldBuilder();
        localeSelector.ChangeLocale(0);

        StartCoroutine(StartLateOne());

        //UI
        AddTurn(0);
        



    }

    

    public IEnumerator StartLateOne() //[CODE BIZARE] Cette methode est appele dans start mais elle s'execute apres tout les autre starts
    {
        yield return new WaitForSeconds(1f);
        SetUpPlayer();
    }

    private void Update()
    {
        GetMouseInput();
    }

    //
    //      SETUP
    //
    private void SetUpUI()
    {
        dic_imgLandscape_tile.Add(TileType.LAND,     allImgLandscape_tile[0]);
        dic_imgLandscape_tile.Add(TileType.MOUNTAIN, allImgLandscape_tile[1]);
        dic_imgLandscape_tile.Add(TileType.WOOD,     allImgLandscape_tile[2]);

        dic_button.Add(ButtonType.TALK,   allButton[0]);
        dic_button.Add(ButtonType.HIDE,   allButton[1]);
        dic_button.Add(ButtonType.REST,   allButton[2]);
        dic_button.Add(ButtonType.UNHIDE, allButton[3]);
        dic_button.Add(ButtonType.FIGHT,  allButton[4]);
        dic_button.Add(ButtonType.SEARCH, allButton[5]);
    }

    private void SetUpDicEnum()
    {
        dic_weaponStyle.Add(WeaponStyle.LONG_SWORD, "Long sword");

        dic_element.Add(Element.DIVIN, "Divin");
        dic_element.Add(Element.FIRE, "Fire");
        dic_element.Add(Element.FUR, "Fur");
        dic_element.Add(Element.LEATHER, "Leather");
        dic_element.Add(Element.LIGHTNING, "Lightning");
        dic_element.Add(Element.METAL, "Metal");
        dic_element.Add(Element.PLUME, "Plume");
        dic_element.Add(Element.ROCK, "Rock");
        dic_element.Add(Element.SKIN, "Skin");
        dic_element.Add(Element.WATER, "Water");
        dic_element.Add(Element.WIND, "Wind");
        dic_element.Add(Element.WOOD, "Wood");
    }

    private void SetUpPlayer()
    {
        Vector2Int startLocation = new Vector2Int(1, 1);
        
        GameObject currentTile = GetTile(startLocation.x, startLocation.y);
        Spot[] spot = currentTile.GetComponentsInChildren<Spot>();
        player = CreateCharacter(ObjectManager.instance.FindCharacterData("Player"), spot[0].gameObject);
        playerCharacter = player.GetComponent<Player>();
        UpdateUILandscape();
        UpdateUIButtonGrid(playerCharacter.GetCurrentButtonAction());

        //Clean les nuages a la main
        List<GameObject> nearTiles = GetTiles(startLocation.x - 1,
                                              startLocation.y - 1,
                                              startLocation.x + 1,
                                              startLocation.y + 1);

        foreach (GameObject tile in nearTiles)
        {
            tile.GetComponentInChildren<Tile>().CleanCloud();
        }

    }

    public void SetUpRacisme()
    {
        //HUMAIN
        relationshipsBoard.Add(new RelationLine(Race.HUMAN, Race.HUMAN, Relation.FRIEND));
        relationshipsBoard.Add(new RelationLine(Race.HUMAN, Race.ELF, Relation.NEUTRAL));
        relationshipsBoard.Add(new RelationLine(Race.HUMAN, Race.BEAST, Relation.NEUTRAL));

        //ELF
        relationshipsBoard.Add(new RelationLine(Race.ELF, Race.HUMAN, Relation.NEUTRAL));
        relationshipsBoard.Add(new RelationLine(Race.ELF, Race.ELF, Relation.FRIEND));
        relationshipsBoard.Add(new RelationLine(Race.ELF, Race.BEAST, Relation.FRIEND));

        //BEAST
        relationshipsBoard.Add(new RelationLine(Race.BEAST, Race.HUMAN, Relation.HOSTIL));
        relationshipsBoard.Add(new RelationLine(Race.BEAST, Race.ELF, Relation.NEUTRAL));
        relationshipsBoard.Add(new RelationLine(Race.BEAST, Race.BEAST, Relation.FRIEND));
    }





    //
    //      INPUT PLAYER
    //
    private void GetMouseInput()
    {
        //CHEAT CODE
        if(Input.GetKeyDown(KeyCode.C)) //CLEAN CLOUD
        {
            List<GameObject> tiles = GetTiles(0, 0, GlobalConst.SIZE_BOARD, GlobalConst.SIZE_BOARD);

            foreach (GameObject item in tiles)
            {
                item.GetComponentInChildren<Tile>().CleanCloud();
            }
        }

        if(Input.GetKeyDown(KeyCode.I)) //CREATE ENEMY
        {
            GameObject currentTile = GetTile(2, 2);
            Spot[] spot = currentTile.GetComponentsInChildren<Spot>();

            GameObject newPnj = CreateCharacter(ObjectManager.instance.FindCharacterData("Wolf"), spot[0].gameObject);
            newPnj.GetComponent<Character>().CommandEmpty(); // [CODE TMP]
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            CameraController cc = cam.GetComponent<CameraController>();
            cc.ToggleFreezeCam();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            QuitCombatScene();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            foreach (Character character in characterList)
            {
                if (character.isPlayer())
                    character.weaponInventory.Add(CreateWeapon("CRACKED"));
                else
                    character.weaponInventory.Add(CreateWeapon("SWORD"));
            }
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            GameObject currentTile = GetTile(1,1);
            Spot[] spot = currentTile.GetComponentsInChildren<Spot>();

            MyObject obj = CreateObjectOnMap("Bottle");
            spot[0].AddObject(obj);
        }

        if(Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log(playerCharacter.objectInventory.Count);
            foreach (MyObject obj in playerCharacter.objectInventory)
            {
                Debug.Log(obj.objectData.name);
            }
        }

    }





    //
    //      GET & SET
    //
    public GameObject GetTile(int x, int y, bool debugDraw = false)
    {
        GameObject res = null;

        Vector2 pos = new Vector2(x * GlobalConst.OFF_SET_TILE, y * GlobalConst.OFF_SET_TILE);
        Collider2D[] target = Physics2D.OverlapCircleAll(pos, 1f);

        foreach (Collider2D item in target)
        {
            if (item.CompareTag("Tile"))
            {
                res = item.transform.gameObject;
                if (debugDraw) Instantiate(CircleTest, pos, Quaternion.identity);
            }
        }
        return res;
    }

    /*
        Ca recupere les tiles dans un caré comme ça  

        ###  ###  ###       +     +    +   
        ###  ###  ###       +     +    y2  
        ###  ###  ###       +     +    x2
        
        ###  ###  ###       +     +    +  
        ###  ###  ###       +     +    +
        ###  ###  ###       +     +    +

        ###  ###  ###       +     +    +     
        ###  ###  ###       x1    +    +
        ###  ###  ###       y1    +    +
     */
    public List<GameObject> GetTiles(int x1, int y1, int x2, int y2, bool debugDraw = false)
    {
        List<GameObject> res = new List<GameObject>();

        for (int i = x1; i <= x2; i++)
        {
            for (int j = y1; j <= y2; j++)
            {
                GameObject newTile = GetTile(i, j, debugDraw);
                if (newTile != null) res.Add(newTile);
            }
        }
        return res;
    }

    public List<ButtonType> GetStandarActionButton()
    {
        List<ButtonType> actionButtonStandar = new List<ButtonType>();
        actionButtonStandar.Add(ButtonType.REST);

        if(playerCharacter.GetHide() == false)
            actionButtonStandar.Add(ButtonType.HIDE);
        else
            actionButtonStandar.Add(ButtonType.UNHIDE);

        actionButtonStandar.Add(ButtonType.FIGHT);
        actionButtonStandar.Add(ButtonType.SEARCH);

        return actionButtonStandar;
    }
    public Relation GetRelationRace(Race race_judge, Race race_eval)
    {
        foreach (RelationLine relation in relationshipsBoard)
        {
            if(race_judge == relation.race_jugde && race_eval == relation.race_eval)
            {
                return relation.relation;
            }
        }

        Debug.LogError("ERROR RELATION");
        return Relation.ALLY;
    }




    //
    //      UI
    //
    public void AddTurn(int amount)
    {
        turn += amount;
        nbTurn.text = turn.ToString();
    }

    public void UpdateUILandscape(LocationType location = LocationType.EMPTY)
    {
        if(location == LocationType.EMPTY)
            imgLandscape.sprite = dic_imgLandscape_tile[playerCharacter.GetCurrentTileType()];
    }

    //A chaque apelle de la methode on destroy tous les bouton est on les recrées
    public void UpdateUIButtonGrid(List<ButtonType> buttonType) 
    {
        foreach (GameObject btn in currentButtons)
        {
            Destroy(btn);
        }

        foreach (ButtonType btnType in buttonType)
        {
            GameObject newBtn = Instantiate(dic_button[btnType], btn_grid.transform);
            currentButtons.Add(newBtn);
        }
    }

    public void CreateInfoGridLayoutGroupe(Vector3 pos, List<Character> characters)
    {
        Vector3 newPos = Vector3.zero;
        newPos.x = offSetInfoGrid.x + pos.x;
        newPos.y = offSetInfoGrid.y + pos.y;

        currentInfoGridLayoutGroupe = Instantiate(prefabInfoGridLayoutGroupe, newPos, Quaternion.identity);
        currentInfoGridLayoutGroupe.transform.SetParent(worldCanvas,true);
        
        Transform gridTransform = currentInfoGridLayoutGroupe.transform;

        foreach (Character character in characters)
        {
            CreateInfoCharacter(character, pos, gridTransform);
        }
    }

    public void CreateInfoCharacter(Character character, Vector3 pos, Transform InfoGridLayoutGroupe) //[CODE PLACARD] (cf OnMouseEnter() in Spot.cs)
    {
        GameObject currentInfoCharacter = Instantiate(prefabInfoCharacter, pos, Quaternion.identity);
        InfoCharacter infoCharacter = currentInfoCharacter.GetComponent<InfoCharacter>();
        currentInfoCharacter.transform.SetParent(InfoGridLayoutGroupe);

        Sprite spr = character.gameObject.GetComponentInChildren<SpriteRenderer>().sprite;
        string shape = character.characterData.shape.ToString();
        int life = character.characterData.maxLife;
        //int dodgeValue = character.characterData.dodgeTime;

        //infoCharacter.SetInfoCharacter(spr, shape, life, dodgeValue);
    }

    public void DestroyInfoGridLayoutGroupe()
    {
        Destroy(currentInfoGridLayoutGroupe);
    }





    //
    //      OTHER
    //
    private void SeedRandom(int seed)
    {
        Random.InitState(seed);
    }

    public GameObject CreateCharacter(CharacterData characterData, GameObject spot)
    {
        GameObject newCharacter;
        if (characterData.name == "Player")
            newCharacter = Instantiate(prefabPlayer, spot.transform.position, Quaternion.identity);
        else
            newCharacter = Instantiate(prefabCharacter, spot.transform.position, Quaternion.identity);

        Character _chatacter = newCharacter.GetComponent<Character>();

        _chatacter.characterData = characterData;
        _chatacter.SetCurrentSpot(spot);
        Transform t_spot = spot.transform;
        _chatacter.SetTarget(new Vector3(t_spot.position.x, t_spot.position.y, t_spot.position.z));
        _chatacter.UpdateSmoothTime();
        
        characterList.Add(_chatacter);

        return newCharacter;
    }

    public MyObject CreateObjectOnMap(string nameObject)
    {
        ObjectData objData = ObjectManager.instance.FindObjectData(nameObject);
        MyObject obj = new MyObject(objData);
        return obj;


        
    }

    public Weapon CreateWeapon(string nameWeapon)
    {
        WeaponData wp = ObjectManager.instance.FindWeaponData(nameWeapon);
        Weapon w = new Weapon(wp);
        return w;
    }


    public void _ExecuteActionQueue()
    {
        StartCoroutine(ExecuteActionQueue());
    }
    //      -- METHODE IMPORTANTE --
    // Va executer toute les actions des characters
    // jusqu'a ce que player n'ai plus d'action dans sa stackAction
    //
    // [ANALYSE DE BUG RESOLU]
    // Quand on appele StartCoroutine(ExecuteActionQueue())
    // depuis un autre script, par exemple ButtonAction,
    // si le gameObject qui contient le script ButtonAction 
    // est detruit pendant un yield return
    // new WaitForSeconds de la methode : Alors la suite de la methode
    // ne va pas s'executer (je crois).
    // Du coup j'utilise le subterfuge ci-dessus...
    private IEnumerator ExecuteActionQueue()
    {
        inputBlock = true;
        AddTurn(1);

        // Application des effects
        List<int> effectToDelete = new List<int>();
        for (int i = 0; i < effectList.Count; i++)
        {
            if (effectList[i].Clock())
                effectToDelete.Add(i);
        }
        for (int i = 0; i < effectToDelete.Count; i++)
        {
            effectList.RemoveAt(effectToDelete[i]);
        }

        // Recuperation des actions de chaque character
        foreach (Character character in characterList) 
        {
            character.CancelReset();
            actionQueue.Add(character.PopAction());
        }

        // Execution des actions de chaque character
        for (int i = 0; i < GlobalConst.RANGE_PRIOTITY; i++)
        {
            foreach (Action action in actionQueue)
            {
                yield return new WaitUntil(() => CombatManager.instance.GetOnFight() == false);

                if (action.GetPriority() == i &&  !action.GetUser().isCanceled())
                {
                    action.PerfomAction();
                    yield return new WaitForSeconds(0.01f); //[CODE WARNING] Peut etre une source de bug (jsp)
                }
            }
        }

        foreach(Character character in characterList)
        {
            character.Metting();
        }

        yield return new WaitUntil(() => CombatManager.instance.GetOnFight() == false);        

        actionQueue.Clear();
        CommandPnj();

        //On fait les updates de UI uniquement si le player n'est pas sur un chemin
        if(!playerCharacter.GetOnPath())
        {
            UpdateUIButtonGrid(playerCharacter.GetCurrentButtonAction());
            UpdateUILandscape(playerCharacter.GetCurrentLocationType());
        }

        // Appele recursif de la fonction tant que la pile d'actions du player n'est pas vide
        if (playerCharacter.isStackActionEmpty() == false)
        {
            yield return new WaitForSeconds(GlobalConst.TIME_TURN_SEC);
            StartCoroutine(ExecuteActionQueue());
        }
        else
        {
            yield return new WaitForSeconds(GlobalConst.TIME_TURN_SEC);
            inputBlock = false;
        }
    }

    // Envoie la commande d'action de tout les pnj
    public void CommandPnj() //[CODE TMP] commande move pour l'instant ou bagare si il y a un enemy
    {
        foreach (Character character in characterList)
        {
            if(character.isPlayer() == false)
            {
                if(character.isDead == true)
                {
                    character.CommandEmpty();
                }
                else
                {
                    bool fightFound = false;
                    List<Character> charactersInSpot = character.GetAllCharactersInSpot();
                    foreach (Character _characterInSpot in charactersInSpot)
                    {
                        if(character.WantToFight(_characterInSpot))
                        {
                            fightFound = true;
                        }
                    } 

                    if(fightFound)
                    {
                        character.CommandFight();
                    }
                    else
                    {
                        List<GameObject> adjSpot = character.GetCurrentSpot().GetComponent<Spot>().GetAdjacentSpots();
                        int rng = Random.Range(0, adjSpot.Count);
                        character.CommandMove(adjSpot[rng]);
                    }
                }
            }
        }
    }


    //
    //      COMBAT
    //
    public void StartFight()
    {
        //Cancel toute les actions des character sur le spot
        List<Character> allCharactersInSpot = new List<Character>();
        List<Character> allCharactersAliveInSpot = new List<Character>();
        allCharactersInSpot = playerCharacter.GetAllCharactersInSpot();

        foreach (Character character in allCharactersInSpot)
        {
            if(character.isDead == false)
            {
                character.CancelAction();
                allCharactersAliveInSpot.Add(character);
            }
        }

        CombatManager.instance.SetUpFight(allCharactersAliveInSpot);

        //Visuel
        actionCanvas.SetActive(false);
        CombatManager.instance.ToggleFight();
        cam.GetComponent<CameraController>().ToggleCamPos();
    }

    public void QuitCombatScene()
    {
        actionCanvas.SetActive(true);
        CombatManager.instance.ToggleFight();
        cam.GetComponent<CameraController>().ToggleCamPos();

        CombatManager.instance.ClearCombatScene();
    }
}

public enum TileType {
    PRE_BUILD,
    LAND,
    WOOD,
    MOUNTAIN
}

public enum LocationType {
    EMPTY,
    LAND_HAMLET,
    FARMHOUSE,
    STONE,
    TOWER,
    LAC,
    ELF_HOUSE,
    WOOD_HAMLET,
    FLOWERS,
    DRUIDE_HOUSE,
    CAVE,
    MOUNTAIN_HAMLET,
    CIRCLE_STONES
}

public enum ButtonType { 
    TALK,
    REST,
    HIDE,
    UNHIDE,
    FIGHT,
    SEARCH
}

public enum Element {
    FIRE,
    LIGHTNING,
    WATER,
    WIND,
    METAL,
    WOOD,
    ROCK,
    FUR,
    PLUME,
    LEATHER,
    SKIN,
    DIVIN
}

public enum Divinity {
    EMPTY,
    SOLAR,
    WIZENED,
    ARSTAL,
    NATURE
}

public enum Range {
    CONTACT,
    PROJECTILE
}

public enum DamageType {
    SMASH,
    SHARP,
    ELEM
}

public enum WeaponStyle {
    LONG_SWORD
}

public enum SkillType {
    ATTACK,
    PARRY,
    SUMMON,
}

public enum ParryType {
    SIMPLE,
    COUNTER
}

public enum Relation {
    ALLY,       // Suive les deplacements et les combats
    FRIEND,     // Aide au combat sur la même case
    NEUTRAL,    // 
    HOSTIL,     // Attaque si on est sur la même case
    ENNMY       // Suive si on est pas trop loin
}

public enum Race {
    HUMAN,
    ELF,
    BEAST
}

public class RelationLine {
    public Race race_jugde;
    public Race race_eval;
    public Relation relation;

    public RelationLine(Race rj, Race re, Relation rel){
        race_jugde = rj;
        race_eval = re;
        relation = rel;
    }
}


public class GlobalConst {
    public static float OFF_SET_TILE = 10f;    // La taille entre les tuiles pour la créeation
    public static int SIZE_BOARD = 10;         // Le nombres de tuiles sur un coté lors de la création

    public static float TIME_TURN_SEC = 0.6f;    // Le temps de voir les actions se faire

    public static float BASIC_SMOOTHTIME = 0.4f;
    public static float HIDE_SMOOTHTIME = 0.8f;
    public static float DELTA_SMOOTH_CREW = 0.2f;   // Pour que les groupes se suive à la queue leu leu 


    // -- PRIORITE D'ACTION --
    // les actions suivent un ordre de priorité croissant
    public static int EMPTY_PRIORITY  = 1;
    public static int FIGHT_PRIORITY  = 2;
    public static int HIDE_PRIORITY   = 3;
    public static int MOVE_PRIORITY   = 4;
    public static int SEARCH_PRIORITY = 5;
    public static int REST_PRIORITY   = 6;

    public static int RANGE_PRIOTITY = 6; // Le nombre total de priority d'action

    // -- PRIOTITE D'EFFET --
    public static int ONPATH_PRIORITY = 1;
}
