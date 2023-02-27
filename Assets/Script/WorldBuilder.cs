using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    public static WorldBuilder instance;
    public void Awake()
    {
        instance = this;
    }

    [SerializeField] private GameObject[,] allTilesInGame = new GameObject[GlobalVariable.SIZE_BOARD, GlobalVariable.SIZE_BOARD];

    [SerializeField] private List<GameObject> prefabTiles = new List<GameObject>();
    [SerializeField] private GameObject prefabParentTile;

    private List<GameObject> prefabTilesLand = new List<GameObject>();
    private List<GameObject> prefabTilesWood = new List<GameObject>();
    private List<GameObject> prefabTilesMountain = new List<GameObject>();

    private TileType[,] protoWorld = new TileType[GlobalVariable.SIZE_BOARD, GlobalVariable.SIZE_BOARD]; // La map qui contient uniquement les types de tiles potentiels

    //LOCATION
    public List<GameObject> prefabLocations = new List<GameObject>(); //TODO a transformer en dico
    
    
    public void StartWorldBuilder()
    {
        SetUpListTiles();
        CreateProtoWorld();
        CreateWorld();
        MakeLink();
    }

    private void MakeLink()
    {
        for (int y = 0; y < GlobalVariable.SIZE_BOARD; y++)
        {
            for (int x = 0; x < GlobalVariable.SIZE_BOARD; x++)
            {
                GameObject currentGameObjetTile = GameManager.instance.GetTile(x, y);

                if (currentGameObjetTile != null)
                {
                    Tile currentTile = currentGameObjetTile.GetComponentInChildren<Tile>();
                    GameObject[] currentTileBorderSpot = currentTile.GetBorderSpots();

                    for (int i = 0; i < 4; i++)
                    {
                        int i_x = (int)Mathf.Sin(i * Mathf.PI / 2);
                        int i_y = (int)Mathf.Cos(i * Mathf.PI / 2);

                        if (currentTileBorderSpot[i] != null)
                        {
                            GameObject adjacentGameObjetTile = GameManager.instance.GetTile(x + i_x, y + i_y);
                            if (adjacentGameObjetTile == null)
                            {
                                Debug.Log("Bug : Monde ouvert");
                            }
                            else
                            {
                                Tile adjacentTile = adjacentGameObjetTile.GetComponentInChildren<Tile>();

                                if (adjacentTile != null)
                                    currentTileBorderSpot[i].GetComponent<Spot>().AddAdjacentSpots(adjacentTile.GetBorderSpots(i), false);
                            }
                        }
                    }
                }
            }
        }
    }

    private void SetUpListTiles()
    {
        foreach (GameObject tile in prefabTiles)
        {
            TileData tileData = tile.GetComponent<Tile>().GetTileData();
            switch (tileData.tileType)
            {
                case TileType.LAND:
                    prefabTilesLand.Add(tile);
                    break;
                case TileType.WOOD:
                    prefabTilesWood.Add(tile);
                    break;
                case TileType.MOUNTAIN:
                    prefabTilesMountain.Add(tile);
                    break;
                default:
                    break;
            }
        }
    }

    private void CreateWorld()
    {
        for (int y = 0; y < GlobalVariable.SIZE_BOARD; y++)
        {
            for (int x = 0; x < GlobalVariable.SIZE_BOARD; x++)
            {
                if (allTilesInGame[x, y] == null)
                {
                    TileToBuild(x, y, protoWorld[x, y]);
                }
            }
        }
    }

    private void TileToBuild(int x, int y, TileType protoTileType)
    {
        int[] infoBorder = new int[4];
        infoBorder = GetBorder(x, y);
        GameObject tileToBuild = null;


        //Commencer par chercher dans les tilles du protoworld
        switch (protoTileType)
        {
            case TileType.LAND:
                tileToBuild = RandomChoiceTile(prefabTilesLand, infoBorder);
                break;
            case TileType.WOOD:
                tileToBuild = RandomChoiceTile(prefabTilesWood, infoBorder);
                break;
            case TileType.MOUNTAIN:
                tileToBuild = RandomChoiceTile(prefabTilesMountain, infoBorder);
                break;
        }

        if (tileToBuild == null)
        {
            //Debug.Log("404 baby");

            tileToBuild = RandomChoiceTile(prefabTiles, infoBorder);
        }

        if (tileToBuild == null)
        {
            //Debug.Log("404");
        }
        else
        {
            //Debug.Log("create");
            Vector2 pos = new Vector2(x * GlobalVariable.OFF_SET_TILE, y * GlobalVariable.OFF_SET_TILE);

            GameObject newTile = Instantiate(tileToBuild, pos, Quaternion.identity);
            allTilesInGame[x, y] = tileToBuild;

            GameObject newParentTile = Instantiate(prefabParentTile, pos, Quaternion.identity);
            newTile.transform.parent = newParentTile.transform;
        }
    }

    /*
     Methode qui va choisir une tile et la placé 
     
     Si on a la bonne tile on la renvoie, sinon on appele recursivement l'algo en force
     brute jusqu'a ce quil trouve ou qu'il n'y est plus d'element de la liste.
     Dans ce cas on returne null
     */
    private GameObject RandomChoiceTile(List<GameObject> listPrefabTiles, int[] infoBorder)
    {
        if (listPrefabTiles.Count == 0) return null;

        List<GameObject> newListPrefabTiles = new List<GameObject>(listPrefabTiles);

        int rng = Random.Range(0, newListPrefabTiles.Count);

        Tile tile = newListPrefabTiles[rng].GetComponentInChildren<Tile>();

        bool valide = true;
        GameObject[] borderSpot = tile.GetBorderSpots();

        // On compare les deux tableaux pour savoir si la tuile correspond à nos attentes
        for (int i = 0; i < 4; i++)
        {
            if (infoBorder[i] == -1) valide = true;
            else if (infoBorder[i] == 0) if (borderSpot[i] == null) valide = true;
                else
                {
                    valide = false;
                    break;
                }
            else if (infoBorder[i] == 1) if (borderSpot[i] != null) valide = true;
                else
                {
                    valide = false;
                    break;
                }
        }


        if (valide)
        {
            return newListPrefabTiles[rng];
        }
        else
        {
            newListPrefabTiles.RemoveAt(rng);
            return RandomChoiceTile(newListPrefabTiles, infoBorder);
        }

    }

    /*
     DIRECTION ADJACENT 
     0 = UP
     1 = RIGHT
     2 = DOWN
     3 = LEFT


     INFO BORDER

      0 = coté lise
      1 = coté ouvert
     -1 = n'importe

     */
    private int[] GetBorder(int x, int y)
    {
        int[] infoBorder = new int[4];

        if (y == GlobalVariable.SIZE_BOARD - 1) infoBorder[0] = 0;
        else if (allTilesInGame[x, y + 1] == null) infoBorder[0] = -1;
        else infoBorder[0] = allTilesInGame[x, y + 1].GetComponentInChildren<Tile>().GetDirectionBorder(2);

        if (x == GlobalVariable.SIZE_BOARD - 1) infoBorder[1] = 0;
        else if (allTilesInGame[x + 1, y] == null) infoBorder[1] = -1;
        else infoBorder[1] = allTilesInGame[x + 1, y].GetComponentInChildren<Tile>().GetDirectionBorder(3);

        if (y == 0) infoBorder[2] = 0;
        else if (allTilesInGame[x, y - 1] == null) infoBorder[2] = -1;
        else infoBorder[2] = allTilesInGame[x, y - 1].GetComponentInChildren<Tile>().GetDirectionBorder(0);

        if (x == 0) infoBorder[3] = 0;
        else if (allTilesInGame[x - 1, y] == null) infoBorder[3] = -1;
        else infoBorder[3] = allTilesInGame[x - 1, y].GetComponentInChildren<Tile>().GetDirectionBorder(1);


        return infoBorder;
    }


    /*
     Foncrion recursive en brute force qui va crée le protoWorld 
     le proto world est un tableau 2 dimensions de type de tille dont on
     va essayer de suivre pour crée le world avec les vrais tiles
     */
    private void CreateProtoWorld()
    {
        protoWorld[2, 2] = TileType.LAND; //TODO a refactot pour que les tiles de départ change 

        for (int y = 0; y < GlobalVariable.SIZE_BOARD; y++)
        {
            for (int x = 0; x < GlobalVariable.SIZE_BOARD; x++)
            {
                if (protoWorld[x, y] == TileType.PRE_BUILD)
                {
                    protoWorld[x, y] = ProtoTileToBuild(x, y);
                }
            }
        }

        for (int y = 0; y < GlobalVariable.SIZE_BOARD; y++)
        {
            for (int x = 0; x < GlobalVariable.SIZE_BOARD; x++)
            {
                if (protoWorld[x, y] == TileType.PRE_BUILD)
                {
                    CreateProtoWorld();
                }
            }
        }
    }

    /*
     [CODE PANIQUE]
     Cette fonction ne marche que pour tuiles land, wood et mountain
     */
    private TileType ProtoTileToBuild(int x, int y)
    {

        //compter les types de tiles à coté de la current tile 
        int nbLand = 0;
        int nbWood = 0;
        int nbMountain = 0;

        //[CODE PANIQUE] ne pas ouvrir ;)
        if (x < GlobalVariable.SIZE_BOARD - 1)
        {
            switch (protoWorld[x + 1, y])
            {
                case TileType.PRE_BUILD:
                    break;
                case TileType.LAND:
                    nbLand++;
                    break;
                case TileType.WOOD:
                    nbWood++;
                    break;
                case TileType.MOUNTAIN:
                    nbMountain++;
                    break;
                default:
                    break;
            }
        }
        if (x > 0)
        {
            switch (protoWorld[x - 1, y])
            {
                case TileType.PRE_BUILD:
                    break;
                case TileType.LAND:
                    nbLand++;
                    break;
                case TileType.WOOD:
                    nbWood++;
                    break;
                case TileType.MOUNTAIN:
                    nbMountain++;
                    break;
                default:
                    break;
            }
        }
        if (y < GlobalVariable.SIZE_BOARD - 1)
        {
            switch (protoWorld[x, y + 1])
            {
                case TileType.PRE_BUILD:
                    break;
                case TileType.LAND:
                    nbLand++;
                    break;
                case TileType.WOOD:
                    nbWood++;
                    break;
                case TileType.MOUNTAIN:
                    nbMountain++;
                    break;
                default:
                    break;
            }
        }
        if (y > 0)
        {
            switch (protoWorld[x, y - 1])
            {
                case TileType.PRE_BUILD:
                    break;
                case TileType.LAND:
                    nbLand++;
                    break;
                case TileType.WOOD:
                    nbWood++;
                    break;
                case TileType.MOUNTAIN:
                    nbMountain++;
                    break;
                default:
                    break;
            }
        }


        if (nbLand == 0 && nbWood == 0 && nbMountain == 0) return TileType.PRE_BUILD;       // Si il n'y a pas de tile a coté on reconstruit un pre_build

        if (nbLand > nbWood && nbLand > nbMountain) return TileSpread(TileType.LAND);       // Sinon on build la tile en fonction de la tile majoritaire
        else if (nbWood > nbLand && nbWood > nbMountain) return TileSpread(TileType.WOOD);
        else if (nbMountain > nbWood && nbMountain > nbLand) return TileSpread(TileType.MOUNTAIN);
        else
        {
            int rng = Random.Range(0, 3);                                                   // Au final dans le cas d'une egalité on en prend un au hazard
            switch (rng)
            {
                case 0:
                    return TileType.LAND;
                case 1:
                    return TileType.LAND;
                case 2:
                    return TileType.LAND;
            }
        }

        return TileType.PRE_BUILD; // [CODE CALIN] pour ne pas faire hurler le compilateur

    }

    /*
     Methode qui renvoie une tile en fonction de la tile passer en parametre
     exemple : un tile de land a plus de chance de crée une tile land en n+1 que
               de  crée un tile wood

    [TODO REFACTOT 1] faire en sorte qu'il y n'est pas de valeur en dur dans le code
     */
    private TileType TileSpread(TileType currentTileType)
    {
        TileType res = TileType.PRE_BUILD;
        int rng = Random.Range(0, 100);

        switch (currentTileType)
        {
            case TileType.PRE_BUILD:
                Debug.LogError("BUG -> TileSpread");
                break;
            case TileType.LAND:
                if (rng > 0 && rng <= 60) res = TileType.LAND;
                if (rng > 60 && rng <= 90) res = TileType.WOOD;
                if (rng > 90 && rng <= 100) res = TileType.MOUNTAIN;
                break;
            case TileType.WOOD:
                if (rng > 0 && rng <= 20) res = TileType.LAND;
                if (rng > 20 && rng <= 80) res = TileType.WOOD;
                if (rng > 80 && rng <= 100) res = TileType.MOUNTAIN;
                break;
            case TileType.MOUNTAIN:
                if (rng > 0 && rng <= 30) res = TileType.LAND;
                if (rng > 30 && rng <= 60) res = TileType.WOOD;
                if (rng > 60 && rng <= 100) res = TileType.MOUNTAIN;
                break;
            default:
                break;
        }
        return res;
    }
}
