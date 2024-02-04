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
    public List<Character> characters = new List<Character>();
    private bool onFight;
    private int speedInstant = 0;




    

    public void CastSkills() //TODO fonction qui va toujours se remplire
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
                            
                            if(!characterTarget.isDying)
                            {
                                if(characterTarget.ParryableAttack(character.currentLoadedSkill))
                                {
                                    PlayerCombatManager.instance.CreateFxFightSkill(PlayerCombatManager.instance.dic_CharacterSpriteFight[characterTarget].transform, characterTarget.currentLoadedSkill);
                                    
                                    if(characterTarget.currentLoadedSkill.parryType == ParryType.COUNTER)
                                    {
                                        character.TakeDamage(characterTarget.currentLoadedSkill.damage);
                                        PlayerCombatManager.instance.CreateFxFightSkill(PlayerCombatManager.instance.dic_CharacterSpriteFight[character].transform, characterTarget.currentLoadedSkill,true);
                                    }
                                }
                                else
                                {
                                    characterTarget.TakeDamage(character.currentLoadedSkill.damage);
                                    PlayerCombatManager.instance.CreateFxFightSkill(PlayerCombatManager.instance.dic_CharacterSpriteFight[characterTarget].transform, character.currentLoadedSkill);
                                }
                                PlayerCombatManager.instance.dic_CharacterSpriteFight[character].AnimAtk();
                            }
                        }
                        break;

                    case SkillType.PARRY:
                        foreach(Character characterTarget in character.selectedCharacter)
                        {
                            //TODO ne marche pas si on a plusieur type de parry en même temps
                            //exemple parrade classique + bouclier aquatique
                            character.nbGarde = character.currentLoadedSkill.nbGarde;
                        }
                        break;

                    default:
                        break;
                }
            }
        }
    }

    public void CheckDying()
    {
        foreach (Character _character in characters)
        {
            if (_character.isDying == true && _character.isDead == false)
            {
                _character.Die();
                PlayerCombatManager.instance.dic_CharacterSpriteFight[_character].AnimDie();
            }
        }
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


    public IEnumerator FightSequence()
    {
        foreach (GameObject cs in PlayerCombatManager.instance.charactersSprites)
        {
            cs.GetComponent<SpriteFight>().ResetSelected();
        }

        PlayerCombatManager.instance.timerFight.ActiveTimer(true);
        while (speedInstant != 6)
        {
            PlayerCombatManager.instance.timerFight.SetTimer(speedInstant);
            CastSkills();
            CheckDying();
            PlayerCombatManager.instance.UpdateAllUI();

            yield return new WaitForSeconds(PlayerCombatManager.instance.TIME_FIGHT);
            speedInstant++;
        }
        speedInstant = 0;
        PlayerCombatManager.instance.timerFight.ActiveTimer(false);

        GameManager.instance.playerCharacter.selectedCharacter = new List<Character>();

        //TMP pour les parry
        foreach (Character character in characters)
        {
            character.nbGarde = 0;
        }
    }


    //SET UP
    public void ToggleFight()
    {
        if (onFight == false) onFight = true;
        else onFight = false;
    }

    public void SetUpFight(List<Character> _characters, bool playerInFight)
    {
        Debug.Log("playerInFight:" + playerInFight);

        characters = _characters;
        LoadSkillAI();

        if (playerInFight)
        {
            PlayerCombatManager.instance.FillSpot();
            PlayerCombatManager.instance.SetUpPanel();
            PlayerCombatManager.instance.UpdateAllUI();
        }
    }



    // [BUG RESOLUE]
    // Supprimer la list de character dans CombatManager supprime la list des characters dans spot
    // du coup je fait pas de clear mais je comprend pas pourquoi
    public void ClearCombatScene()
    {
        characters = new List<Character>();
        PlayerCombatManager.instance.dic_CharacterSpriteFight = new Dictionary<Character, SpriteFight>();
        PlayerCombatManager.instance.dic_CharacterCombatSpot = new Dictionary<Character, CombatSpot>();

        foreach (GameObject cs in PlayerCombatManager.instance.charactersSprites)
        {
            Destroy(cs);
        }

        PlayerCombatManager.instance.charactersSprites = new List<GameObject>();

        foreach (GameObject cs in PlayerCombatManager.instance.combatSpots)
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

}