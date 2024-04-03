public class ActionTrade : Action
{
    public ActionTrade(Character u) : base(GlobalConst.TRADE_PRIORITY, u) {; }
    public override void PerfomAction()
    {
        base.GetUser().Trade();
    }
}