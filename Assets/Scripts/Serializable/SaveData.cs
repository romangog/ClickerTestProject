using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public int Balance;
    public List<BusinessData> BoughtBusinesses;

    public SaveData(BusinessesConfigs businessesConfigs)
    {
        BoughtBusinesses = new List<BusinessData>();

        // initialize New Game data

        BoughtBusinesses.Add(new BusinessData(
            1,
            0,
            businessesConfigs.BusinessesList[0].BaseCost,
            businessesConfigs.BusinessesList[0].BaseIncome,
            businessesConfigs.BusinessesList[0].Upgrades.Length));

        BoughtBusinesses.Add(new BusinessData(
            0,
            1,
            businessesConfigs.BusinessesList[1].BaseCost,
            businessesConfigs.BusinessesList[1].BaseIncome,
            businessesConfigs.BusinessesList[1].Upgrades.Length));

        Balance = 0;
    }
}
