using Leopotam.EcsLite;

sealed class UpdateBusinessReactiveViewSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsFilter _businessesFilter;
    private EcsPool<BusinessViewComponent> _businessViewPool;
    private EcsPool<UpgradeBusinessButtonComponent> _upgradesPool;
    public void Init(IEcsSystems systems)
    {
        EcsWorld world = systems.GetWorld();
        _businessesFilter = world.Filter<BusinessViewComponent>().Inc<UpdateViewEvent>().End();
        _businessViewPool = world.GetPool<BusinessViewComponent>();
        _upgradesPool = world.GetPool<UpgradeBusinessButtonComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _businessesFilter)
        {
            // Update all data on business view that changes only by player actions
            ref var businessView = ref _businessViewPool.Get(entity);
            businessView.Lvl.text = "LVL\n" + businessView.Data.Level;
            businessView.Income.text = "Доход\n" + businessView.Data.Income + "$";
            businessView.LvlUpPrice.text = "Цена: " + businessView.Data.NextLevelCost + "$";

            for (int i = 0; i < businessView.UpgradeButtonsEntities.Length; i++)
            {
                int upgradeEntity = businessView.UpgradeButtonsEntities[i];
                ref var upgradeView = ref _upgradesPool.Get(upgradeEntity);
                if (businessView.Data.UpgradesBought[upgradeView.Id])
                    upgradeView.CostLabel.text = "Куплено";
            }
        }
    }
}


