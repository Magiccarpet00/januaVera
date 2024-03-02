
public class Weapon : MyObject
{
    public int currentState;

    public Weapon(WeaponData wd) : base(wd)
    {
        currentState = wd.maxState;
    }

    public override bool isWeapon()
    {
        return true;
    }

}
