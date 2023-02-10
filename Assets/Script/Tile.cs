using System;
using System.Collections;
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
    [SerializeField] private Tile[] adjacentTiles = new Tile[4];    // Remplit dans unity en fonction de chaque sprite
    [SerializeField] private Spot[] borderSpots = new Spot[4];
    [SerializeField] private List<Spot> allSpots = new List<Spot>();
    

    // FX
    [SerializeField] private Animator animatorCloud;

    void Start()
    {
        SetUp();
        CleanCloud();
    }

    private void SetUp()
    {
        //Animator
        animatorCloud = transform.parent.GetComponentInChildren<Animator>();
    }

    private void CleanCloud()
    {
        animatorCloud.SetTrigger("triggerHide"); //[CODE PRUDENCE] peut etre à l'origine de BUG le 1
    }

    public Spot[] GetBorderSpots()
    {
        return borderSpots;
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

    
}
