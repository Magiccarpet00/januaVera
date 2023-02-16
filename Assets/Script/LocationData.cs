using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocationData", menuName = "JanuaVera/Location Data")]
public class LocationData : ScriptableObject
{
    /*
        La structure de donnée est plutot etrange elle fonctionnne
        en paralelle de la liste "allSpots" de la classe Tile

        Exemple :              |----------------------------------------------------|
        dropChance[0] = 50;    |                                                    |
        dropChance[1] = 40;    |  La somme de ce tableau dois tjr etre egale a 100  |
        dropChance[2] = 10;    |                                                    |
                               |----------------------------------------------------|

        locations[0] = LAND_STONE;
        locations[0] = LAND_HARVEST;
        locations[0] = LAND_RECLUT;
     */

    public int[] dropChance;
    public LocationType[] locations;
}
