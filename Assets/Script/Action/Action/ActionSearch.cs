public class ActionSearch : Action
{
    public ActionSearch(Character u) : base(GlobalConst.SEARCH_PRIORITY, u) {; }
    public override void PerfomAction()
    {
        base.GetUser().Search();
    }
}