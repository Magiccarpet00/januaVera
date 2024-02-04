using System.Collections.Generic;
using UnityEngine;


/*      
 *      La classe tile est dans le gameObjet enfant de parentTile
 *      si on veux avoir la main sur des élément tel que cloud
 *      il faudrais faire un getComponant en passant par le parent
 */
public class Tile : MonoBehaviour
{

    [SerializeField] private TileData tileData;
    /*
     DIRECTION ADJACENT 
     0 = UP
     1 = RIGHT
     2 = DOWN
     3 = LEFT
     */
    [SerializeField] private GameObject[] borderSpots = new GameObject[4];
    [SerializeField] public List<GameObject> allSpots = new List<GameObject>();

    // FX
    [SerializeField] private Animator animatorCloud;
    [SerializeField] private bool isDiscovered;

    void Start()
    {
        gameObject.name = GameManager.instance.CreateId();
        SetUpComponant();
        SetUpLocation();
    }





    //
    //      SETUP 
    //
    private void SetUpComponant()
    {
        animatorCloud = transform.parent.GetComponentInChildren<Animator>();
    }
    
    private void SetUpLocation() //Fait apparaitre les location dans les spots de la tile 
    {
        for (int i = 0; i < tileData.locationDatas.Length; i++)
        {
            if (tileData.locationDatas[i] != null)
            {
                SpotLocationRNGData locationData = tileData.locationDatas[i];

                int intervalUp, intervalDown;
                int rng = Random.Range(0, 100);
                int choice = 0;
                int nblocation = locationData.locations.Length;

                intervalDown = 0;
                intervalUp = locationData.dropChance[0];

                for (int j = 0; j < nblocation; j++)
                {
                    if (rng >= intervalDown && rng < intervalUp)
                    {
                        choice = j;
                    }

                    if (j < nblocation - 1)
                    {
                        intervalDown += locationData.dropChance[j];
                        intervalUp += locationData.dropChance[j + 1];
                    }
                    else if (intervalUp != 100)
                    {
                        Debug.LogError("ERROR RNG " + locationData);    // La somme des drop est != de 100
                    }
                }

                if (choice != 0)
                {
                    GameObject g = Instantiate(WorldBuilder.instance.locationPrefab, allSpots[i].transform.position, Quaternion.identity);
                    Location location = g.GetComponent<Location>();
                    location.locationData = locationData.locations[choice];
                    location.SetUpLocation();
                    g.transform.parent = allSpots[i].transform;
                }
            }
        }
    }





    //
    //      FX
    //
    public void CleanCloud()
    {
        animatorCloud.SetTrigger("triggerHide");
        isDiscovered = true;
    }





    //
    //      GET & SET
    //
    /*
     ATTENTION
     Quand on utilise cette fonction il faut faire attention a la corespondance des spots
     exemple : le borderSpot 0 de la tuile T1 est a coté du borderSpot 2 de la tuille T2

                ----    Border Spot
                |T2|        
                ----        0
                ----       3 1
                |T1|        2
                ----
     */
    public GameObject[] GetBorderSpots()
    {
        return borderSpots;
    }

    public GameObject GetBorderSpots(int dir)
    {
        int id = (dir + 2) % 4;
        return borderSpots[id];
    }

    /*
      Ce getteur spécial renvoie 0 si la tile est lisse sur son bord dir
      et 1 si elle a un spot adjacent
     */
    public int GetDirectionBorder(int dir)
    {
        if (borderSpots[dir] == null) return 0;
        else                          return 1;
    }

    public TileData GetTileData()
    {
        return tileData;
    }

    public bool GetIsDiscovered()
    {
        return isDiscovered;
    }

    public List<Spot> GetSpots()
    {
        List<Spot> spots = new List<Spot>();

        foreach (GameObject spot in allSpots)
        {
            spots.Add(spot.GetComponent<Spot>());
        }

        return spots;
    }
    
}
