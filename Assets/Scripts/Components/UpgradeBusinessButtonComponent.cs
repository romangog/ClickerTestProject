using System;
using TMPro;

[Serializable]
public struct UpgradeBusinessButtonComponent
{
    public int Id;
    public int BusinessEntity;

    public TMP_Text NameLabel;
    public TMP_Text UpgradeEffectLabel;
    public TMP_Text CostLabel;
}

