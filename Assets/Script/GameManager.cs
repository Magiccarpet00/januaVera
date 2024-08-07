﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;

public class GameManager : MonoBehaviour
{
    /*-----------MEGA TODO---------
      |                           |
      | -Refactot Stats -> list   |
      | -Refactot armor           |
      | -Skill debuff/buff        |
      | -Dijkstra                 |
      |                           |
      | -Bug move meeting path    |
      -----------------------------*/

    public static GameManager instance;
    void Awake()
    {
        instance = this;
    }

    // DEBUG
    [Header("DEGUB")]
    public GameObject CircleTest;
    //public GameObject prefabTmpEnemy;
    public int id = 0;
    public bool toggle;
    public bool debugTeleport;
    public int AstarDeep;
    public string debugName;


    // GLOBAL VAR
    [Header("GLOBAL VAR")]
    public GameObject cam;
    public GameObject cam_fight;
    public GameObject prefabPlayer;
    public GameObject prefabCharacter;
    public GameObject prefabObjectOnMap;
    public GameObject prefabCombatManagerInstance;

    [HideInInspector] public GameObject player;
    public Player playerCharacter;

    public WorldBuilder worldBuilder;
    public LocaleSelector localeSelector;

    private int turn = 0;
    public bool inputBlock;

    public List<Action> actionQueue = new List<Action>(); // liste de chaque action de tout les characters du current turn
    public List<Character> characterList = new List<Character>(); // liste de tout les characters en jeu
    public List<Effect> effectList = new List<Effect>();

    // UI
    [Header("UI")]
    public Text nbTurn;
    public Image imgLandscape;
    [SerializeField] private Sprite[] allImgLandscape_tile;
    [HideInInspector] public Dictionary<TileType, Sprite> dic_imgLandscape_tile = new Dictionary<TileType, Sprite>();

    [SerializeField] private GameObject btn_grid;
    [SerializeField] private GameObject[] allButton;
    [HideInInspector] public Dictionary<ButtonType, GameObject> dic_button = new Dictionary<ButtonType, GameObject>();
    private List<GameObject> currentButtons = new List<GameObject>();

    [SerializeField] private Transform worldCanvas;
    [SerializeField] private GameObject actionCanvas;
    [SerializeField] private Vector3 offSetInfoGrid;
    [SerializeField] private GameObject prefabInfoGridLayoutGroupe;
    private GameObject currentInfoGridLayoutGroupe;
    [SerializeField] private GameObject prefabInfoCharacter;
    public TextMeshProUGUI tmpInfo;
    public TextMeshProUGUI tmpInfo2;

    //FX
    [Header("FX")]
    public GameObject prefabOverMouse_fx;


    //DIALOG
    public GameObject panelDialog;
    public TextMeshProUGUI dialogText;
    public bool dialogWindowOpened; //[CODE OBJECT] Les dialogues box pourais etre plus homogene
    public AnswerButton dialogAnswer;

    // NAME ENUM
    public Dictionary<WeaponStyle, string> dic_weaponStyle = new Dictionary<WeaponStyle, string>();
    public Dictionary<Element, string> dic_element = new Dictionary<Element, string>();

    //RACISME
    public Dictionary<(Race, Race), Relation> relationshipsBoard = new Dictionary<(Race, Race), Relation>();

    //POKEMON typeChart
    public Dictionary<(string, Element), float> typeChart = new Dictionary<(string, Element), float>();

    


    public void Start()
    {
        SetUpUI();
        SetUpDicEnum();
        SetUpRacisme();
        SetUpTypeChart();

        //--------SEED RNG---------
        Random.InitState((int)Random.Range(0, 9999999));

        //string seed = "Miriamo54";
        //Random.InitState(seed.GetHashCode());
        //-------------------------

        worldBuilder.StartWorldBuilder();
        localeSelector.ChangeLocale(0);

        StartCoroutine(StartLateOne());

        //UI
        AddTurn(0);
    }

    public IEnumerator StartLateOne() //[CODE BIZARE] Cette methode est appele dans start mais elle s'execute apres tout les autre starts
    {
        yield return new WaitForSeconds(1f);
        SetUpPlayer();
    }

    private void Update()
    {
        GetMouseInput();
    }

    //
    //      SETUP
    //
    private void SetUpUI()
    {
        dic_imgLandscape_tile.Add(TileType.LAND,     allImgLandscape_tile[0]);
        dic_imgLandscape_tile.Add(TileType.MOUNTAIN, allImgLandscape_tile[1]);
        dic_imgLandscape_tile.Add(TileType.WOOD,     allImgLandscape_tile[2]);

        dic_button.Add(ButtonType.TALK,   allButton[0]);
        dic_button.Add(ButtonType.HIDE,   allButton[1]);
        dic_button.Add(ButtonType.REST,   allButton[2]);
        dic_button.Add(ButtonType.UNHIDE, allButton[3]);
        dic_button.Add(ButtonType.FIGHT,  allButton[4]);
        dic_button.Add(ButtonType.SEARCH, allButton[5]);
        dic_button.Add(ButtonType.HIRE, allButton[6]);
        dic_button.Add(ButtonType.TRADE, allButton[7]);
        dic_button.Add(ButtonType.WAIT, allButton[8]);
    }

    private void SetUpDicEnum()
    {
        dic_weaponStyle.Add(WeaponStyle.LONG_SWORD, "Long sword");

        dic_element.Add(Element.DIVIN, "Divin");
        dic_element.Add(Element.FIRE, "Fire");
        dic_element.Add(Element.FUR, "Fur");
        dic_element.Add(Element.LEATHER, "Leather");
        dic_element.Add(Element.LIGHTNING, "Lightning");
        dic_element.Add(Element.METAL, "Metal");
        dic_element.Add(Element.PLUME, "Plume");
        dic_element.Add(Element.ROCK, "Rock");
        dic_element.Add(Element.SKIN, "Skin");
        dic_element.Add(Element.WATER, "Water");
        dic_element.Add(Element.WIND, "Wind");
        dic_element.Add(Element.WOOD, "Wood");
    }

    private void SetUpPlayer()
    {
        Vector2Int startLocation = new Vector2Int(5, 5);
        
        GameObject currentTile = GetTile(startLocation.x, startLocation.y);
        Spot[] spot = currentTile.GetComponentsInChildren<Spot>();
        player = CreateCharacter((CharacterData)ScriptableManager.instance.FindData("HumanPlayer"), spot[0].gameObject, true);
        playerCharacter = player.GetComponent<Player>();
        UpdateUILandscape();
        UpdateUIButtonGrid(playerCharacter.GetCurrentButtonAction());
        UpdateTmpInfo();

        //Clean les nuages a la main
        List<GameObject> nearTiles = GetTiles(startLocation.x - 1,
                                              startLocation.y - 1,
                                              startLocation.x + 1,
                                              startLocation.y + 1);

        foreach (GameObject tile in nearTiles)
        {
            tile.GetComponentInChildren<Tile>().CleanCloud();
        }


    }

    public void SetUpRacisme()
    {
        //HUMAIN
        relationshipsBoard.Add((Race.HUMAN, Race.HUMAN), Relation.FRIEND);
        relationshipsBoard.Add((Race.HUMAN, Race.ELF), Relation.NEUTRAL);
        relationshipsBoard.Add((Race.HUMAN, Race.BEAST), Relation.NEUTRAL);
        relationshipsBoard.Add((Race.HUMAN, Race.NEUTRAL), Relation.NEUTRAL);

        //ELF
        relationshipsBoard.Add((Race.ELF, Race.HUMAN), Relation.NEUTRAL);
        relationshipsBoard.Add((Race.ELF, Race.ELF), Relation.FRIEND);
        relationshipsBoard.Add((Race.ELF, Race.BEAST), Relation.FRIEND);
        relationshipsBoard.Add((Race.ELF, Race.NEUTRAL), Relation.NEUTRAL);

        //BEAST
        relationshipsBoard.Add((Race.BEAST, Race.HUMAN), Relation.HOSTIL);
        relationshipsBoard.Add((Race.BEAST, Race.ELF), Relation.NEUTRAL);
        relationshipsBoard.Add((Race.BEAST, Race.BEAST), Relation.FRIEND);
        relationshipsBoard.Add((Race.BEAST, Race.NEUTRAL), Relation.NEUTRAL);

        //NEUTRAL
        relationshipsBoard.Add((Race.NEUTRAL, Race.HUMAN), Relation.NEUTRAL);
        relationshipsBoard.Add((Race.NEUTRAL, Race.ELF), Relation.NEUTRAL);
        relationshipsBoard.Add((Race.NEUTRAL, Race.BEAST), Relation.NEUTRAL);
        relationshipsBoard.Add((Race.NEUTRAL, Race.NEUTRAL), Relation.NEUTRAL);
    }

    public void SetUpTypeChart() //TODO A COMPLETER
    {

        //POKEMON CHART
        

        typeChart.Add((DamageType.SHARP.ToString(),Element.SKIN), 1f);
        typeChart.Add((DamageType.SHARP.ToString(), Element.FUR), 1f);
        typeChart.Add((DamageType.SHARP.ToString(), Element.PLUME), 1f);
        typeChart.Add((DamageType.SHARP.ToString(), Element.METAL), 0.25f);
        typeChart.Add((DamageType.SHARP.ToString(), Element.WOOD), 0.5f);
        typeChart.Add((DamageType.SHARP.ToString(), Element.ROCK), 0.25f);
        typeChart.Add((DamageType.SHARP.ToString(), Element.LEATHER), 0.5f);
        typeChart.Add((DamageType.SHARP.ToString(), Element.WATER), 0f);
        typeChart.Add((DamageType.SHARP.ToString(), Element.FIRE), 0f);
        typeChart.Add((DamageType.SHARP.ToString(), Element.WIND), 0f);
        typeChart.Add((DamageType.SHARP.ToString(), Element.LIGHTNING), 0f);

        typeChart.Add((DamageType.SMASH.ToString(), Element.SKIN), 1f);
        typeChart.Add((DamageType.SMASH.ToString(), Element.FUR), 1f);
        typeChart.Add((DamageType.SMASH.ToString(), Element.PLUME), 1f);
        typeChart.Add((DamageType.SMASH.ToString(), Element.METAL), 1f);
        typeChart.Add((DamageType.SMASH.ToString(), Element.WOOD), 1f);
        typeChart.Add((DamageType.SMASH.ToString(), Element.ROCK), 1f);
        typeChart.Add((DamageType.SMASH.ToString(), Element.LEATHER), 1f);
        typeChart.Add((DamageType.SMASH.ToString(), Element.WATER), 1f);
        typeChart.Add((DamageType.SMASH.ToString(), Element.FIRE), 0f);
        typeChart.Add((DamageType.SMASH.ToString(), Element.WIND), 0f);
        typeChart.Add((DamageType.SMASH.ToString(), Element.LIGHTNING), 0f);

        //ELEM
        typeChart.Add((Element.FIRE.ToString(), Element.SKIN), 1f);
        typeChart.Add((Element.FIRE.ToString(), Element.FUR), 4f);
        typeChart.Add((Element.FIRE.ToString(), Element.PLUME), 4f);
        typeChart.Add((Element.FIRE.ToString(), Element.METAL), 1f);
        typeChart.Add((Element.FIRE.ToString(), Element.WOOD), 4f);
        typeChart.Add((Element.FIRE.ToString(), Element.ROCK), 1f);
        typeChart.Add((Element.FIRE.ToString(), Element.LEATHER), 1f);
        typeChart.Add((Element.FIRE.ToString(), Element.WATER), 0.25f);
        typeChart.Add((Element.FIRE.ToString(), Element.FIRE), 0f);
        typeChart.Add((Element.FIRE.ToString(), Element.WIND), 0f);
        typeChart.Add((Element.FIRE.ToString(), Element.LIGHTNING), 0f);

        typeChart.Add((Element.LIGHTNING.ToString(), Element.SKIN), 1f);
        typeChart.Add((Element.LIGHTNING.ToString(), Element.FUR), 1f);
        typeChart.Add((Element.LIGHTNING.ToString(), Element.PLUME), 1f);
        typeChart.Add((Element.LIGHTNING.ToString(), Element.METAL), 4f);
        typeChart.Add((Element.LIGHTNING.ToString(), Element.WOOD), 1f);
        typeChart.Add((Element.LIGHTNING.ToString(), Element.ROCK), 1f);
        typeChart.Add((Element.LIGHTNING.ToString(), Element.LEATHER), 1f);
        typeChart.Add((Element.LIGHTNING.ToString(), Element.WATER), 4f);
        typeChart.Add((Element.LIGHTNING.ToString(), Element.FIRE), 0f);
        typeChart.Add((Element.LIGHTNING.ToString(), Element.WIND), 0f);
        typeChart.Add((Element.LIGHTNING.ToString(), Element.LIGHTNING), 0f);
    }



    //
    //      INPUT PLAYER
    //
    private void GetMouseInput()
    {
        //CHEAT CODE
        if(Input.GetKeyDown(KeyCode.C)) //CLEAN CLOUD
        {
            List<GameObject> tiles = GetTiles(0, 0, GlobalConst.SIZE_BOARD, GlobalConst.SIZE_BOARD);

            foreach (GameObject item in tiles)
            {
                item.GetComponentInChildren<Tile>().CleanCloud();
            }
        }


        if (Input.GetKeyDown(KeyCode.P))
        {
            CameraController cc = cam.GetComponent<CameraController>();
            toggle = !toggle;
            cc.ToggleFreezeCam(toggle);
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentSceneName);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            for (int i = 0; i < 25; i++)
                playerCharacter.AddXp(1);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            if (debugTeleport)
                debugTeleport = false;
            else
                debugTeleport = true;

            playerCharacter.s_VITALITY += 100;
            playerCharacter.c_VITALITY += 100;
            playerCharacter.gold += 100;

        }
    }





    //
    //      GET & SET
    //
    public GameObject GetTile(int x, int y, bool debugDraw = false)
    {
        GameObject res = null;

        Vector2 pos = new Vector2(x * GlobalConst.OFF_SET_TILE, y * GlobalConst.OFF_SET_TILE);
        Collider2D[] target = Physics2D.OverlapCircleAll(pos, 1f);

        foreach (Collider2D item in target)
        {
            if (item.CompareTag("Tile"))
            {
                res = item.transform.gameObject;
                if (debugDraw) Instantiate(CircleTest, pos, Quaternion.identity);
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
    public List<GameObject> GetTiles(int x1, int y1, int x2, int y2, bool debugDraw = false) //TODO [REFACTOT AOUT]
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

    public List<ButtonType> GetStandarActionButton()
    {
        List<ButtonType> actionButtonStandar = new List<ButtonType>();

        actionButtonStandar.Add(ButtonType.FIGHT);
        actionButtonStandar.Add(ButtonType.WAIT);

        //actionButtonStandar.Add(ButtonType.REST);

        //if(playerCharacter.GetHide() == false)
        //    actionButtonStandar.Add(ButtonType.HIDE);
        //else
        //    actionButtonStandar.Add(ButtonType.UNHIDE);

        //actionButtonStandar.Add(ButtonType.SEARCH);
        //actionButtonStandar.Add(ButtonType.HIRE);
        //actionButtonStandar.Add(ButtonType.TRADE);

        return actionButtonStandar;
    }

    //
    //      UI
    //
    public void AddTurn(int amount)
    {
        turn += amount;
        nbTurn.text = turn.ToString();
    }

    public void UpdateUILandscape(LocationType location = LocationType.EMPTY)
    {
        if(location == LocationType.EMPTY)
            imgLandscape.sprite = dic_imgLandscape_tile[playerCharacter.GetCurrentTileType()];
    }

    //A chaque apelle de la methode on destroy tous les bouton est on les recrées
    public void UpdateUIButtonGrid(List<ButtonType> buttonType) 
    {
        foreach (GameObject btn in currentButtons)
        {
            Destroy(btn);
        }

        foreach (ButtonType btnType in buttonType)
        {
            GameObject newBtn = Instantiate(dic_button[btnType], btn_grid.transform);
            currentButtons.Add(newBtn);
        }
    }

    public void UpdateTmpInfo()
    {
        string info = "";
        info += "VIT  " + playerCharacter.c_VITALITY + "/" + playerCharacter.s_VITALITY + "\t" + "LVL " + playerCharacter.lvl + "\n";
        info += "END  " + playerCharacter.c_ENDURANCE + "/" + playerCharacter.s_ENDURANCE + "\t" + "XP  " + playerCharacter.xp + "\n";
        info += "GOLD " + playerCharacter.gold;

        tmpInfo.text = info;
        tmpInfo2.text = info;
    }

    public void CreateInfoGridLayoutGroupe(Vector3 pos, List<Character> characters)
    {
        Vector3 newPos = Vector3.zero;
        newPos.x = offSetInfoGrid.x + pos.x;
        newPos.y = offSetInfoGrid.y + pos.y;

        currentInfoGridLayoutGroupe = Instantiate(prefabInfoGridLayoutGroupe, newPos, Quaternion.identity);
        currentInfoGridLayoutGroupe.transform.SetParent(worldCanvas,true);
        
        Transform gridTransform = currentInfoGridLayoutGroupe.transform;

        foreach (Character character in characters)
            if(!character.isPlayer()) CreateInfoCharacter(character, pos, gridTransform);
    }

    public void CreateInfoCharacter(Character character, Vector3 pos, Transform InfoGridLayoutGroupe) //[CODE PLACARD] (cf OnMouseEnter() in Spot.cs)
    {
        GameObject currentInfoCharacter = Instantiate(prefabInfoCharacter, pos, Quaternion.identity);
        InfoCharacter infoCharacter = currentInfoCharacter.GetComponent<InfoCharacter>();
        currentInfoCharacter.transform.SetParent(InfoGridLayoutGroupe);
        Sprite spr = character.gameObject.GetComponentInChildren<SpriteRenderer>().sprite;


        //COLOR
        Color _color = new Color(1f,1f,1f);
        switch (relationshipsBoard[(character.characterData.race, playerCharacter.characterData.race)])
        {
            case Relation.ALLY:
                _color = new Color(0f, 1f, 0f);
                break;
            case Relation.FRIEND:
                _color = new Color(0f, 1f, 0f);
                break;
            case Relation.HOSTIL:
                _color = new Color(1f, 0f, 0f);
                break;
            case Relation.ENNEMY:
                _color = new Color(1f, 0f, 0f);
                break;
        }

        infoCharacter.SetInfoCharacter(spr, character.name, character.lvl, _color);
    }

    public void DestroyInfoGridLayoutGroupe()
    {
        Destroy(currentInfoGridLayoutGroupe);
    }


    public void OpenDialogWindow(string text)
    {
        dialogWindowOpened = true;
        panelDialog.SetActive(true);
        dialogText.text = text;
    }

    public void CloseDialogWindow()
    {
        panelDialog.SetActive(false);
        dialogWindowOpened = false;
    }




    //
    //      OTHER
    //

    public GameObject CreateCharacter(CharacterData characterData, GameObject spot, bool isPlayer=false)
    {
        GameObject newCharacter;
        if (isPlayer)
        {
            newCharacter = Instantiate(prefabPlayer, spot.transform.position, Quaternion.identity);
        }
        else
        {
            newCharacter = Instantiate(prefabCharacter, spot.transform.position, Quaternion.identity);
            newCharacter.name = characterData.name + CreateId();
        }

        Character _character = newCharacter.GetComponent<Character>();
        _character.SetUp(characterData, spot);

        foreach (var item in characterData.objectInventory)
            AddObjectToCharacter(item, InventoryType.INVENTORY, _character);

        foreach (var item in characterData.armorsEquiped)
            AddObjectToCharacter(item, InventoryType.ARMOR, _character);

        foreach (var item in characterData.objectToSell)
            AddObjectToCharacter(item, InventoryType.SELL, _character);

        _character.AutoEquip();

        characterList.Add(_character);

        return newCharacter;
    }

    //CREATEUR D'OBJECT [CODE OBJECT PAS SUR]
 
    public void AddObjectToCharacter(ObjectData item, InventoryType inventoryType, Character character)
    {
        switch (inventoryType)
        {
            case InventoryType.INVENTORY:
                character.objectInventory.Add(CreateObject(item));
                break;
            case InventoryType.ARMOR:
                character.armorsEquiped.Add((Armor)CreateObject(item));
                break;
            case InventoryType.SELL:
                character.objectToSell.Add(CreateObject(item));
                break;
        }
    }

    public MyObject CreateObject(ObjectData item)
    {
        switch (item.objectType)
        {
            case ObjectType.ACTIVE:
                return new ActiveObject((ActiveObjectData)item);
            case ObjectType.ARMOR:
                return new Armor((ArmorData)item);
            case ObjectType.OBJECT:
                return new MyObject(item);
            case ObjectType.WEAPON:
                return new Weapon((WeaponData)item);
        }
        return null;
    }

    public string CreateId()
    {
        id++;
        return "_" + id.ToString();
    }

    public void _ExecuteActionQueue()
    {
        StartCoroutine(ExecuteActionQueue());
    }
    //      -- METHODE IMPORTANTE --
    // Va executer toute les actions des characters
    // jusqu'a ce que player n'ai plus d'action dans sa stackAction
    //
    // [ANALYSE DE BUG RESOLU]
    // Quand on appele StartCoroutine(ExecuteActionQueue())
    // depuis un autre script, par exemple ButtonAction,
    // si le gameObject qui contient le script ButtonAction 
    // est detruit pendant un yield return
    // new WaitForSeconds de la methode : Alors la suite de la methode
    // ne va pas s'executer (je crois).
    // Du coup j'utilise le subterfuge ci-dessus...
    private IEnumerator ExecuteActionQueue()
    {
        inputBlock = true;
        AddTurn(1);
        float debugTime = Time.fixedTime;

        // Application des effects
        int j = 0;
        foreach (Effect effect in effectList)
            effect.Clock();
        while (j < effectList.Count)
        {
            if (effectList[j].toDelete)
                effectList.RemoveAt(j);
            else
                j++;
        }

        // Recuperation des actions de chaque character
        foreach (Character character in characterList) 
        {
            character.CancelReset();
            Action characterAction = character.PopAction();
            if(characterAction != null)
                actionQueue.Add(characterAction);
        }

        // Execution des actions de chaque character
        for (int i = 0; i < GlobalConst.RANGE_PRIOTITY; i++)
        {
            foreach (Action action in actionQueue)
            {
                //yield return new WaitUntil(() => CombatManager.instance.GetOnFight() == false);

                if (action.GetPriority() == i && !action.GetUser().isCanceled())
                {
                    action.PerfomAction();
                    yield return new WaitUntil(()=>dialogWindowOpened == false); //[CODE WARNING] dialogWindow sont toute les UI
                }
            }
        }

        foreach (Character character in characterList)
        {
            if(!character.isDead) character.MettingOnPath();
        }

        yield return new WaitUntil(() => playerCharacter.onFight == false);

        foreach (Character character in characterList)
        {
            if(!character.isDead) character.MettingOnSpot();
            if(!character.isPlayer() && !character.isDead) character.AI_MakeCrew();
        }

        QuestManager.instance.UpdateQuests();

        actionQueue.Clear();
        CommandPnj();

        //On fait les updates de UI uniquement si le player n'est pas sur un chemin
        if (!playerCharacter.GetOnPath())
        {
            UpdateUIButtonGrid(playerCharacter.GetCurrentButtonAction());
            UpdateUILandscape(playerCharacter.GetCurrentLocationType());
            UpdateTmpInfo();
        }

        // Appele recursif de la fonction tant que la pile d'actions du player n'est pas vide
        if (playerCharacter.isStackActionEmpty() == false)
        {
            yield return new WaitForSeconds(GlobalConst.TIME_TURN_SEC);
            StartCoroutine(ExecuteActionQueue());
        }
        else
        {
            yield return new WaitForSeconds(GlobalConst.TIME_TURN_SEC);
            inputBlock = false;
        }
    }

    // Envoie la commande d'action de tout les pnj
    public void CommandPnj() //[CODE TMP] commande move pour l'instant ou bagare si il y a un enemy
    {
        foreach (Character character in characterList)
        {
            if (!character.isPlayer())
                character.AI_Command();
        }
    }

    //
    //      COMBAT
    //
    public void _StartFight(List<Character> characters, Character initiator, bool fightOnPath = false)
    {
        StartCoroutine(StartFight(characters,initiator,fightOnPath));
    }

    public IEnumerator StartFight(List<Character> characters, Character initiator, bool fightOnPath= false)
    {
        bool playerIsGoingToFight = false;

        foreach (Character character in characters)
        {
            if (character.isPlayer())
                playerIsGoingToFight = true;

            character.CancelAction();
        }

        if (fightOnPath) 
            yield return new WaitForSeconds(0.3f);

        if (playerIsGoingToFight)
        {
            playerCharacter.onFight = true;
            ToggleMapSceneFightScene(true);
        }

        GameObject combatManagerInstance = Instantiate(prefabCombatManagerInstance, transform.position, Quaternion.identity);
        CombatManager _combatManagerInstance = combatManagerInstance.GetComponent<CombatManager>();

        _combatManagerInstance.SetUpFight(characters);

    
    }

    //ClearCombatScene a mettre dans PlayerCombatManager
    public void QuitCombatScene()
    {
        playerCharacter.onFight = false; //TMP
        ToggleMapSceneFightScene(false);
        playerCharacter.currentCombatManager.ClearCombatScene();
    }

    public void ToggleMapSceneFightScene(bool b) //[CODE ULTRA DOUTEUX] Peut etre une source de bug si il ya plus de 2 emplacements de camera
    {
        PlayerCombatManager.instance.CanvasFight.SetActive(b);
        actionCanvas.SetActive(!b);
        cam.GetComponent<CameraController>().ToggleCamPos(b);
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
    FARMHOUSE,
    STONE,
    TOWER,
    LAC,
    ELF_HOUSE,
    WOOD_HAMLET,
    FLOWERS,
    DRUIDE_HOUSE,
    CAVE,
    MOUNTAIN_HAMLET,
    CIRCLE_STONES
}

public enum ButtonType { 
    TALK,
    REST,
    HIDE,
    UNHIDE,
    FIGHT,
    SEARCH,
    HIRE,
    TRADE,
    WAIT
}

public enum Element {
    FIRE,
    LIGHTNING,
    WATER,
    WIND,
    METAL,
    WOOD,
    ROCK,
    FUR,
    PLUME,
    LEATHER,
    SKIN,
    DIVIN,

    PARENT,
    NONE
}

public enum Divinity {
    EMPTY,
    SOLAR,
    WIZENED,
    ARSTAL,
    NATURE
}

public enum DamageType {
    SMASH,
    SHARP,
    ELEM
}

public enum ObjectType {
    ACTIVE,
    ARMOR,
    OBJECT,
    WEAPON
}

public enum WeaponStyle {
    LONG_SWORD
}

public enum SkillType {
    ATTACK,
    PARRY,
    SUMMON,
    HEAL,
    BUFF
}

public enum ParryType {
    SIMPLE,
    COUNTER
}

public enum Relation {
    ALLY,       // Suive les deplacements et les combats
    FRIEND,     // Aide au combat sur la même case
    NEUTRAL,    // 
    HOSTIL,     // Attaque si on est sur la même case
    ENNEMY       // Suive si on est pas trop loin
}

public enum Race {
    HUMAN,
    ELF,
    BEAST,
    NEUTRAL
}

public enum Stats{
    VITALITY,
    ENDURANCE, 
    STRENGHT, 
    DEXTERITY, 
    FAITH,
    SPEED, 
    DAMAGE
}

public enum AnswerButton{
    WAIT,
    YES,
    NO,
    CLOSE,
    HIRE
}

public enum InventoryType{
    INVENTORY,
    ARMOR,
    SELL
}

public enum MoodAI
{
    STATIC,
    MOVING_AREA,
    CHASE
}

public class GlobalConst {
    public static float OFF_SET_TILE = 10f;    // La taille entre les tuiles pour la créeation
    public static int SIZE_BOARD = 10;         // Le nombres de tuiles sur un coté lors de la création

    public static float TIME_TURN_SEC = 0.3f;    // Le temps de voir les actions se faire

    public static float BASIC_SMOOTHTIME = 0.4f;
    public static float HIDE_SMOOTHTIME = 0.8f;
    public static float DELTA_SMOOTH_CREW = 0.2f;   // Pour que les groupes se suive à la queue leu leu 


    // -- PRIORITE D'ACTION --
    // les actions suivent un ordre de priorité croissant
    public static int EMPTY_PRIORITY  = 1;
    public static int FIGHT_PRIORITY  = 2;
    public static int HIDE_PRIORITY   = 3;
    public static int TRADE_PRIORITY  = 4;
    public static int HIRE_PRIORITY   = 4;
    public static int TALK_PRIORITY   = 4;
    public static int MOVE_PRIORITY   = 5;
    public static int SEARCH_PRIORITY = 6;
    public static int REST_PRIORITY   = 7;

    public static int RANGE_PRIOTITY = 7; // Le nombre total de priority d'action

    // -- PRIOTITE D'EFFET --
    public static int ONPATH_PRIORITY = 1;
}
