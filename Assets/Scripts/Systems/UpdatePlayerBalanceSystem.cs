using Leopotam.EcsLite;

sealed class UpdatePlayerBalanceSystem : IEcsRunSystem, IEcsInitSystem
{
    private readonly PlayerData _playerData;
    private EcsFilter _playerBalanceUpdateFilter;
    private EcsPool<PlayerBalanceViewComponent> _playerBalanceViewPool;

    public UpdatePlayerBalanceSystem(PlayerData playerData)
    {
        _playerData = playerData;
    }

    public void Init(IEcsSystems systems)
    {
        EcsWorld world = systems.GetWorld();
        _playerBalanceUpdateFilter = world.Filter<PlayerBalanceViewComponent>().Inc<UpdateViewEvent>().End();
        _playerBalanceViewPool = world.GetPool<PlayerBalanceViewComponent>();

    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _playerBalanceUpdateFilter)
        {
            ref var balanceView = ref _playerBalanceViewPool.Get(entity);
            balanceView.BalanceLabel.text = "Баланс: " + _playerData.Balance + "$";
        }
    }
}


