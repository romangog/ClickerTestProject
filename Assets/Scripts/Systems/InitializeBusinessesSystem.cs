using Leopotam.EcsLite;

sealed class InitializeBusinessesSystem : IEcsRunSystem, IEcsInitSystem
{
    private EcsFilter _uninitializedBusinessesFilter;
    private EcsPool<UpdateViewEvent> _updateViewPool;
    private EcsPool<InitializedTag> _initializedPool;
    private EcsPool<RecountIncomeNumberEvent> _recountIncomeEvent;

    private EcsWorld _world;
    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _updateViewPool = _world.GetPool<UpdateViewEvent>();
        _recountIncomeEvent = _world.GetPool<RecountIncomeNumberEvent>();
        _initializedPool = _world.GetPool<InitializedTag>();
        _uninitializedBusinessesFilter = _world.Filter<BusinessViewComponent>().Exc<InitializedTag>().End();
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _uninitializedBusinessesFilter)
        {
            _updateViewPool.Add(entity);
            _recountIncomeEvent.Add(entity);
            _initializedPool.Add(entity);
        }
    }
}


