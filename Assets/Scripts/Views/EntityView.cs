using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityView : MonoBehaviour
{
    public int Entity { get; private set; }

    private bool _IsInitialized;

    private void Start()
    {
        InitializeEntity();
    }

    public void InitializeEntity()
    {
        if (_IsInitialized) return;

        _IsInitialized = true;

        Entity = EcsStartup.World.NewEntity();

        IEntityViewAddition[] addons = this.GetComponents<IEntityViewAddition>();

        foreach (var addon in addons)
        {
            addon.OnEntityCreated(Entity);
        }
    }
}
