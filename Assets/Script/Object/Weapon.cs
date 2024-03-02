
public class Weapon : MyObject
{
    public int currentState;

    public Weapon(WeaponData wd) : base(wd)
    {
        currentState = wd.init_STATE;
    }

    public override void UseObject()
    {
        base.UseObject();
    }

    public override bool isWeapon()
    {
        return true;
    }

}
