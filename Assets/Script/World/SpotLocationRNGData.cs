using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpotLocationRNGData", menuName = "JanuaVera/SpotLocationRNG Data")]
public class SpotLocationRNGData : ScriptableObject
{
    /*
        La structure de donn�e est plutot etrange elle fonctionnne
        en paralelle de la liste "allSpots" de la classe Tile

        Exemple :              |----------------------------------------------------|
        dropChance[0] = 50;    |                                                    |
        dropChance[1] = 40;    |  La somme de ce tableau dois tjr etre egale a 100  |
        dropChance[2] = 10;    |                                                    |
                               |----------------------------------------------------|

        locations[0] = EMPTY;
        locations[0] = LAND_HARVEST;
        locations[0] = LAND_RECLUT;
     */

    public int[] dropChance;
    public LocationData[] locations;

    //PS: locations[0] ne peut pas etre valide
}
