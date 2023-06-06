using System.Collections.Generic;

public class PlayerData
{
    public int Balance
    {
        get
        {
            return _savedData.Balance;
        }
        set
        {
            _savedData.Balance = value;
        }
    }

    public List<BusinessData> BoughtBusinesses => _savedData.BoughtBusinesses;
    public SaveData SaveData => _savedData;

    private SaveData _savedData;

    public void CopyFrom(SaveData savedData)
    {
        _savedData = savedData;
    }
}
