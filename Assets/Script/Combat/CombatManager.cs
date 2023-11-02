using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;
    void Awake()
    {
        instance = this;
    }

    //Global
    [SerializeField] private List<Character> characters = new List<Character>();
    private List<GameObject> charactersSprites = new List<GameObject>();
    public Dictionary<Character, SpriteFight> dic_CharacterSpriteFight = new Dictionary<Character, SpriteFight>();

    [SerializeField] private List<GameObject> combatSpots = new List<GameObject>();
    public Dictionary<Character, CombatSpot> dic_CharacterCombatSpot = new Dictionary<Character, CombatSpot>();

    private bool onFight;
    [SerializeField] private Vector3 posFight;
    [SerializeField] private GameObject prefabSpriteCharacter;

    //FIGHT SEQUENCE
    private float TIME_FIGHT = 0.2f;
    private int speedInstant = 0;
    public TimerFight timerFight;


    //Target (uniquement pour player)
    public bool inTargetMode;
    public int nbTarget;

    //Panel
    public GameObject panelGlobal;
    public GameObject panelWeapon;
    public GameObject panelSkill;

    public Stack<GameObject> panelStack = new Stack<GameObject>();

    public GameObject prefabButtonWeapon;
    public List<GameObject> buttonsWeapons = new List<GameObject>();

    public GameObject prefabButtonSkill;
    public List<GameObject> buttonsSkills = new List<GameObject>();


    public void FillSpot()
    {
        int countSpot = 0;
        foreach (Character c in characters)
        {
            // COMBAT SPOT
            CombatSpot cs = combatSpots[countSpot].GetComponent<CombatSpot>();
            cs.character = c;
            cs.SetActiveSpotUI(true);

            // CHARACTER SPRITE
            GameObject characterSprite = Instantiate(prefabSpriteCharacter, combatSpots[countSpot].transform.position, Quaternion.identity);
            characterSprite.GetComponent<SpriteFight>().SetCharacter(c);
            charactersSprites.Add(characterSprite);
            characterSprite.GetComponentInChildren<SpriteRenderer>().sprite = characters[countSpot].characterData.spriteFight;

            // DICO
            dic_CharacterSpriteFight.Add(c, characterSprite.GetComponent<SpriteFight>());
            dic_CharacterCombatSpot.Add(c, cs);


            // TODO il faut faire en sort que quand on quitte un combat la scene sois "clean"
            countSpot++;
        }
    }

    public void ClickButtonWeaponGlobal()
    { 
        PushPanel(panelWeapon);

        foreach (Weapon weapon in GameManager.instance.playerCharacter.weaponInventory)
        {
            GameObject btnWeapon = Instantiate(prefabButtonWeapon, transform.position, Quaternion.identity);
            btnWeapon.transform.SetParent(panelWeapon.transform);
            btnWeapon.transform.localScale = new Vector3(1, 1, 1); //[CODE BIZZARE] Je ne sais pas pourquoi je dois faire ce changement de scale

            ButtonWeapon bw = btnWeapon.GetComponent<ButtonWeapon>();
            bw.SetUpUI(weapon);
            

            buttonsWeapons.Add(btnWeapon);
        }
    }

    public void ClickButtonWeapon(Weapon weapon)
    {
        PushPanel(panelSkill);

        foreach (SkillData skill in weapon.weaponData.skills)
        {
            GameObject btnSkill = Instantiate(prefabButtonSkill, transform.position, Quaternion.identity);
            btnSkill.transform.SetParent(panelSkill.transform);
            btnSkill.transform.localScale = new Vector3(1, 1, 1);

            ButtonSkill bs = btnSkill.GetComponent<ButtonSkill>();
            bs.skillData = skill;
            bs.btnName.text = skill.name;
            bs.btnText.text = skill.damage.ToString();

            buttonsSkills.Add(btnSkill);
        }
    }

    public void ClickButtonSkill(SkillData skillData)
    {
        GameManager.instance.playerCharacter.currentLoadedSkill = skillData;        
        TargetMode(skillData.nbTarget);
    }

    public void ClickButtonBack()
    {
        PanelBack();
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

    public void LoadSkill()
    {
        //currentPlayerSkill.owner = GameManager.instance.playerCharacter;
        //currentPlayerSkill.targets.AddRange(targetedCharacter);

        //GameManager.instance.playerCharacter.selectedCharacter
    }

    public void CastSkills()
    {
        foreach (Character character in characters)
        {
            if(character.currentLoadedSkill.speed == speedInstant && character.isDead == false)
            {
                switch (character.currentLoadedSkill.skillType)
                {
                    case SkillType.ATTACK:
                        foreach (Character characterTarget in character.selectedCharacter)
                        {
                            characterTarget.TakeDamage(character.currentLoadedSkill.damage);
                            dic_CharacterSpriteFight[character].AnimAtk();
                        }
                        break;

                    default:
                        break;
                }
            }


            //On verrifer les morts
            foreach (Character _character in characters)
            {
                if (_character.isDying == true && _character.isDead == false)
                {
                    _character.Die();
                    dic_CharacterSpriteFight[_character].AnimDie();
                }
            }


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

    public void ClickButtonEscape()
    {
        GameManager.instance.QuitCombatScene();
    }


    public void LoadSkillAI()
    {
        foreach (Character character in characters)
        {
            if (character.isPlayer() == false)
            {
                List<Character> tmp = new List<Character>();
                tmp.Add(GameManager.instance.playerCharacter);
                character.SetRandomLoadedSkill(tmp);
            }
        }
    }

    public void ClickEndButton() //A REFACTOT
    {
        ClearButtonWeapon(); //TMP
        ClearButtonSkill();
        while(panelStack.Peek() != panelGlobal)
        {
            PanelBack();
        }
        StartCoroutine(FightSequence());
    }

    public IEnumerator FightSequence()
    {

        foreach (GameObject cs in charactersSprites)
        {
            cs.GetComponent<SpriteFight>().ResetSelected();
        }

        timerFight.ActiveTimer(true);
        while (speedInstant != 6)
        {
            timerFight.SetTimer(speedInstant);
            CastSkills();
            UpdateAllUI();

            yield return new WaitForSeconds(TIME_FIGHT);
            speedInstant++;
            
        }
        speedInstant = 0;
        timerFight.ActiveTimer(false);

        GameManager.instance.playerCharacter.selectedCharacter = new List<Character>();
    }


    public void UpdateAllUI()
    {
        foreach(GameObject cs in combatSpots)
        {
            cs.GetComponent<CombatSpot>().UpdateUI();
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

    //STACK PANEL
    public void PushPanel(GameObject panel)
    {
        if(panelStack.Count != 0)
            panelStack.Peek().SetActive(false);
        panelStack.Push(panel);
        panel.SetActive(true);
    }

    public void PopPanel()
    {
        panelStack.Pop().SetActive(false);
        panelStack.Peek().SetActive(true);
    }



    //SET UP
    public void ToggleFight()
    {
        if (onFight == false) onFight = true;
        else onFight = false;
    }

    public void SetUpFight(List<Character> _characters)
    {

        characters = _characters;
        FillSpot();
        SetUpPanel();
        LoadSkillAI();
        UpdateAllUI();
    }

    public void SetUpPanel()
    {
        panelGlobal.SetActive(false);
        panelWeapon.SetActive(false);
        panelSkill.SetActive(false);

        PushPanel(panelGlobal);
    }

    // [BUG RESOLUE]
    // Supprimer la list de character dans CombatManager supprime la list des characters dans spot
    // du coup je fait pas de clear mais je comprend pas pourquoi
    public void ClearCombatScene()
    {
        characters = new List<Character>();
        dic_CharacterSpriteFight = new Dictionary<Character, SpriteFight>();
        dic_CharacterCombatSpot = new Dictionary<Character, CombatSpot>();

        foreach (GameObject cs in charactersSprites)
        {
            Destroy(cs);
        }

        charactersSprites = new List<GameObject>();

        foreach (GameObject cs in combatSpots)
        {
            cs.GetComponent<CombatSpot>().SetActiveSpotUI(false);
        }

    }

    //
    //      GET && SET
    //
    public bool GetOnFight()
    {
        return onFight;
    }

    public Vector3 GetPosFight()
    {
        return posFight;
    }
}