using Leopotam.EcsLite;

sealed class SpendingsProcessingSystem : IEcsRunSystem, IEcsInitSystem
{
    private readonly PlayerData _playerData;

    private EcsFilter _spendRequestsFilter;
    private EcsFilter _balanceViewsFilter;
    private EcsPool<TrySpendMoneyRequest> _spendRequestsPool;
    private EcsPool<UpdateViewEvent> _updateViewPool;

    private EcsWorld _world;

    public SpendingsProcessingSystem(
        PlayerData playerData)
    {
        _playerData = playerData;
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();

        _spendRequestsFilter = _world.Filter<TrySpendMoneyRequest>().End();
        _spendRequestsPool = _world.GetPool<TrySpendMoneyRequest>();

        _balanceViewsFilter = _world.Filter<PlayerBalanceViewComponent>().End();
        _updateViewPool = _world.GetPool<UpdateViewEvent>();

    }

    public void Run(IEcsSystems systems)
    {
        bool isBalanceChanged = false;
        foreach (var entity in _spendRequestsFilter)
        {
            // Check spend requests, approve it if possible
            ref var spendRequest = ref _spendRequestsPool.Get(entity);
            if (_playerData.Balance >= spendRequest.Price)
            {
                _playerData.Balance -= spendRequest.Price;
                isBalanceChanged = true;
                spendRequest.ResponseEvent?.Invoke(true);
            }
            else
            {
                spendRequest.ResponseEvent?.Invoke(false);
            }
        }

        // If money were spend, update balance view
        if (isBalanceChanged)
            foreach (var entity in _balanceViewsFilter)
            {
                if (!_updateViewPool.Has(entity))
                    _updateViewPool.Add(entity);
            }
    }
}


