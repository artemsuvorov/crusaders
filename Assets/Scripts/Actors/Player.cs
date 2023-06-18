using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Resources resources;

    [SerializeField]
    private Instantiator instantiator;

    [SerializeField]
    private Transform factionParent;

    [SerializeField]
    private UnitController[] availableUnits;

    [SerializeField]
    private BuildingController[] availableBuildings;

    private readonly Faction faction;
    private readonly UnitSquad squad;

    public Resources Resources => resources;
    public Faction Faction => faction;

    public event UnityAction TownhallSelected;
    public event UnityAction TownhallDeselected;
    
    public event UnityAction<UnitController> UnitBecameAvailable;
    public event UnityAction<UnitController> UnitBecameUnavailable;

    public event UnityAction<BuildingController> BuildingBecameAvailable;
    public event UnityAction<BuildingController> BuildingBecameUnavailable;

    public Player()
    {
        faction = new Faction(FactionName.English);
        squad = new UnitSquad(faction);
    }

    private void Awake()
    {
        resources.ResourceChanged += OnResourceChanged;
        OnResourceChanged(resources);
        LoadMissionEntities();
    }

    public void OnAreaSelected(SelectionEventArgs args)
    {
        var colliders = Physics2D.OverlapAreaAll(args.Start, args.End);
        var hasUnits = colliders
            .Any(c => c.GetComponent<UnitController>() is not null);

        if (hasUnits)
        {
            squad.SelectUnitsInArea(args.Start, args.End);
            TownhallDeselected?.Invoke();
            return;
        }

        var townhall = colliders.FirstOrDefault(c => 
            c.GetComponent<TownhallController>() == faction.Townhall);
        if (townhall is null)
            return;

        TownhallSelected?.Invoke();
        squad.DeselectAllUnits();
    }

    public void OnTargetPointMoved(Vector2 targetPosition)
    {
        squad.MoveUnitsAndAutoAttack(targetPosition);
        //squad.MoveUnitsTo(targetPosition);
        //squad.AutoAttackClosestTargetAt(targetPosition);
    }

    public void OnEntityCreated(InstanceEventArgs args)
    {
        var instance = instantiator.Instantiate(args.Instance, args.Position);
        var entity = instance.GetComponent<EntityController>();
        if (entity is null)
            return;
        resources.DecreaseResource(entity.Cost);
        faction.AddAlly(entity);
    }

    public void OnEntityDied(InstanceEventArgs args)
    {
        var unit = args.Instance.GetComponent<UnitController>();
        if (unit is not null)
            unit.Died += () => squad.Deselect(unit);
    }

    public void OnEntityBlueprinting(InstanceEventArgs args)
    {
        var blueprintInstance =
            instantiator.Instantiate(args.Instance, args.Position);
        var blueprintController = blueprintInstance
            .GetComponentInParent<BlueprintController>();
        if (blueprintController is not null)
            blueprintController.Placed += OnBlueprintPlaced;
    }

    private void OnBlueprintPlaced(InstanceEventArgs args)
    {
        var instance = instantiator.Instantiate(args.Instance, args.Position);
        var entity = instance.GetComponent<EntityController>();
        if (entity)
            faction.AddAlly(entity);

        var building = instance.GetComponent<BuildingController>();
        if (building is not null)
            resources.DecreaseResource(building.Cost);

        var resourceBuilding = instance.GetComponent<ResourceBuildingController>();
        if (resourceBuilding is not null)
            resourceBuilding.ResourceProduced += (resourceArgs) => 
                resources.IncreaseResource(resourceArgs.Resource, resourceArgs.Amount);

        var unitBuilding = instance.GetComponent<UnitBuildingController>();
        if (unitBuilding is not null)
            unitBuilding.UnitCreated += (unitArgs) => OnEntityCreated(unitArgs);
    }

    public void OnResourceSold(Resource resource)
    {
        if (resources.Values[resource] <= 0)
            return;

        resources.IncreaseResource(resource, -1);

        // TODO: introduce resource class with its appropriate cost
        var cost = resource switch
        {
            Resource.Wood => 2,
            Resource.Stone => 4,
            _ => throw new ArgumentException(
                $"Unexpected resource {resource} to be sold.")
        };
        resources.IncreaseResource(Resource.Gold, cost);
    }

    private void OnResourceChanged(Resources resources)
    {
        foreach (var unit in availableUnits)
        {
            if (resources.CanAfford(unit.Cost))
                UnitBecameAvailable?.Invoke(unit);
            else
                UnitBecameUnavailable?.Invoke(unit);
        }

        foreach (var building in availableBuildings)
        {
            if (resources.CanAfford(building.Cost))
                BuildingBecameAvailable?.Invoke(building);
            else
                BuildingBecameUnavailable?.Invoke(building);
        }
    }

    private void LoadMissionEntities()
    {
        foreach (Transform child in factionParent)
        {
            var entity = child.GetComponent<EntityController>();
            if (entity is null)
                continue;

            faction.AddAlly(entity);

            var resourceBuilding = entity.GetComponent<ResourceBuildingController>();
            if (resourceBuilding is not null)
                resourceBuilding.ResourceProduced += (resourceArgs) =>
                    resources.IncreaseResource(resourceArgs.Resource, resourceArgs.Amount);

            var unitBuilding = entity.GetComponent<UnitBuildingController>();
            if (unitBuilding is not null)
                unitBuilding.UnitCreated += (unitArgs) => OnEntityCreated(unitArgs);
        }
    }
}
