using Leopotam.EcsLite;
using UnityEngine;

sealed class UpdateBusinessesIncomeProgressSystem : IEcsRunSystem, IEcsInitSystem
{
    private readonly BusinessesConfigs _configs;
    private EcsFilter _boughtBusinessesFilter;
    private EcsPool<BusinessViewComponent> _businessesViewsPool;
    private EcsPool<BusinessIncomeEvent> _businessIncomePool;

    private EcsWorld _world;

    public UpdateBusinessesIncomeProgressSystem(BusinessesConfigs configs)
    {
        _configs = configs;
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _boughtBusinessesFilter = _world.Filter<BusinessViewComponent>().Inc<BusinessWorkingTag>().End();
        _businessesViewsPool = _world.GetPool<BusinessViewComponent>();
        _businessIncomePool = _world.GetPool<BusinessIncomeEvent>();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _boughtBusinessesFilter)
        {
            // Update business income progress, fire event if income delay expired
            ref var businessView = ref _businessesViewsPool.Get(entity);
            var data = businessView.Data;
            var config = _configs.BusinessesList[data.Id];
            data.BusinessTime = Mathf.MoveTowards(data.BusinessTime, config.IncomeDelay, Time.deltaTime);

            if (data.BusinessTime == config.IncomeDelay)
            {
                _businessIncomePool.Add(entity);
                data.BusinessTime = 0f;
            }

            businessView.IncomeDelaySlider.value = data.BusinessTime / config.IncomeDelay;
        }
    }
}


