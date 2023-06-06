using System;

public struct TrySpendMoneyRequest
{
    public int Price;
    public Action<bool> ResponseEvent;
}

