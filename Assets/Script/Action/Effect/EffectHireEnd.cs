
public class EffectHireEnd : Effect
{

    public EffectHireEnd(Character u, Character t, int nbTurnHire) : base(u, t, GlobalConst.ONPATH_PRIORITY, nbTurnHire) {;}

    public override void PerfomEffect()
    {
        target.followersCharacters.Remove(user);
        user.leaderCharacter = null;
    }
}
