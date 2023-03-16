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
    public GameObject CircleTest;
    public GameObject prefabTmpEnemy;

    // GLOBAL VAR
    public GameObject prefabPlayer;
    [HideInInspector] public GameObject player;

    public WorldBuilder worldBuilder;

    private int turn = 0;

    public List<Action> actionQueue = new List<Action>();
    public List<Character> listPnj = new List<Character>();
    


    // UI
    public Text nbTurn;
    public Image imgLandscape;
    [SerializeField] private Sprite[] allImgLandscape_tile;
    [HideInInspector] public Dictionary<TileType, Sprite> dic_imgLandscape_tile = new Dictionary<TileType, Sprite>();

    [SerializeField] private Sprite[] allImgLandscape_location;
    [HideInInspector] public Dictionary<LocationType, Sprite> dic_imgLandscape_location = new Dictionary<LocationType, Sprite>(); //TODO autre img a metre dans ce dico

    [SerializeField] private GameObject btn_grid;
    [SerializeField] private GameObject[] allButton;
    [HideInInspector] public Dictionary<ButtonType, GameObject> dic_button = new Dictionary<ButtonType, GameObject>();
    private List<GameObject> currentButtons = new List<GameObject>();


    public void Start()
    {
        SetUpUI();
        //SEED RNG
        //SeedRandom((int)Random.Range(0, 9999999));
        SeedRandom(8);

        worldBuilder.StartWorldBuilder();

        StartCoroutine(StartLateOne());

        //UI
        AddTurn(0);
    }

    public IEnumerator StartLateOne() //[CODE BIZARE] Cette methode est app�l� dans start mais elle s'execute apres tout les autre starts
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

        dic_button.Add(ButtonType.TALK, allButton[0]);
        dic_button.Add(ButtonType.HIDE, allButton[1]);
        dic_button.Add(ButtonType.REST, allButton[2]);
    }

    private void SetUpPlayer()
    {
        Vector2Int startLocation = new Vector2Int(1, 1);
        
        GameObject currentTile = GetTile(startLocation.x, startLocation.y);
        Spot[] spot = currentTile.GetComponentsInChildren<Spot>();
        player = CreateCharacter(prefabPlayer, spot[0].gameObject);
        UpdateUILandscape();
        UpdateUIButtonGrid(player.GetComponent<Character>().GetCurrentButtonAction());

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
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity);
            foreach (RaycastHit2D h in hits)
            {
                GameObject gameObjectHit = h.transform.gameObject;

                if (gameObjectHit.CompareTag("Spot"))
                {
                    //player.GetComponent<Character>().Move(gameObjectHit);
                    player.GetComponent<Character>().CommandMove(gameObjectHit);
                    ExecuteActionQueue();
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
            listPnj.Add(newPnj.GetComponent<Character>());
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
        actionButtonStandar.Add(ButtonType.HIDE);
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
    public void UpdateUILandscape() //Si on est sur spot vide
    {
        imgLandscape.sprite = dic_imgLandscape_tile[player.GetComponent<Character>().GetCurrentTileType()];
    }
    public void UpdateUILandscape(LocationType location) //Si on est sur spot avec location
    {
        imgLandscape.sprite = dic_imgLandscape_location[location];
    }
    public void UpdateUIButtonGrid(List<ButtonType> buttonType) //A chaque apelle de la methode on destroy tous les bouton est on les recrées
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

        return newCharacter;
    }

    public void ExecuteActionQueue() // Va executer toute les actions des characters
    {
        foreach (Action action in actionQueue)
        {
            action.PerfomAction();
        }

        actionQueue.Clear();
        CommandPnj();
    }

    public void CommandPnj() // Envoie la commande d'action de tout les pnj
    {
        foreach (Character pnj in listPnj)
        {
            List<GameObject> adjSpot =  pnj.GetCurrentSpot().GetComponent<Spot>().GetAdjacentSpots();
            int rng = Random.Range(0, adjSpot.Count);
            pnj.CommandMove(adjSpot[rng]);
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
    HIDE
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
}