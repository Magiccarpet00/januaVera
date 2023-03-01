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
    public Character characterTest;
    public WorldBuilder worldBuilder;

    private int turn = 0;

    // UI
    public Text nbTurn;
    public Image imgLandscape;
    [SerializeField] private Sprite[] allImgLandscape_tile;
    [HideInInspector] public Dictionary<TileType, Sprite> dic_imgLandscape_tile = new Dictionary<TileType, Sprite>();

    [SerializeField] private Sprite[] allImgLandscape_location;
    [HideInInspector] public Dictionary<LocationType, Sprite> dic_imgLandscape_location = new Dictionary<LocationType, Sprite>(); //TODO autre img a metre dans ce dico


    public void Start()
    {
        SetUpUI();
        //SEED RNG
        //SeedRandom((int)Random.Range(0, 9999999));
        SeedRandom(8);

        worldBuilder.StartWorldBuilder();

        //UI
        AddTurn(0);

        StartCoroutine(StartLateOne());
    }

    private void SetUpUI()
    {
        dic_imgLandscape_tile.Add(TileType.LAND,     allImgLandscape_tile[0]);
        dic_imgLandscape_tile.Add(TileType.MOUNTAIN, allImgLandscape_tile[1]);
        dic_imgLandscape_tile.Add(TileType.WOOD,     allImgLandscape_tile[2]);

        dic_imgLandscape_location.Add(LocationType.LAND_HAMLET, allImgLandscape_location[0]);
        dic_imgLandscape_location.Add(LocationType.FARMHOUSE, allImgLandscape_location[1]);
        dic_imgLandscape_location.Add(LocationType.STONE, allImgLandscape_location[2]);
        dic_imgLandscape_location.Add(LocationType.TOWER, allImgLandscape_location[3]);
        dic_imgLandscape_location.Add(LocationType.LAC, allImgLandscape_location[4]);
        dic_imgLandscape_location.Add(LocationType.ELF_HOUSE, allImgLandscape_location[5]);
        dic_imgLandscape_location.Add(LocationType.WOOD_HAMLET, allImgLandscape_location[6]);
        dic_imgLandscape_location.Add(LocationType.FLOWERS, allImgLandscape_location[7]);
        dic_imgLandscape_location.Add(LocationType.DRUIDE_HOUSE, allImgLandscape_location[8]);
        dic_imgLandscape_location.Add(LocationType.CAVE, allImgLandscape_location[9]);
        dic_imgLandscape_location.Add(LocationType.MOUNTAIN_HAMLET, allImgLandscape_location[10]);
        dic_imgLandscape_location.Add(LocationType.CIRCLE_STONES, allImgLandscape_location[11]);

    }

    public IEnumerator StartLateOne() //[CODE BIZARE] Cette methode est appélé dans start mais elle s'execute apres tout les autre starts
    {
        yield return new WaitForSeconds(1f);
        SetUpPlayer();       

    }

    public void Update()
    {
        GetMouseInput();
    }

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
                    characterTest.Move(gameObjectHit);
                }

                if (gameObjectHit.CompareTag("Location"))
                {
                    
                }

            }
        }
    }

    private void SetUpPlayer()
    {
        GameObject g = GetTile(1, 1);
        Tile currentTile = g.GetComponentInChildren<Tile>();
        GameObject[] currentTileBorderSpot = currentTile.GetBorderSpots();

        characterTest.SetCurrentSpot(currentTileBorderSpot[0]);
        currentTile.CleanCloud();

        Transform t_spot = currentTileBorderSpot[0].transform;
        characterTest.SetTarget(new Vector3(t_spot.position.x, t_spot.position.y, t_spot.position.z));

        UpdateUILandscape(false);
    }

    private IEnumerator ProtoMoveCharacter() //CODE ULTRA SPAGETI
    {
        for (int i = 0; i < 2000; i++)
        {
            yield return new WaitForSeconds(0.4f);
            GameObject gameObjectSpot = characterTest.GetCurrentSpot();
            Spot spot = gameObjectSpot.GetComponent<Spot>();

            List<GameObject> adjacentSpot = spot.GetAdjacentSpots();

            int rng = Random.Range(0, adjacentSpot.Count);

            characterTest.Move(adjacentSpot[rng]);

            //Enlever les nuages
            Vector3 v = gameObjectSpot.transform.parent.position;
            List<GameObject> tiles = GetTiles(((int)v.x)/10 - 1, ((int)v.y/10) - 1, ((int)v.x/10) + 1, ((int)v.y/10) +1);
            foreach (GameObject tile in tiles)
            {
                Tile t = tile.GetComponentInChildren<Tile>();
                t.CleanCloud();
            }
        }
    }

    public GameObject GetTile(int x, int y, bool debugDraw = false)
    {
        GameObject res = null;

        Vector2 pos = new Vector2(x * GlobalVariable.OFF_SET_TILE, y * GlobalVariable.OFF_SET_TILE);
        Collider2D[] target = Physics2D.OverlapCircleAll(pos, 1f);

        foreach (Collider2D item in target)
        {
            if (item.CompareTag("Tile"))
            {
                res = item.transform.gameObject;
                if(debugDraw) Instantiate(CircleTest, pos, Quaternion.identity);
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


    private void SeedRandom(int seed)
    {
        Random.InitState(seed);
    }











    // UI
    public void AddTurn(int amount)
    {
        turn += amount;
        nbTurn.text = turn.ToString();
    }

    public void UpdateUILandscape(bool inLocation) {
        
        if(inLocation)
            ;
        else
            imgLandscape.sprite = dic_imgLandscape_tile[characterTest.GetCurrentTileType()];
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

public class GlobalVariable {
    public static float OFF_SET_TILE = 10f;    // La taille entre les tuiles pour la créeation
    public static int SIZE_BOARD = 10;         // Le nombres de tuiles sur un coté lors de la création
}