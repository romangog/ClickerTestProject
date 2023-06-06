using Leopotam.EcsLite;
using UnityEngine;

sealed class EarningProcessingSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsWorld _world;
    private EcsFilter _earnRequestsFilter;
    private EcsFilter _balanceViewsFilter;
    private EcsPool<EarnMoneyEvent> _earningsPool;
    private EcsPool<UpdateViewEvent> _updateViewPool;
    private PlayerData _playerData;

    public EarningProcessingSystem(
        PlayerData playerData)
    {
        _playerData = playerData;
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();

        _earnRequestsFilter = _world.Filter<EarnMoneyEvent>().End();
        _earningsPool = _world.GetPool<EarnMoneyEvent>();

        _balanceViewsFilter = _world.Filter<PlayerBalanceViewComponent>().End();
        _updateViewPool = _world.GetPool<UpdateViewEvent>();
    }

    public void Run(IEcsSystems systems)
    {
        bool isNeedToUpdateView = false;
        float incomePerFrame = 0f;

        foreach (var entity in _earnRequestsFilter)
        {
            // Iterate through earning events, summarize them
            ref var earnRequest = ref _earningsPool.Get(entity);
            incomePerFrame += earnRequest.Earning;
            isNeedToUpdateView = true;
        }

        // Add earned money to balance, rounding it
        _playerData.Balance += Mathf.FloorToInt(incomePerFrame);

        // If balance has changed, update it's view
        if (isNeedToUpdateView)
            foreach (var entity in _balanceViewsFilter)
            {
                if (!_updateViewPool.Has(entity))
                    _updateViewPool.Add(entity);
            }
    }
}


