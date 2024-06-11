using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;



public class InfoCharacter : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI lvl;


    public void SetInfoCharacter(Sprite spr, string _name, int _lvl, Color color)
    {
        image.sprite = spr;
        name.text = _name;
        lvl.text += " " + _lvl.ToString();

        name.color = color;
        lvl.color = color;
        
    }


}
