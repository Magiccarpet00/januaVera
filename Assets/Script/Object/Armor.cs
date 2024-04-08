
public class Armor : MyObject
{
    public Armor(ArmorData ad) : base(ad)
    {

    }

    public override bool isWeapon() {return false;}
    public override bool isArmor() { return true; }

}
