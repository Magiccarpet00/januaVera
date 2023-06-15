using UnityEngine;

public class Member
{
    private MemberType memberType;
    private Element element;
    private Divinity divinity;

    private int maxHealth;
    private int currentHealth;

    private bool isVital;

    public Member(MemberType _memberType, Element _element, int _maxHealth, bool _isVital)
    {
        memberType = _memberType;
        element = _element;
        divinity = Divinity.EMPTY;
        maxHealth = _maxHealth;
        currentHealth = _maxHealth;
        isVital = _isVital;
    }

    public Member(MemberType _memberType, Divinity _divinity, int _maxHealth, bool _isVital)
    {
        memberType = _memberType;
        element = Element.DIVIN;
        divinity = _divinity;
        maxHealth = _maxHealth;
        currentHealth = _maxHealth;
        isVital = _isVital;
    }

    

}
