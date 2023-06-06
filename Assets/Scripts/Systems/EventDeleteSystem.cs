using Leopotam.EcsLite;

sealed class EventDeleteSystem<T> : IEcsRunSystem, IEcsInitSystem where T : struct
{
    private EcsFilter _eventsFilter;
    private EcsPool<T> _clicksPool;

    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        _eventsFilter = world.Filter<T>().End();
        _clicksPool = world.GetPool<T>();
    }

    public void Run(IEcsSystems systems)
    {
        // Delete OneFrame Events
        foreach (var entity in _eventsFilter)
        {
            _clicksPool.Del(entity);
        }
    }
}


