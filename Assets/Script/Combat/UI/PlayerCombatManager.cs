using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerCombatManager : MonoBehaviour
{
    public static PlayerCombatManager instance;
    private void Awake()
    {
        instance = this;
    }

    public List<GameObject> charactersSprites = new List<GameObject>();
    public Dictionary<Character, SpriteFight> dic_CharacterSpriteFight = new Dictionary<Character, SpriteFight>();

    public List<GameObject> combatSpots = new List<GameObject>();
    public Dictionary<Character, CombatSpot> dic_CharacterCombatSpot = new Dictionary<Character, CombatSpot>();

    [SerializeField] private Vector3 posFight;
    [SerializeField] private GameObject prefabSpriteCharacter;

    public float TIME_FIGHT = 0.5f;
    public TimerFight timerFight;
    public bool inputBlock;

    //Target (uniquement pour player)
    public bool inTargetMode;
    public int nbTarget;

    //Panel
    public GameObject CanvasFight;
    public GameObject uiPanel;
    public GameObject panelGlobal;
    public GameObject panelReadyWeapon;
    public GameObject panelWeapon;
    public GameObject gridWeapon;
    public GameObject panelSkill;
    public GameObject gridSkill;
    public GameObject helpSkill;
    public GameObject panelOrder;
    public GameObject panelHand;
    public TextMeshProUGUI textHandRight;
    public TextMeshProUGUI textHandLeft;
    public int handSelected; //0 = none     1 = right    2 = left;

    public GameObject buttonBack;
    public GameObject buttonEnd;

    public Stack<GameObject> panelStack = new Stack<GameObject>();

    public GameObject prefabButtonWeapon;
    public List<GameObject> buttonsWeapons = new List<GameObject>();

    public GameObject prefabButtonSkill;
    public List<GameObject> buttonsSkills = new List<GameObject>();
    public ButtonSkill buttonSkillBuffer;

    [Header("FX")]
    public GameObject prefabFxSkills;

    private int countSpot;
    public void FillSpot()
    {
        countSpot = 0;
        foreach (Character c in GameManager.instance.playerCharacter.currentCombatManager.characters)
        {
            AddCharacterOnSpot(c);
        }
    }

    private void Start()
    {
        foreach (GameObject cs in combatSpots)
            cs.GetComponent<CombatSpot>().SetActiveSpotUI(false);
    }

    private void Update()
    {
        //TMP
        if(GameManager.instance.playerCharacter != null)
            if(GameManager.instance.playerCharacter.isDead) inputBlock = true;
    }

    public void AddCharacterOnSpot(Character c)
    {
        // COMBAT SPOT
        CombatSpot cs = combatSpots[countSpot].GetComponent<CombatSpot>();
        cs.character = c;
        cs.SetActiveSpotUI(true);

        // CHARACTER SPRITE
        GameObject characterSprite = Instantiate(prefabSpriteCharacter, combatSpots[countSpot].transform.position, Quaternion.identity);
        characterSprite.GetComponent<SpriteFight>().SetCharacter(c);
        charactersSprites.Add(characterSprite);
        characterSprite.GetComponentInChildren<SpriteRenderer>().sprite = GameManager.instance.playerCharacter.currentCombatManager.characters[countSpot].characterData.spriteFight;

        // DICO
        dic_CharacterSpriteFight.Add(c, characterSprite.GetComponent<SpriteFight>());
        dic_CharacterCombatSpot.Add(c, cs);

        // TODO il faut faire en sort que quand on quitte un combat la scene sois "clean"
        countSpot++;
    }

    public void ClickButtonWeaponGlobal()
    {
        if (inputBlock) return;
       
        PushPanel(panelWeapon);

        foreach (Weapon weapon in GameManager.instance.playerCharacter.GetWeaponsInventory())
        {
            if(!GameManager.instance.playerCharacter.weaponHands.Contains(weapon))
            {
                GameObject btnWeapon = Instantiate(prefabButtonWeapon, transform.position, Quaternion.identity);
                btnWeapon.transform.SetParent(gridWeapon.transform);
                btnWeapon.transform.localScale = new Vector3(1, 1, 1);

                ButtonWeapon bw = btnWeapon.GetComponent<ButtonWeapon>();
                bw.activeWeapon = false;
                bw.SetUpUI(weapon);

                buttonsWeapons.Add(btnWeapon);
            }
        }

        
    }

    public void ClickButtonObjectGlobal()
    {
        if (inputBlock) return;

        PushPanel(panelSkill);
        foreach(ActiveObject activeObject in GameManager.instance.player.GetComponent<Character>().GetActiveObjectInventory())
        {
            if (activeObject.c_STATE > 0)
            {
                GameObject btnSkill = Instantiate(prefabButtonSkill, transform.position, Quaternion.identity);
                btnSkill.transform.SetParent(gridSkill.transform);
                btnSkill.transform.localScale = new Vector3(1, 1, 1);

                ButtonSkill bs = btnSkill.GetComponent<ButtonSkill>();
                bs.myObjectParent = activeObject;

                bs.SetUpUI(activeObject.activeObjectData.skillData);
                buttonsSkills.Add(btnSkill);
            }
        }

        helpSkill.SetActive(true);
    }

    public void ClickCommandGlobal()
    {
        if (inputBlock) return;
        PushPanel(panelOrder);
        TargetMode(10);
    }

    public void ClickButtonComandReset()
    {
        ResetAllSelected();
        PanelBack();
    }

    public void ClickButtonComandValide()
    {
        foreach (Character selectedCharacter in GameManager.instance.playerCharacter.selectedCharacters)
        {
            GameManager.instance.playerCharacter.charactersEncountered[selectedCharacter] = Relation.ENNEMY;
        }

        GameManager.instance.playerCharacter.SpreadRelation();

        foreach (Character followersCharacters in GameManager.instance.playerCharacter.followersCharacters)
        {
            followersCharacters.AI_SetRandomLoadedSkill(followersCharacters.currentCombatManager.characters);
        }
        UpdateAllUI();
        ResetAllSelected();
        PanelBack();
    }


    public void ClickButtonWeapon(Weapon weapon, bool activeWeapon)
    {
        if (inputBlock) return;

        PushPanel(panelSkill);
        WeaponData wd = (WeaponData)weapon.objectData;

        if(activeWeapon)
            GameManager.instance.playerCharacter.currentLoadedObject = weapon;
        else
            GameManager.instance.playerCharacter.selectedWeaponToEquip = weapon;

        foreach (SkillData skill in wd.skills)
        {
            GameObject btnSkill = Instantiate(prefabButtonSkill, transform.position, Quaternion.identity);
            btnSkill.transform.SetParent(gridSkill.transform);
            btnSkill.transform.localScale = new Vector3(1, 1, 1);

            ButtonSkill bs = btnSkill.GetComponent<ButtonSkill>();
            bs.myObjectParent = weapon;

            bs.SetUpUI(skill, activeWeapon);
            buttonsSkills.Add(btnSkill);
        }

        if(!activeWeapon)
        {
            panelHand.SetActive(true);
            if (GameManager.instance.playerCharacter.weaponHands[0] != null)
                textHandRight.text = GameManager.instance.playerCharacter.weaponHands[0].objectData.name;
            else
                textHandRight.text = "empty";

            if (GameManager.instance.playerCharacter.weaponHands[1] != null)
                textHandLeft.text = GameManager.instance.playerCharacter.weaponHands[1].objectData.name;
            else
                textHandLeft.text = "------";
        }
    }

    public void ClickButtonSkill(SkillData skillData, MyObject myObjectParent) //[CODE WARNING] peut etre une source de bug dans pour les skills 0target lancer par des PNJ
    {
        if (inputBlock) return;

        if(GameManager.instance.playerCharacter.currentLoadedSkill != null)
        {
            buttonSkillBuffer.templateAttack.GetComponent<ButtonSkillTemplateAttack>().ColorSwitch(false);
        }

        GameManager.instance.playerCharacter.currentLoadedObject = myObjectParent;
        GameManager.instance.playerCharacter.currentLoadedSkill = skillData;

        ResetAllSelected();

        if (skillData.nbTarget > 0) // Si nb target == 0 -> cible == lanceur
            TargetMode(skillData.nbTarget);
        else
            GameManager.instance.playerCharacter.selectedCharacters.Add(GameManager.instance.playerCharacter);

        UpdateEndButton();
    }

    public void ClickButtonBack()
    {
        if (inputBlock) return;
        handSelected = 0;
        panelHand.SetActive(false);
        ResetAllSelected();
        GameManager.instance.playerCharacter.currentLoadedSkill = null;
        PanelBack();
    }

    public void ClickButtonRightHand()
    {
        handSelected = 2;
        UpdateEndButton();
    }

    public void ClickButtonLeftHand()
    {
        handSelected = 1;
        UpdateEndButton();
    }

    public void ClickEndButton() //A REFACTOT
    {
        if (inputBlock) return;
        inputBlock = true;

        ClearButtonWeapon(); //TMP
        ClearButtonSkill();
        panelHand.SetActive(false);

        if (GameManager.instance.playerCharacter.selectedWeaponToEquip != null)
            GameManager.instance.playerCharacter.EquipWeapon(GameManager.instance.playerCharacter.selectedWeaponToEquip, handSelected);

        handSelected = 0;

        while (panelStack.Peek() != panelGlobal)
            PanelBack();

        GameManager.instance.cam_fight.GetComponent<Animator>().SetTrigger("fight");
        uiPanel.SetActive(false);
        StartCoroutine(GameManager.instance.playerCharacter.currentCombatManager.FightSequence());

    }

    public void ClickButtonEscape()
    {
        if (inputBlock) return;

        foreach (GameObject combatSpot in combatSpots)
        {
            CombatSpot _combatSpot = combatSpot.GetComponent<CombatSpot>();
            _combatSpot.UpdateEndRoundUI();
            _combatSpot.character = null;
        }
        dic_CharacterSpriteFight = new Dictionary<Character, SpriteFight>();
        GameManager.instance.QuitCombatScene();
    }


    public void PanelBack()
    {
        if (panelStack.Peek() == panelGlobal)
            return;

        if (panelStack.Peek() == panelWeapon)
            ClearButtonWeapon();

        if (panelStack.Peek() == panelSkill)
            ClearButtonSkill();

        PopPanel();

        if (panelStack.Peek() == panelGlobal)
            FillReadyToUseWeapons();
    }

    public void FillReadyToUseWeapons() //TODO A REFACTOT DEMAIN, BUG QUAND JE FAIT CHANGE WEAPON -> BACK
    {
        ClearButtonWeapon();
        if (GameManager.instance.playerCharacter.weaponHands[0] != null)
        {
            GameObject btnWeapon = Instantiate(prefabButtonWeapon, transform.position, Quaternion.identity);
            btnWeapon.transform.SetParent(panelReadyWeapon.transform);
            btnWeapon.transform.localScale = new Vector3(1, 1, 1); //[CODE BIZZARE] Je ne sais pas pourquoi je dois faire ce changement de scale

            ButtonWeapon bw = btnWeapon.GetComponent<ButtonWeapon>();
            bw.activeWeapon = true;
            bw.SetUpUI(GameManager.instance.playerCharacter.weaponHands[0]);

            buttonsWeapons.Add(btnWeapon);
        }

        if (GameManager.instance.playerCharacter.weaponHands[1] != null &&
           GameManager.instance.playerCharacter.weaponHands[1] != GameManager.instance.playerCharacter.weaponHands[0])
        {
            GameObject btnWeapon = Instantiate(prefabButtonWeapon, transform.position, Quaternion.identity);
            btnWeapon.transform.SetParent(panelReadyWeapon.transform);
            btnWeapon.transform.localScale = new Vector3(1, 1, 1); //[CODE BIZZARE] Je ne sais pas pourquoi je dois faire ce changement de scale

            ButtonWeapon bw = btnWeapon.GetComponent<ButtonWeapon>();
            bw.activeWeapon = true;
            bw.SetUpUI(GameManager.instance.playerCharacter.weaponHands[1]);

            buttonsWeapons.Add(btnWeapon);
        }


        
    }

    public void ClearButtonWeapon()
    {
        for (int i = 0; i < buttonsWeapons.Count; i++)
        {
            Destroy(buttonsWeapons[i]);
        }
    }

    public void ClearButtonSkill()
    {
        for (int i = 0; i < buttonsSkills.Count; i++)
        {
            Destroy(buttonsSkills[i]);
        }
    }

    public void UpdateAllUI()
    {
        GameManager.instance.UpdateTmpInfo();
        foreach (GameObject cs in combatSpots)
        {
            cs.GetComponent<CombatSpot>().UpdateUI();
        }
    }

    public void UpdateEndRoundAllUI()
    {
        foreach (GameObject cs in combatSpots)
        {
            cs.GetComponent<CombatSpot>().UpdateEndRoundUI();
        }
    }

    //------TARGET-------
    public void TargetMode(int _nbTarget)
    {
        nbTarget = _nbTarget;
        inTargetMode = true;
    }

    public void MinusTarget() // Decrementation du nombre de target
    {
        nbTarget--;
        if (nbTarget == 0)
            inTargetMode = false;

        UpdateEndButton();
    }

    public void ResetAllSelected()
    {
        foreach (GameObject cs in charactersSprites)
            cs.GetComponent<SpriteFight>().ResetSelected();

        GameManager.instance.playerCharacter.selectedCharacters = new List<Character>();
    }

    //STACK PANEL
    public void PushPanel(GameObject panel)
    {
        if (panelStack.Count != 0)
            panelStack.Peek().SetActive(false);
        panelStack.Push(panel);
        panel.SetActive(true);
        UpdateBackButton();
    }

    public void PopPanel()
    {
        panelStack.Pop().SetActive(false);
        panelStack.Peek().SetActive(true);
        UpdateBackButton();
        UpdateEndButton();
    }

    public void SetUpPanel()
    {
        CanvasFight.SetActive(true);
        panelGlobal.SetActive(false);
        panelWeapon.SetActive(false);
        panelSkill.SetActive(false);

        PushPanel(panelGlobal);
        FillReadyToUseWeapons();
        UpdateBackButton();
        UpdateEndButton();
    }

    public void UpdateBackButton()
    {
        if (panelStack.Peek() == panelGlobal)
            buttonBack.SetActive(false);
        else
            buttonBack.SetActive(true);
    }

    public void UpdateEndButton()
    {
        if (panelStack.Peek() == panelOrder)
            return;

        if (inputBlock)
        {
            buttonEnd.SetActive(false);
            return;
        }

        if(handSelected != 0)
        {
            buttonEnd.SetActive(true);
            return;
        }


        if (GameManager.instance.playerCharacter.selectedCharacters.Count > 0)
            buttonEnd.SetActive(true);
        else
            buttonEnd.SetActive(false);
            
    }

    //
    //      FX
    //
    public void CreateFxFightSkill(Transform _transform, SkillData skillData, bool skillComingFromCaster = false)
    {
        float offSet = 1;
        GameObject newFxFightSkill = Instantiate(prefabFxSkills, _transform.position, Quaternion.identity);

        switch (skillData.skillType)
        {
            case SkillType.ATTACK:
                SkillAttackData skillAttackData = (SkillAttackData)skillData;

                if (skillAttackData.damageType == DamageType.ELEM)
                    newFxFightSkill.GetComponent<FxFightSkills>().TriggerFxFightSkill(skillAttackData.element.ToString());
                else
                    newFxFightSkill.GetComponent<FxFightSkills>().TriggerFxFightSkill(skillAttackData.damageType.ToString());

                newFxFightSkill.transform.Translate(Random.Range(-offSet, offSet), Random.Range(-offSet, offSet), 0);
                break;

            case SkillType.PARRY:
                //[CODE SEPTIQUE] Je suis pas sur de faire qu'une seul anim de block pour les couters
                //j'aimerai bien que le mur d'eau, la riposte et le block d'un bouclier est des anims
                //differentes
                SkillParryData skillParryData = (SkillParryData)skillData;

                newFxFightSkill.GetComponent<FxFightSkills>().TriggerFxFightSkill("BLOCK");
                if (skillComingFromCaster) // [CODE DOUTEUX] j'aime pas trop ce copier la ligne du dessus � revoir
                {
                    if (skillParryData.damageType == DamageType.ELEM)
                        newFxFightSkill.GetComponent<FxFightSkills>().TriggerFxFightSkill(skillData.element.ToString());
                    else
                        newFxFightSkill.GetComponent<FxFightSkills>().TriggerFxFightSkill(skillParryData.damageType.ToString());
                    newFxFightSkill.transform.Translate(Random.Range(-offSet, offSet), Random.Range(-offSet, offSet), 0);
                }
                break;

            case SkillType.SUMMON:
                break;
        }

    }

    public Vector3 GetPosFight()
    {
        return posFight;
    }
}
