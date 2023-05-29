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
    private BuildingController[] availableBuildings;

    private readonly Faction faction;
    private readonly UnitSquad squad;

    public Resources Resources => resources;
    public Faction Faction => faction;

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
        squad.SelectUnitsInArea(args.Start, args.End);
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
        if (entity)
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

    private void OnResourceChanged(Resources resources)
    {
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
