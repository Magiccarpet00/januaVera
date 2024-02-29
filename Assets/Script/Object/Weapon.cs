
public class Weapon : MyObject
{
    public int currentState;

    public Weapon(WeaponData wd) : base(wd)
    {
        
    }

    public override bool isWeapon()
    {
        return true;
    }

}
