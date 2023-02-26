using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameManager : MonoBehaviour
{
    //SINGLETON
    public static GameManager instance;
    void Awake()
    {
        instance = this;
    }

    // DEBUG
    public GameObject CircleTest;
    public Character characterTest;
    public WorldBuilder worldBuilder;

    public void Start()
    {
        //SEED RNG
        //SeedRandom((int)Random.Range(0, 9999999));
        SeedRandom(8);
        worldBuilder.StartWorldBuilder();
        StartCoroutine(ProtoMoveCharacter());
    }

    public void Update()
    {
        // [CODE PROVISOIRE]
        if(Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity);
            foreach (RaycastHit2D h in hits)
            {
                if (h.transform.gameObject.CompareTag("Spot"))
                {
                    int c = h.transform.gameObject.GetComponent<Spot>().GetAdjacentSpots().Count;
                    Debug.Log(c);
                }

                if (h.transform.gameObject.CompareTag("Location"))
                {
                    Debug.Log(h.transform.name);
                }

            }
        }
    }

    private IEnumerator ProtoMoveCharacter() //CODE ULTRA SPAGETI
    {
        GameObject go = GetTile(1, 1);
        Tile currentTile = go.GetComponentInChildren<Tile>();
        GameObject[] currentTileBorderSpot = currentTile.GetBorderSpots();
        foreach (GameObject spot in currentTileBorderSpot)
        {
            if(spot != null)
            {
                characterTest.Move(spot);
            }

        }
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
    LAND_STONE
}

public class GlobalVariable {
    public static float OFF_SET_TILE = 10f;    // La taille entre les tuiles pour la créeation
    public static int SIZE_BOARD = 10;         // Le nombres de tuiles sur un coté lors de la création
}