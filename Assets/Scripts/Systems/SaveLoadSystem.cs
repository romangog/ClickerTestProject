using Leopotam.EcsLite;
using System.IO;
using UnityEngine;

sealed class SaveLoadSystem : IEcsRunSystem, IEcsInitSystem
{
    private readonly BusinessesConfigs _configs;

    private PlayerData _playerData;
    private EcsFilter _saveEventsFilter;

    private EcsWorld _world;

    public SaveLoadSystem(PlayerData playerData, BusinessesConfigs config)
    {
        _playerData = playerData;
        _configs = config;
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _saveEventsFilter = _world.Filter<SaveEvent>().End();

        string savePath = Application.persistentDataPath + "save";

        if (File.Exists(savePath))
        {
            using (var read = new StreamReader(savePath))
            {
                string json = read.ReadToEnd();
                _playerData.CopyFrom(JsonUtility.FromJson<SaveData>(json));
            }
        }
        else
        {
            _playerData.CopyFrom(new SaveData(_configs));
        }
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var entity in _saveEventsFilter)
        {
            string savePath = Application.persistentDataPath + "save";
            string json = JsonUtility.ToJson(_playerData.SaveData);
            using (var writer = new StreamWriter(savePath))
            {
                writer.Write(json);
            }
        }
    }
}


