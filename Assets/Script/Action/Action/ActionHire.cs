public class ActionHire : Action
{
    public ActionHire(Character u) : base(GlobalConst.HIRE_PRIORITY, u) {; }
    public override void PerfomAction()
    {
        base.GetUser().Hire();
    }
}