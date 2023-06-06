using Leopotam.EcsLite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickEmitter : MonoBehaviour ,IEntityViewAddition
{
    [SerializeField] private Button _button;

    private int _entity;

    private void Start()
    {
        _button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        EcsStartup.World.GetPool<ClickedEvent>().Add(_entity);
    }

    public void OnEntityCreated(int entity)
    {
        _entity = entity;
    }
}
