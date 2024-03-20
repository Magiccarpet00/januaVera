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
    public bool playerOnFight = false;

    public IEnumerator FightSequence()
    {
        if (playerOnFight) foreach (GameObject cs in PlayerCombatManager.instance.charactersSprites)
            cs.GetComponent<SpriteFight>().ResetSelected();

        if(playerOnFight) PlayerCombatManager.instance.timerFight.ActiveTimer(true);
        if(playerOnFight) PlayerCombatManager.instance.timerFight.ActiveTimer(true);

        while (speedInstant <= 6)
        {
            if(playerOnFight) PlayerCombatManager.instance.timerFight.SetTimer(speedInstant);

            CastSkills();
            CheckDying();

            if(playerOnFight) PlayerCombatManager.instance.UpdateAllUI();
            if(playerOnFight) yield return new WaitForSeconds(PlayerCombatManager.instance.TIME_FIGHT);

            speedInstant++;
        }
        speedInstant = 0;

        if(playerOnFight) PlayerCombatManager.instance.timerFight.ActiveTimer(false);
        if(playerOnFight) GameManager.instance.playerCharacter.selectedCharacters = new List<Character>();

        //TMP pour les parry
        foreach(Character character in characters)
        {
            character.nbGarde = 0;
        }

        if(!playerOnFight)
        {
            LoadSkillAI();
            if(!CheckFightEnd())
                StartCoroutine(FightSequence());
        }
    }


    public void CastSkills() //TODO fonction qui va toujours se remplire
    {
        List<Character> _characters = new List<Character>(); //[CODE BRISCAR] Astuce de vieux forban pour ajouter un element a characters pendant le foreach
        _characters.AddRange(characters);

        foreach (Character character in _characters)
        {
            if(character.currentLoadedSkill != null &&
               character.currentLoadedSkill.speed == speedInstant &&
               character.isDead == false &&
               character.selectedCharacters != null)
            {
                switch (character.currentLoadedSkill.skillType)
                {
                    case SkillType.ATTACK:
                        foreach (Character characterTarget in character.selectedCharacters)
                        {
                            SkillAttackData skillAttackData = (SkillAttackData)character.currentLoadedSkill;
                            if (!characterTarget.isDying)
                            {
                                if (characterTarget.ParryableAttack(skillAttackData))
                                {
                                    if (playerOnFight) PlayerCombatManager.instance.CreateFxFightSkill(PlayerCombatManager.instance.dic_CharacterSpriteFight[characterTarget].transform, characterTarget.currentLoadedSkill);

                                    if (characterTarget.currentLoadedSkill.skillType == SkillType.PARRY)
                                    {
                                        SkillParryData skillParryData = (SkillParryData)characterTarget.currentLoadedSkill;
                                        if(skillParryData.parryType == ParryType.COUNTER)
                                        {
                                            character.TakeDamage(skillParryData.damage, skillParryData.damageType, skillParryData.element);
                                            if (playerOnFight) PlayerCombatManager.instance.CreateFxFightSkill(PlayerCombatManager.instance.dic_CharacterSpriteFight[character].transform, characterTarget.currentLoadedSkill, true);
                                        }
                                    }
                                }
                                else
                                {
                                    characterTarget.TakeDamage(skillAttackData.damage, skillAttackData.damageType, skillAttackData.element);
                                    if (playerOnFight) PlayerCombatManager.instance.CreateFxFightSkill(PlayerCombatManager.instance.dic_CharacterSpriteFight[characterTarget].transform, character.currentLoadedSkill);
                                }
                                if (playerOnFight) PlayerCombatManager.instance.dic_CharacterSpriteFight[character].AnimAtk();
                            }
                        }
                        break;

                    case SkillType.PARRY:
                        foreach (Character characterTarget in character.selectedCharacters)
                        {
                            //TODO ne marche pas si on a plusieur type de parry en même temps
                            //exemple parrade classique + bouclier aquatique
                            SkillParryData skillParryData = (SkillParryData)character.currentLoadedSkill;
                            character.nbGarde = skillParryData.nbGarde;

                            if (playerOnFight) PlayerCombatManager.instance.CreateFxFightSkill(PlayerCombatManager.instance.dic_CharacterSpriteFight[characterTarget].transform, characterTarget.currentLoadedSkill);
                        }
                        break;

                    case SkillType.HEAL:
                        foreach (Character characterTarget in character.selectedCharacters)
                        {
                            SkillHealData skillHealData = (SkillHealData)character.currentLoadedSkill;
                            characterTarget.TakeHeal(skillHealData.amount);
                        }
                        break;

                    case SkillType.BUFF:
                        foreach (Character characterTarget in character.selectedCharacters)
                        {
                            SkillBuffData skillBuffData = (SkillBuffData)character.currentLoadedSkill;
                            //TODO BUFF
                        }
                        break;

                    case SkillType.SUMMON:
                        SkillSummonData skillSummonData = (SkillSummonData)character.currentLoadedSkill;
                        foreach (CharacterData characterToSummon in skillSummonData.characters)
                        {
                            GameObject characterSummoned = GameManager.instance.CreateCharacter(characterToSummon, character.GetCurrentSpot());
                            characters.Add(characterSummoned.GetComponent<Character>());
                            if (playerOnFight) PlayerCombatManager.instance.AddCharacterOnSpot(characterSummoned.GetComponent<Character>());
                        }
                        break;

                        
                }

                character.currentLoadedObject.UseObject();

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
                if (playerOnFight) PlayerCombatManager.instance.dic_CharacterSpriteFight[_character].AnimDie();
            }
        }
    }

    public void LoadSkillAI()
    {
        foreach (Character character in characters)
        {
            if (character.isPlayer() == false)
            {
                character.AI_SetRandomLoadedSkill(characters);
            }
        }
    }
    

    //SET UP
    public void ToggleFight(bool b)
    {
        onFight = b;
        //if (onFight == false) onFight = true;
        //else onFight = false;
    }

    public void SetUpFight(List<Character> _characters, bool playerInFight)
    {
        playerOnFight = playerInFight; //[CODE GRAMAIRE] bofbof le nommage...
        characters = _characters;
        LoadSkillAI();

        if (playerInFight)
        {
            PlayerCombatManager.instance.FillSpot();
            PlayerCombatManager.instance.SetUpPanel();
            PlayerCombatManager.instance.UpdateAllUI();
        }
        else
        {
            StartCoroutine(FightSequence());
        }
    }

    public bool CheckFightEnd()
    {
        bool fightEnd = true;

        foreach (Character characterWatching in characters)
            foreach (Character characterWatched in characters)
                if (characterWatching.WantToFight(characterWatched))
                    fightEnd = false;

        return fightEnd;
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