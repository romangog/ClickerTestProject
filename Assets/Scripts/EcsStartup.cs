using Leopotam.EcsLite;
using System.IO;
using UnityEngine;

public class EcsStartup : MonoBehaviour
{
    public static EcsWorld World
    {
        get
        {
            if (_world == null)
            {
                _world = new EcsWorld();
            }
            return _world;
        }
    }

    IEcsSystems _systems;
    private static EcsWorld _world;
    private PlayerData _playerData;
    private BusinessesConfigs _configs;
    private Prefabs _prefabs;
    private bool _savedGame;

    void Awake()
    {
        _configs = Resources.Load<BusinessesConfigs>("BusinessesConfigs");
        _prefabs = Resources.Load<Prefabs>("Prefabs");

        _playerData = new PlayerData();

        LoadGame();
    }

    private void Start()
    {
        _systems = new EcsSystems(World, _configs);
        _systems
             .Add(new SpawnBusinessPanelsSystem(_configs, _playerData, _prefabs))
                .Add(new EventDeleteSystem<UnlockedNewBusinessEvent>())
             .Add(new InitializeBusinessesSystem())
             .Add(new LvlUpButtonClickedSystem(_configs))
             .Add(new UpgradeButtonClickedSystem(_configs))
             .Add(new UpdateBusinessesIncomeProgressSystem(_configs))
             .Add(new AccountBusinessIncomeSystem())
             .Add(new EarningProcessingSystem(_playerData))
             .Add(new SpendingsProcessingSystem(_playerData))
             .Add(new RecountBusinessIncomeNumberSystem(_configs))
             .Add(new UpdateBusinessReactiveViewSystem())
             .Add(new UpdatePlayerBalanceSystem(_playerData))

                .Add(new EventDeleteSystem<ClickedEvent>())
                .Add(new EventDeleteSystem<UpdateViewEvent>())
                .Add(new EventDeleteSystem<TrySpendMoneyRequest>())
                .Add(new EventDeleteSystem<BusinessIncomeEvent>())
                .Add(new EventDeleteSystem<EarnMoneyEvent>())
                .Init();
    }

    void Update()
    {
        _systems?.Run();
    }

    void OnDestroy()
    {
        if (_systems != null)
        {
            _systems.Destroy();
            _systems = null;
        }

        if (_world != null)
        {
            _world.Destroy();
            _world = null;
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus && !_savedGame)
        {
            SaveGame();
        }
    }

    private void OnApplicationPause(bool paused)
    {
        if (paused && !_savedGame)
        {
            SaveGame();
        }
    }

    private void LoadGame()
    {
        string json = PlayerPrefs.GetString("saveData");// Save game as JSON file on quit
        if (json == "")
        {
            _playerData.CopyFrom(new SaveData(_configs));
        }
        else
        {
            _playerData.CopyFrom(JsonUtility.FromJson<SaveData>(json));
        }
    }

    private void SaveGame()
    {
        string json = JsonUtility.ToJson(_playerData.SaveData);
        PlayerPrefs.SetString("saveData", json);// Save game as JSON file on quit
        _savedGame = true;
    }
}
