
public class Armor : MyObject
{
    public int currentState;

    public Armor(ArmorData ad) : base(ad)
    {

    }

    public override bool isWeapon() {return false;}
    public override bool isArmor() { return true; }

}
