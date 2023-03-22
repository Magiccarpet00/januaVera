using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileData", menuName = "JanuaVera/Tile Data")]
public class TileData : ScriptableObject
{
    public TileType tileType;

    //MANA
    public float manaPoolYellow;
    public float manaPoolBlack;
    public float manaPoolPurple;
    public float manaPoolGreen;

    public SpotLocationRNGData[] locationDatas;

}
