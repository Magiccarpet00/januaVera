
public class EffectOnPath : Effect
{
    public EffectOnPath(Character u, Character t) : base(u, t, GlobalConst.ONPATH_PRIORITY, 1) {;}

    public override void PerfomEffect()
    {
        target.ResetOnPath();
    }
}
