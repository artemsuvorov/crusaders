using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Resources resources;

    [SerializeField]
    private Instantiator instantiator;

    private readonly UnitSquad squad = new();

    public void OnAreaSelected(SelectionEventArgs args)
    {
        if (args.MouseButton == MouseButton.Left)
        {
            squad.SelectUnitsInArea(args.Start, args.End);
        }
        else if (args.MouseButton == MouseButton.Right)
        {
            var attackTarget = SelectAttackTargetInArea(args.Start, args.End);
            if (attackTarget)
                squad.MoveUnitsAndAttack(attackTarget);
        }
    }

    private EntityController SelectAttackTargetInArea(Vector2 start, Vector2 end)
    {
        var collider = Physics2D.OverlapArea(start, end);
        if (collider is null)
            return null;
        var entity = collider.GetComponent<EntityController>();
        if (entity is null || !entity.Alive)
            return null;
        return entity;
    }

    public void OnTargetPointMoved(Vector2 targetPosition)
    {
        squad.MoveUnitsTo(targetPosition);
    }

    public void OnEntityCreated(InstanceEventArgs args)
    {
        instantiator.Instantiate(args.Instance, args.Position);
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
