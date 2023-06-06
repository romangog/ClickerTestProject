using UnityEngine;

[RequireComponent(typeof(EntityView))]
public class EntityAddition<T> : MonoBehaviour, IEntityViewAddition where T:struct
{
    [SerializeField] protected T _component;
    public virtual void OnEntityCreated(int entity)
    {
         ref var comp = ref EcsStartup.World.GetPool<T>().Add(entity);
        comp = _component;
    }
}

public interface IEntityViewAddition
{
    void OnEntityCreated(int entity);
}

