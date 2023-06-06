using Leopotam.EcsLite;
using UnityEngine;

sealed class SpawnBusinessPanelsSystem : IEcsRunSystem, IEcsInitSystem
{
    private readonly BusinessesConfigs _businessesConfigs;
    private readonly PlayerData _playerData;
    private readonly Prefabs _prefabs;

    private EcsFilter _businessesListFilter;
    private EcsFilter _boughtBusinessEventsFilter;
    private EcsPool<BusinessesListComponent> _businessesListPool;
    private EcsPool<BusinessViewComponent> _businessComponentsPool;
    private EcsPool<UpgradeBusinessButtonComponent> _upgradeButtonsViewPool;
    private EcsPool<BusinessWorkingTag> _businessWorkingPool;
    private EcsPool<SaveEvent> _saveEventsPool;

    private EcsWorld _world;

    public SpawnBusinessPanelsSystem(
        BusinessesConfigs businessesConfigs,
        PlayerData playerData,
        Prefabs prefabs)
    {
        _businessesConfigs = businessesConfigs;
        _playerData = playerData;
        _prefabs = prefabs;
    }

    public void Init(IEcsSystems systems)
    {
        _world = systems.GetWorld();
        _businessesListFilter = _world.Filter<BusinessesListComponent>().End();
        _businessesListPool = _world.GetPool<BusinessesListComponent>();
        _businessComponentsPool = _world.GetPool<BusinessViewComponent>();
        _upgradeButtonsViewPool = _world.GetPool<UpgradeBusinessButtonComponent>();
        _businessWorkingPool = _world.GetPool<BusinessWorkingTag>();
        _saveEventsPool = _world.GetPool<SaveEvent>();

        _boughtBusinessEventsFilter = _world.Filter<UnlockedNewBusinessEvent>().End();

        // Spawn businesses at start based on player data
        foreach (var entity in _businessesListFilter)
        {
            ref var businessList = ref _businessesListPool.Get(entity);

            for (int i = 0; i < _playerData.BoughtBusinesses.Count; i++)
            {
                SpawnBusinessPanel(businessList.ContentParent, i);
            }
        }
    }

    public void Run(IEcsSystems systems)
    {
        foreach (var eventEntity in _boughtBusinessEventsFilter)
        {
            foreach (var businessListEntity in _businessesListFilter)
            {
                // Spawn new business when previous gets first level up
                _saveEventsPool.Add(_world.NewEntity());
                if (_playerData.BoughtBusinesses.Count == _businessesConfigs.BusinessesList.Length) return;
                ref var businessList = ref _businessesListPool.Get(businessListEntity);
                int id = _playerData.BoughtBusinesses.Count;
                _playerData.BoughtBusinesses.Add(
                    new BusinessData(
                        0,
                        _playerData.BoughtBusinesses.Count,
                        _businessesConfigs.BusinessesList[id].BaseCost,
                        _businessesConfigs.BusinessesList[id].BaseIncome,
                        _businessesConfigs.BusinessesList[id].Upgrades.Length));

                SpawnBusinessPanel(businessList.ContentParent, id);
            }
        }
    }

    // Spawn business view, associate it with data, display static data (name, upgrade effects etc)
    private void SpawnBusinessPanel(Transform parent, int id)
    {
        // Spawn business panel
        var businessEntityView = GameObject.Instantiate(_prefabs.BusinessPanelPrefab, parent);
        // Initialize MonoBehaviour Entity holder in purpose to access it immediately
        businessEntityView.InitializeEntity();
        ref var businessView = ref _businessComponentsPool.Get(businessEntityView.Entity);
        var businessConfig = _businessesConfigs.BusinessesList[id];
        businessView.Name.text = businessConfig.Name;
        // Associate business view with business data
        businessView.Data = _playerData.BoughtBusinesses[id];
        businessView.Data.NextLevelCost = businessConfig.BaseCost * (businessView.Data.Level + 1);
        businessView.LvlUpBusinessButtonComponent.SetBusinessEntity(businessEntityView.Entity);

        // Spawn upgrades according to business config, display non-changing data
        businessView.UpgradeButtonsEntities = new int[businessConfig.Upgrades.Length];
        for (int j = 0; j < businessConfig.Upgrades.Length; j++)
        {
            var upgradeConfig = businessConfig.Upgrades[j];
            var upgradeButtonEntityView = GameObject.Instantiate(_prefabs.UpgradeButtonView, businessView.UpgradesGroup);
            upgradeButtonEntityView.InitializeEntity();
            businessView.UpgradeButtonsEntities[j] = upgradeButtonEntityView.Entity;
            ref var upgradeButtonComponent = ref _upgradeButtonsViewPool.Get(upgradeButtonEntityView.Entity);
            upgradeButtonComponent.BusinessEntity = businessEntityView.Entity;
            upgradeButtonComponent.Id = j;
            upgradeButtonComponent.NameLabel.text = upgradeConfig.UpgradeName;
            upgradeButtonComponent.UpgradeEffectLabel.text = string.Format($"Доход: + {upgradeConfig.IncomeAddPercent}%");
            upgradeButtonComponent.CostLabel.text = string.Format($"Цена: {upgradeConfig.Cost}$");
        }

        if (businessView.Data.Level > 0)
        {
            _businessWorkingPool.Add(businessEntityView.Entity);
        }
    }
}


