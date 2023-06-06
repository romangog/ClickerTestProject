using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct BusinessViewComponent
{
    [HideInInspector] public BusinessData Data;
    [HideInInspector] public int[] UpgradeButtonsEntities;
    public TMP_Text Name;
    public TMP_Text Lvl;
    public TMP_Text Income;
    public TMP_Text LvlUpPrice;
    public Slider IncomeDelaySlider;
    public Transform UpgradesGroup;
    public LvlUpButtonView LvlUpBusinessButtonComponent;
}

