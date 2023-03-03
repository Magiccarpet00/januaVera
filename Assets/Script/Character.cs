using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private GameObject currentSpot;
    //TMP
    private Vector3 target;
    private float smoothTime = 0.2F;
    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, smoothTime);
    }


    // TODO 
    // [CODI BUG]
    // Quand on clic sur la même case ça fait quand meme passer un tour, à réparer
    public virtual void Move(GameObject spot)
    {
        List<GameObject> adjSpot = currentSpot.GetComponent<Spot>().GetAdjacentSpots();

        //clic sur un spot adj
        if (adjSpot.Contains(spot))
        {
            currentSpot = spot;
            Transform t_spot = currentSpot.transform;
            target = new Vector3(t_spot.position.x, t_spot.position.y, t_spot.position.z);

            //Enlever les nuages
            Vector3 v = t_spot.parent.position;
            List<GameObject> tiles = GameManager.instance.GetTiles(((int)v.x) / 10 - 1, ((int)v.y / 10) - 1, ((int)v.x / 10) + 1, ((int)v.y / 10) + 1);
            foreach (GameObject tile in tiles)
            {
                Tile t = tile.GetComponentInChildren<Tile>();
                t.CleanCloud();
            }
        }
        else
        {
            //TODO faire le AStarMove quand on aura les pilles d'action
            AStarMove();
        }
        
    }

    private void AStarMove()
    {
        //TODO
    }






    //
    //      GET & SET
    //
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
        return currentSpot;
    }

    public void SetCurrentSpot(GameObject spot)
    {
        currentSpot = spot;
    }

    public void SetTarget(Vector3 t)
    {
        target = t;
    }

    public List<ButtonType> GetCurrentButtonAction()
    {
        return currentSpot.GetComponent<Spot>().GetActionInSpot();
    }



}
