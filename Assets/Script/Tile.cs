using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*      
 *      La classe tile est dans le gameObjet enfant de parentTile
 *      si on veux avoir la main sur des �l�ment tel que cloud
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
    [SerializeField] private List<GameObject> allSpots = new List<GameObject>();
    

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
        animatorCloud.SetTrigger("triggerHide"); //[CODE PRUDENCE] peut etre � l'origine de BUG le 1
    }

    /*
     Attention

     Quand on utilise cette fonction il faut faire attention a la corespondance des spots

     exemple : le borderSpot 0 de la tuile T1 est a cot� du borderSpot 2 de la tuille T2

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
      Ce getteur sp�cial renvoie 0 si la tile est lisse sur son bord dir
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
