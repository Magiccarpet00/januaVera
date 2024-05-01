using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "JanuaVera/Character Data")]
public class CharacterData : ScriptableObject
{
    public Race race;
    public Element shape;
    public Divinity typeDivinity; //uniquement si le character est divin

    public int init_VITALITY, init_ENDURANCE, init_STRENGHT, init_DEXTERITY, init_FAITH, init_GOLD;
    public int drop_XP;

    public Sprite spriteMap;
    public Sprite spriteFight;

    public int workCost; //Si egale a 0 alors character non recrutable
    public bool isMerchant;
    public bool inLocation;

    public MoodAI currentMood;

    //OBJECT
    public List<ObjectData> objectInventory = new List<ObjectData>();
    public List<ArmorData> armorsEquiped = new List<ArmorData>();
    public List<ObjectData> objectToSell = new List<ObjectData>();
}
