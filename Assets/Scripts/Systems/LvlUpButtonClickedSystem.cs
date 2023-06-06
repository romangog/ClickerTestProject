using Leopotam.EcsLite;

// System for processing level up button clicks
sealed class LvlUpButtonClickedSystem : IEcsRunSystem, IEcsInitSystem
{
    private readonly BusinessesConfigs _configs;

    private EcsFilter _clicksFilter;
    private EcsPool<LvlUpBusinessButtonComponent> _lvlUpTagPool;
    private EcsPool<BusinessViewComponent> _businessViewPool;
    private EcsPool<UpdateViewEvent> _updateViewPool;
    private EcsPool<TrySpendMoneyRequest> _spendMoneyPool;
    private EcsPool<BusinessWorkingTag> _businessWorkingPool;
    private EcsPool<UnlockedNewBusinessEvent> _unlockedNewBusinessEventPool;
    private EcsWorld _world;

    public LvlUpButtonClickedSystem(
        BusinessesConfigs configs)
    {
        _configs = configs;
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _clicksFilter = _world.Filter<ClickedEvent>().Inc<LvlUpBusinessButtonComponent>().End();
        _lvlUpTagPool = _world.GetPool<LvlUpBusinessButtonComponent>();
        _businessViewPool = _world.GetPool<BusinessViewComponent>();
        _updateViewPool = _world.GetPool<UpdateViewEvent>();
        _spendMoneyPool = _world.GetPool<TrySpendMoneyRequest>();
        _businessWorkingPool = _world.GetPool<BusinessWorkingTag>();
        _unlockedNewBusinessEventPool = _world.GetPool<UnlockedNewBusinessEvent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _clicksFilter)
        {
            var businessEntity = _lvlUpTagPool.Get(entity).BusinessEntity;
            ref var businessView = ref _businessViewPool.Get(businessEntity);
            var businessData = businessView.Data;
            ref var spend = ref _spendMoneyPool.Add(_world.NewEntity());

            // Send purchase request, subscribe to oepration result
            spend.Price = businessData.NextLevelCost;
            spend.ResponseEvent =
                (success) =>
                {
                    // If player had enough money, level up
                    if (!success) return;
                    if (businessData.Level == 0)
                    {
                        // If business was at 0 level, activate it and fire an UnlockedNewBusiness event+
                        _unlockedNewBusinessEventPool.Add(_world.NewEntity());
                        _businessWorkingPool.Add(businessEntity);
                    }

                    // Update level and calculate next level cost, then update business panel view
                    businessData.Level++;
                    businessData.NextLevelCost = (businessData.Level + 1) * _configs.BusinessesList[businessData.Id].BaseCost;
                    _updateViewPool.Add(businessEntity);
                };
        }
    }
}


