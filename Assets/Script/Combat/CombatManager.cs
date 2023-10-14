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

    [SerializeField] private List<Character> characters = new List<Character>();
    private List<GameObject> charactersSprites = new List<GameObject>();

    [SerializeField] private List<GameObject> combatSpots = new List<GameObject>();
    private bool onFight;
    [SerializeField] private Vector3 posFight;
    [SerializeField] private GameObject prefabSpriteCharacter;

    //Button
    [SerializeField] private GameObject buttonAtk;

    public bool inTargetMode;
    public List<Character> targetedCharacter = new List<Character>(); // La liste actuelle des characters selectionné
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

    //Skill
    public SkillData currentSkill;

    public void FillSpot()
    {
        int countSpot = 0;
        foreach (Character c in characters)
        {
            // COMBAT SPOT
            CombatSpot cs = combatSpots[countSpot].GetComponent<CombatSpot>();
            cs.character = c;
            cs.SetActiveLifeText(true);
            cs.UpdateLife();

            // CHARACTER SPRITE
            GameObject characterSprite = Instantiate(prefabSpriteCharacter, combatSpots[countSpot].transform.position, Quaternion.identity);
            characterSprite.GetComponent<SpriteFight>().SetCharacter(c);
            charactersSprites.Add(characterSprite);
            characterSprite.GetComponent<SpriteRenderer>().sprite = characters[countSpot].characterData.spriteFight;

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
            bw.weapon = weapon;
            bw.btnName.text = weapon.weaponData.name;
            bw.btnStyle.text = GameManager.instance.dic_weaponStyle[weapon.weaponData.style];
            bw.btnState.text = weapon.currentState.ToString() + "/" + weapon.weaponData.maxState.ToString();
            bw.btnMaterial.text = GameManager.instance.dic_element[weapon.weaponData.material];

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
        currentSkill = skillData;
        TargetMode(skillData.nbTarget);
    }

    public void CastSkill()
    {
        switch (currentSkill.skillType)
        {
            case SkillType.ATTACK:

                foreach (Character character in targetedCharacter)
                {
                    character.TakeDamage(currentSkill.damage);
                }

                break;
            default:
                break;
        }

        currentSkill = null;
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

    public void ClickEndButton() //A REFACTOT
    {
        CastSkill();
        UpdateAllUI();
        DeselecteTargetedCharacter();
        ClearButtonWeapon(); //TMP
        ClearButtonSkill();

        while(panelStack.Peek() != panelGlobal)
        {
            PopPanel();
        }
    }

    public void DeselecteTargetedCharacter() //TODO upgarde system 
    {
        foreach (GameObject cs in charactersSprites)
        {
            cs.GetComponent<SpriteFight>().ResetSelected();
        }
        targetedCharacter = new List<Character>();
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
        foreach (GameObject cs in charactersSprites)
        {
            Destroy(cs);
        }
            
        foreach(GameObject cs in combatSpots)
        {
            cs.GetComponent<CombatSpot>().SetActiveLifeText(false);
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