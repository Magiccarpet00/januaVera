public class ActionTalk : Action
{
    public ActionTalk(Character u) : base(GlobalConst.TALK_PRIORITY, u) {;}
    public override void PerfomAction()
    {
        base.GetUser().Talk();
    }
}
