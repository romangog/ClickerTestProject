using Leopotam.EcsLite;

sealed class AccountBusinessIncomeSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _businessesFilter;
    private EcsPool<BusinessViewComponent> _businessesViewPool;
    private EcsPool<EarnMoneyEvent> _earnMoneyEventsPool;

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _businessesFilter = _world.Filter<BusinessIncomeEvent>().End();
        _businessesViewPool = _world.GetPool<BusinessViewComponent>();
        _earnMoneyEventsPool = _world.GetPool<EarnMoneyEvent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _businessesFilter)
        {
            ref var businessView = ref _businessesViewPool.Get(entity);
            ref var earnMoney = ref _earnMoneyEventsPool.Add(_world.NewEntity());
            var data = businessView.Data;
            earnMoney.Earning = data.Income;
        }
    }
}


