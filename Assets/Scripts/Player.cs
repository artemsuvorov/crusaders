using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Resources resources;

    [SerializeField]
    private Instantiator instantiator;

    private readonly Faction faction;
    private readonly UnitSquad squad;

    public Player()
    {
        faction = new Faction(FactionName.English);
        squad = new UnitSquad(faction);
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

        var resourceBuilding = instance.GetComponent<ResourceBuildingController>();
        if (resourceBuilding is not null)
            resourceBuilding.ResourceProduced += (resourceArgs) => 
                resources.IncreaseResource(resourceArgs.Resource, resourceArgs.Amount);

        var unitBuilding = instance.GetComponent<UnitBuildingController>();
        if (unitBuilding is not null)
            unitBuilding.UnitCreated += (unitArgs) => OnEntityCreated(unitArgs);
    }
}
