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
        squad.SelectUnitsInArea(args.Start, args.End);
    }

    public void OnTargetPointMoved(Vector2 targetPosition)
    {
        squad.MoveUnitsTo(targetPosition);
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

    public void OnEntityCreated(InstanceEventArgs args)
    {
        instantiator.Instantiate(args.Instance, args.Position);
    }

    private void OnBlueprintPlaced(InstanceEventArgs args)
    {
        var instance = instantiator.Instantiate(args.Instance, args.Position);
        var building = instance.GetComponent<ResourceBuildingController>();
        building.ResourceProduced += (produceArgs) => 
            resources.IncreaseResource(produceArgs.Resource, produceArgs.Amount);
    }
}
