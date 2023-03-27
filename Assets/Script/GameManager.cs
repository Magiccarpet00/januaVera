using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
    public GameObject prefabTmpEnemy;

    // GLOBAL VAR
    [Header("GLOBAL VAR")]
    public GameObject prefabPlayer;
    [HideInInspector] public GameObject player;
    [HideInInspector] public Player playerCharacter;

    public WorldBuilder worldBuilder;

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

    [SerializeField] private Sprite[] allImgLandscape_location;
    [HideInInspector] public Dictionary<LocationType, Sprite> dic_imgLandscape_location = new Dictionary<LocationType, Sprite>();

    [SerializeField] private GameObject btn_grid;
    [SerializeField] private GameObject[] allButton;
    [HideInInspector] public Dictionary<ButtonType, GameObject> dic_button = new Dictionary<ButtonType, GameObject>();
    private List<GameObject> currentButtons = new List<GameObject>();

    [SerializeField] private Transform worldCanvas;
    [SerializeField] private Vector3 offSetInfoGrid;
    [SerializeField] private GameObject prefabInfoGridLayoutGroupe;
    private GameObject currentInfoGridLayoutGroupe;


    public void Start()
    {
        SetUpUI();

        //--------SEED RNG---------
        //SeedRandom((int)Random.Range(0, 9999999));
        string seed = "Miriamo54";
        SeedRandom(seed.GetHashCode());
        //-------------------------

        worldBuilder.StartWorldBuilder();

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

        dic_imgLandscape_location.Add(LocationType.LAND_HAMLET,     allImgLandscape_location[0]);
        dic_imgLandscape_location.Add(LocationType.FARMHOUSE,       allImgLandscape_location[1]);
        dic_imgLandscape_location.Add(LocationType.STONE,           allImgLandscape_location[2]);
        dic_imgLandscape_location.Add(LocationType.TOWER,           allImgLandscape_location[3]);
        dic_imgLandscape_location.Add(LocationType.LAC,             allImgLandscape_location[4]);
        dic_imgLandscape_location.Add(LocationType.ELF_HOUSE,       allImgLandscape_location[5]);
        dic_imgLandscape_location.Add(LocationType.WOOD_HAMLET,     allImgLandscape_location[6]);
        dic_imgLandscape_location.Add(LocationType.FLOWERS,         allImgLandscape_location[7]);
        dic_imgLandscape_location.Add(LocationType.DRUIDE_HOUSE,    allImgLandscape_location[8]);
        dic_imgLandscape_location.Add(LocationType.CAVE,            allImgLandscape_location[9]);
        dic_imgLandscape_location.Add(LocationType.MOUNTAIN_HAMLET, allImgLandscape_location[10]);
        dic_imgLandscape_location.Add(LocationType.CIRCLE_STONES,   allImgLandscape_location[11]);

        dic_button.Add(ButtonType.TALK,   allButton[0]);
        dic_button.Add(ButtonType.HIDE,   allButton[1]);
        dic_button.Add(ButtonType.REST,   allButton[2]);
        dic_button.Add(ButtonType.UNHIDE, allButton[3]);
    }

    private void SetUpPlayer()
    {
        Vector2Int startLocation = new Vector2Int(1, 1);
        
        GameObject currentTile = GetTile(startLocation.x, startLocation.y);
        Spot[] spot = currentTile.GetComponentsInChildren<Spot>();
        player = CreateCharacter(prefabPlayer, spot[0].gameObject);
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





    //
    //      INPUT PLAYER
    //
    private void GetMouseInput()
    {
        // [CODE PROVISOIRE]
        if (Input.GetMouseButtonDown(0) && inputBlock==false)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity);
            foreach (RaycastHit2D h in hits)
            {
                GameObject gameObjectHit = h.transform.gameObject;

                if (gameObjectHit.CompareTag("Spot") && playerCharacter.isValideMove(gameObjectHit))
                {
                    playerCharacter.CommandMove(gameObjectHit);
                    _ExecuteActionQueue();
                }

                if (gameObjectHit.CompareTag("Location"))
                {
                    
                }

            }
        }


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

            GameObject newPnj = CreateCharacter(prefabTmpEnemy, spot[0].gameObject);
            newPnj.GetComponent<Character>().CommandEmpty(); // [CODE TMP]
        }

        if(Input.GetKeyDown(KeyCode.G)) //CREATE GROUPE 
        {
            GameObject currentTile = GetTile(2, 1);
            Spot[] spot = currentTile.GetComponentsInChildren<Spot>();

            GameObject newPnj1 = CreateCharacter(prefabTmpEnemy, spot[0].gameObject);
            GameObject newPnj2 = CreateCharacter(prefabTmpEnemy, spot[0].gameObject);
            GameObject newPnj3 = CreateCharacter(prefabTmpEnemy, spot[0].gameObject);

            List<Character> grp_char = new List<Character>();
            grp_char.Add(newPnj1.GetComponent<Character>());
            grp_char.Add(newPnj2.GetComponent<Character>());

            CreateGroupe(newPnj3.GetComponent<Character>(), grp_char);


            newPnj1.GetComponent<Character>().CommandEmpty(); // [CODE TMP]
            newPnj2.GetComponent<Character>().CommandEmpty(); // [CODE TMP]
            newPnj3.GetComponent<Character>().CommandEmpty(); // [CODE TMP]
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

        return actionButtonStandar;
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
        else
            imgLandscape.sprite = dic_imgLandscape_location[location];
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

    public void CreateInfoGridLayoutGroupe(Vector3 pos)
    {
        Vector3 newPos = Vector3.zero;
        newPos.x = offSetInfoGrid.x + pos.x;
        newPos.y = offSetInfoGrid.y + pos.y;

        currentInfoGridLayoutGroupe = Instantiate(prefabInfoGridLayoutGroupe, newPos, Quaternion.identity);
        currentInfoGridLayoutGroupe.transform.SetParent(worldCanvas,true);

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

    public GameObject CreateCharacter(GameObject character, GameObject spot)
    {
        GameObject newCharacter = Instantiate(character, spot.transform.position, Quaternion.identity);
        Character _chatacter = newCharacter.GetComponent<Character>();
        _chatacter.SetCurrentSpot(spot);
        Transform t_spot = spot.transform;
        _chatacter.SetTarget(new Vector3(t_spot.position.x, t_spot.position.y, t_spot.position.z));
        _chatacter.UpdateSmoothTime();
        characterList.Add(_chatacter);

        return newCharacter;
    }

    public void CreateGroupe(Character leader, List<Character> crew)
    {
        int i = 0;
        foreach (Character character in crew)
        {
            i++;
            character.SetLeader(leader);
            character.SetIdCrew(i);
            character.UpdateSmoothTime();
        }

        leader.SetCrew(crew);

    }




    public void _ExecuteActionQueue()
    {
        StartCoroutine(ExecuteActionQueue());
    }
    //      -- METHODE IMPORTANTE --
    // Va executer toute les actions des characters jusqu'a ce que player n'ai plus d'action dans sont stack
    //
    // [ANALYSE DE BUG RESOLU]
    // Quand on appele StartCoroutine(ExecuteActionQueue()) depuis un autre script, par exemple ButtonAction,
    // si le gameObject qui contient le script ButtonAction est detruit pendant un yield return
    // new WaitForSeconds de la methode : Alors la suite de la methode ne va pas s'executer (je crois).
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
            actionQueue.Add(character.PopAction());
        }

        // Execution des actions de chaque character
        foreach (Action action in actionQueue)  
        {
            //TODO faire les priorités
            action.PerfomAction();
        }

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
    public void CommandPnj() 
    {
        foreach (Character character in characterList)
        {
            if(character.isPlayer() == false && character.isLeader() == true)
            {
                List<GameObject> adjSpot = character.GetCurrentSpot().GetComponent<Spot>().GetAdjacentSpots();
                int rng = Random.Range(0, adjSpot.Count);
                character.CommandMove(adjSpot[rng]); //[CODE TMP] commande move pour l'instant
            }
        }
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
    UNHIDE
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

public enum DamageType {
    SMASH,
    SHARP,
    ELEM
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
    public static int EMPTY_PRIORITY = 1;
    public static int HIDE_PRIORITY  = 2;
    public static int MOVE_PRIORITY  = 3;
    public static int REST_PRIORITY  = 5;

    // -- PRIOTITE D'EFFET --
    public static int ONPATH_PRIORITY = 1;

    
}