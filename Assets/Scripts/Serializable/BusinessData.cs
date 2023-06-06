using System;
using System.Collections.Generic;

[Serializable]
public class BusinessData
{
    public float BusinessTime;
    public int Level;
    public int Id;
    public int NextLevelCost;
    public float Income;
    public List<bool> UpgradesBought;

    public BusinessData(int level, int id, int nextLevelCost, int income, int upgradesLength)
    {
        BusinessTime = 0f;
        Level = level;
        Id = id;
        NextLevelCost = nextLevelCost;
        Income = income;
        UpgradesBought = new List<bool>(upgradesLength);
        for (int i = 0; i < upgradesLength; i++)
        {
            UpgradesBought.Add(false);
        }
    }
}