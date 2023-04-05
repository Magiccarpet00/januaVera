using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;



public class InfoCharacter : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private LocalizeStringEvent shapeString;
    [SerializeField] private LocalizeStringEvent dodgeString;
    [SerializeField] private TextMeshProUGUI shapeValue;
    [SerializeField] private TextMeshProUGUI dodgeValue;


    public void SetInfoCharacter(Sprite spr, String shape, int _shapeValue, int _dodgeValue)
    {
        image.sprite = spr;
        shapeString.SetEntry(shape);
        shapeValue.text = _shapeValue.ToString();
        dodgeString.SetEntry("DODGE");
        dodgeValue.text = _dodgeValue.ToString();
    }


}
