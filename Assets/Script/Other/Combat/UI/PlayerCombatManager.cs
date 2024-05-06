using System.Collections;
using System.Collections.Generic;
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
    public GameObject panelGlobal;
    public GameObject panelWeapon;
    public GameObject panelSkill;

    public Stack<GameObject> panelStack = new Stack<GameObject>();

    public GameObject prefabButtonWeapon;
    public List<GameObject> buttonsWeapons = new List<GameObject>();

    public GameObject prefabButtonSkill;
    public List<GameObject> buttonsSkills = new List<GameObject>();

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
            GameObject btnWeapon = Instantiate(prefabButtonWeapon, transform.position, Quaternion.identity);
            btnWeapon.transform.SetParent(panelWeapon.transform);
            btnWeapon.transform.localScale = new Vector3(1, 1, 1); //[CODE BIZZARE] Je ne sais pas pourquoi je dois faire ce changement de scale

            ButtonWeapon bw = btnWeapon.GetComponent<ButtonWeapon>();
            bw.SetUpUI(weapon);

            buttonsWeapons.Add(btnWeapon);
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
                btnSkill.transform.SetParent(panelSkill.transform);
                btnSkill.transform.localScale = new Vector3(1, 1, 1);

                ButtonSkill bs = btnSkill.GetComponent<ButtonSkill>();
                bs.myObjectParent = activeObject;

                bs.SetUpUI(activeObject.activeObjectData.skillData);
                buttonsSkills.Add(btnSkill);
            }
        }
    }

    public void ClickButtonWeapon(Weapon weapon)
    {
        if (inputBlock) return;

        PushPanel(panelSkill);
        WeaponData wd = (WeaponData)weapon.objectData;

        GameManager.instance.playerCharacter.currentLoadedObject = weapon;

        foreach (SkillData skill in wd.skills)
        {
            GameObject btnSkill = Instantiate(prefabButtonSkill, transform.position, Quaternion.identity);
            btnSkill.transform.SetParent(panelSkill.transform);
            btnSkill.transform.localScale = new Vector3(1, 1, 1);

            ButtonSkill bs = btnSkill.GetComponent<ButtonSkill>();
            bs.myObjectParent = weapon;

            bs.SetUpUI(skill);
            buttonsSkills.Add(btnSkill);
        }
    }

    public void ClickButtonSkill(SkillData skillData, MyObject myObjectParent) //[CODE WARNING] peut etre une source de bug dans pour les skills 0target lancer par des PNJ
    {
        if (inputBlock) return;

        GameManager.instance.playerCharacter.currentLoadedObject = myObjectParent;
        GameManager.instance.playerCharacter.currentLoadedSkill = skillData;

        ResetAllSelected();


        if (skillData.nbTarget > 0) // Si nb target == 0 -> cible == lanceur
            TargetMode(skillData.nbTarget);
        else
            GameManager.instance.playerCharacter.selectedCharacters.Add(GameManager.instance.playerCharacter);
    }

    public void ClickButtonBack()
    {
        if (inputBlock) return;

        ResetAllSelected();
        GameManager.instance.playerCharacter.currentLoadedSkill = null;
        PanelBack();
    }

    public void ClickEndButton() //A REFACTOT
    {
        if (inputBlock) return;
        
        inputBlock = true;

        ClearButtonWeapon(); //TMP
        ClearButtonSkill();
        while (panelStack.Peek() != panelGlobal)
            PanelBack();
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
    }

    public void PopPanel()
    {
        panelStack.Pop().SetActive(false);
        panelStack.Peek().SetActive(true);
    }

    public void SetUpPanel()
    {
        CanvasFight.SetActive(true);
        panelGlobal.SetActive(false);
        panelWeapon.SetActive(false);
        panelSkill.SetActive(false);

        PushPanel(panelGlobal);
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
                if (skillComingFromCaster) // [CODE DOUTEUX] j'aime pas trop ce copier la ligne du dessus à revoir
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
