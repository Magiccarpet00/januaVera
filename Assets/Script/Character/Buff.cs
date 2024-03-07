public class Buff //AND DEBUFF
{
    public Character character;
    public int nbRound; //Round -> Fight
    public int nbTurn;  //Turn  -> Map

    //BASIC
    public int mod_VITALITY, mod_ENDURANCE, mod_STRENGHT, mod_DEXTERITY, mod_FAITH,
               mod_SPEED, mod_DAMAGE;

    public Buff(Character c, int nbR, int nbT)
    {
        character = c;
        nbRound = nbR;
        nbTurn = nbT;
    }

    public void AddMod(Stats stats, int amount)
    {
        switch (stats)
        {
            case Stats.VITALITY:
                mod_VITALITY = amount;
                break;
            case Stats.ENDURANCE:
                mod_ENDURANCE = amount;
                break;
            case Stats.STRENGHT:
                mod_STRENGHT = amount;
                break;
            case Stats.DEXTERITY:
                mod_DEXTERITY = amount;
                break;
            case Stats.FAITH:
                mod_FAITH = amount;
                break;
            case Stats.SPEED:
                mod_SPEED = amount;
                break;
            case Stats.DAMAGE:
                mod_DAMAGE = amount;
                break;
        }
    }

    public void StartBuff() //[CODE FLEMARD] ya pas speed et damage
    {
        character.c_DEXTERITY += mod_DEXTERITY;
        character.c_ENDURANCE += mod_ENDURANCE;
        character.c_FAITH += mod_FAITH;
        character.c_STRENGHT += mod_STRENGHT;
        character.c_VITALITY += mod_VITALITY;
    }

    public void EndBuff() //[CODE FLEMARD] ya pas speed et damage
    {
        character.c_DEXTERITY -= mod_DEXTERITY;
        character.c_ENDURANCE -= mod_ENDURANCE;
        character.c_FAITH -= mod_FAITH;
        character.c_STRENGHT -= mod_STRENGHT;
        character.c_VITALITY -= mod_VITALITY;
    }

    public void ClockRound()
    {
        nbRound--;
    }

    public void ClockTurn()
    {
        nbTurn--;
    }
    
}