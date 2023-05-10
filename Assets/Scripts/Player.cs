using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Resources resources;

    private readonly UnitSquad squad = new();

    public void OnAreaSelected(SelectionEventArgs args)
    {
        squad.SelectUnitsInArea(args.Start, args.End);
    }

    public void OnTargetPointMoved(Vector2 targetPosition)
    {
        squad.MoveUnitsTo(targetPosition);
    }
}
