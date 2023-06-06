public class UpgradeButtonView : EntityAddition<UpgradeBusinessButtonComponent>
{
    public void SetUpgradeId(int upgradeId)
    {
        _component.Id = upgradeId;
    }
    public void SetBusinessEntity(int businessEntity)
    {
        _component.BusinessEntity = businessEntity;
    }
}


