using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewBusinessConfigsList", menuName = "BusinessConfigList", order = 0)]
public class BusinessesConfigs : ScriptableObject
{
    public BusinessConfig [] BusinessesList;
}

[Serializable]
public class BusinessConfig
{
    public string Name;
    public float IncomeDelay;
    public int BaseCost;
    public int BaseIncome;
    public BusinessUpgradesConfig[] Upgrades;
}

[Serializable]
public class BusinessUpgradesConfig
{
    public string UpgradeName;
    public int Cost;
    public int IncomeAddPercent;
}