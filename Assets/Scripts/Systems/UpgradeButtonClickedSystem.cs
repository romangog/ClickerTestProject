using Leopotam.EcsLite;

sealed class UpgradeButtonClickedSystem : IEcsRunSystem, IEcsInitSystem
{
    private readonly BusinessesConfigs _configs;

    private EcsFilter _clicksFilter;
    private EcsPool<UpgradeBusinessButtonComponent> _upgradeViewsPool;
    private EcsPool<BusinessViewComponent> _businessViewPool;
    private EcsPool<UpdateViewEvent> _updateViewPool;
    private EcsPool<TrySpendMoneyRequest> _spendMoneyPool;
    private EcsPool<SaveEvent> _saveEventsPool;

    private EcsWorld _world;

    public UpgradeButtonClickedSystem(
        BusinessesConfigs configs)
    {
        _configs = configs;
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _clicksFilter = _world.Filter<ClickedEvent>().Inc<UpgradeBusinessButtonComponent>().End();
        _upgradeViewsPool = _world.GetPool<UpgradeBusinessButtonComponent>();
        _businessViewPool = _world.GetPool<BusinessViewComponent>();
        _updateViewPool = _world.GetPool<UpdateViewEvent>();
        _spendMoneyPool = _world.GetPool<TrySpendMoneyRequest>();
        _saveEventsPool = _world.GetPool<SaveEvent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _clicksFilter)
        {
            ref var upgradeView = ref _upgradeViewsPool.Get(entity);
            int upgradeViewId = upgradeView.Id;
            var businessEntity = upgradeView.BusinessEntity;
            ref var businessView = ref _businessViewPool.Get(businessEntity);
            var businessData = businessView.Data;

            if (businessData.UpgradesBought[upgradeViewId]) continue;

            var businessConfig = _configs.BusinessesList[businessData.Id];
            ref var spend = ref _spendMoneyPool.Add(_world.NewEntity());
            spend.Price = businessConfig.Upgrades[upgradeView.Id].Cost;
            spend.ResponseEvent =
                (success) =>
                {
                    if (!success) return;
                    businessData.UpgradesBought[upgradeViewId] = true;
                    _updateViewPool.Add(businessEntity);
                    _saveEventsPool.Add(_world.NewEntity());
                };
        }
    }
}


