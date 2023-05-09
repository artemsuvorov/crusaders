using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private readonly List<UnitController> selectedUnits = new();

    [SerializeField]
    private Resources resources;

    public void OnAreaSelected(SelectionEventArgs args)
    {
        DeselectAllUnits();
        var colliders = Physics2D.OverlapAreaAll(args.Start, args.End);
        SelectUnits(colliders);
    }

    public void OnTargetPointMoved(Vector2 targetPosition)
    {
        MoveSelectedUnitsTo(targetPosition);
    }

    private void SelectUnits(Collider2D[] colliders)
    {
        foreach (var collider in colliders)
        {
            var unit = collider.GetComponent<UnitController>();
            if (unit is null)
                continue;

            selectedUnits.Add(unit);
            unit.Select();
            //Debug.Log(unit);
        }
    }

    private void DeselectAllUnits()
    {
        foreach (var unit in selectedUnits)
            unit.Deselect();
        selectedUnits.Clear();
    }

    private void MoveSelectedUnitsTo(Vector2 position)
    {
        foreach (var unit in selectedUnits)
            unit.MoveTo(position);
    }
}
