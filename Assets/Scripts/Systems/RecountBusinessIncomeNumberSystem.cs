using Leopotam.EcsLite;

sealed class RecountBusinessIncomeNumberSystem : IEcsRunSystem, IEcsInitSystem
{
    private readonly BusinessesConfigs _configs;

    private EcsFilter _businesssesFilter;
    private EcsPool<BusinessViewComponent> _businessesPool;
    private EcsWorld _world;

    public RecountBusinessIncomeNumberSystem(BusinessesConfigs configs)
    {
        this._configs = configs;
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _businesssesFilter = _world.Filter<BusinessViewComponent>().Inc<RecountIncomeNumberEvent>().End();
        _businessesPool = _world.GetPool<BusinessViewComponent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _businesssesFilter)
        {
            // Update business income based on it's configs, upgrades and level
            ref var business = ref _businessesPool.Get(entity);
            var data = business.Data;
            var config = _configs.BusinessesList[data.Id];
            float upgradesBonus = 100f;
            for (int i = 0; i < data.UpgradesBought.Count; i++)
            {
                if (data.UpgradesBought[i])
                {
                    upgradesBonus += config.Upgrades[i].IncomeAddPercent;
                }
            }
            upgradesBonus /= 100f;
            data.Income = data.Level * config.BaseIncome * upgradesBonus;
        }
    }
}


