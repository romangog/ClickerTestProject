using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct ClickedEvent
{
}

[Serializable]
public struct LvlUpBusinessButtonComponent
{
    [HideInInspector] public int BusinessEntity;
}

[Serializable]
public struct UpgradeBusinessButtonComponent
{
    public int Id;
    public int BusinessEntity;

    public TMP_Text NameLabel;
    public TMP_Text UpgradeEffectLabel;
    public TMP_Text CostLabel;
}

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

public struct BusinessIncomeEvent
{

}

public struct RecountIncomeNumberEvent
{

}



public struct InitializedTag
{
}

public struct UpdateViewEvent
{
}

public struct UnlockedNewBusinessEvent
{
}

public struct TrySpendMoneyRequest
{
    public int Price;
    public Action<bool> ResponseEvent;
}

public struct EarnMoneyEvent
{
    public float Earning;
}

public struct SaveEvent
{
}


[Serializable]
public struct PlayerBalanceViewComponent
{
    public TMP_Text BalanceLabel;
}

public struct MoneyChangedEvent
{
    public int Delta;
}

public struct BusinessIncomeModifier
{
    public float AddPercents;
}


[Serializable]
public struct BusinessesListComponent
{
    public Transform ContentParent;
}

public struct BusinessWorkingTag
{
}




public struct Timer
{
    public float TimeLeft;
    public bool IsOver;
    private float _setTime;

    public void Update()
    {
        if (IsOver) return;
        TimeLeft = Mathf.MoveTowards(TimeLeft, 0f, Time.deltaTime);
        if (TimeLeft == 0f)
            IsOver = true;
    }

    internal void Set(float time)
    {
        TimeLeft = time;
        _setTime = time;
        IsOver = false;
    }

    internal void Reset()
    {
        TimeLeft = _setTime;
        IsOver = false;
    }
}

