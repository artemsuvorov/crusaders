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
        var positions = GetPositionsAround(position, selectedUnits.Count);

        for (var i = 0; i < selectedUnits.Count; i++)
        {
            var unit = selectedUnits[i];
            unit.MoveTo(positions[i % positions.Count]);
        }
    }

    private List<Vector2> GetPositionsAround(Vector2 position, int count)
    {
        const int FirstRingCount = 5;
        const float BetweenUnitDistance = 1.25f;

        var positions = new List<Vector2>() { position };
        var ringCount = Mathf.CeilToInt(((float)count) / FirstRingCount);

        for (var i = 0; i < ringCount; i++)
        {
            var positionsInRing = GetPositionsAround(
                position, (i+1) * FirstRingCount, (i+1) * BetweenUnitDistance);
            positions.AddRange(positionsInRing);
        }

        return positions;
    }

    private List<Vector2> GetPositionsAround(
        Vector2 position, int count, float distance)
    {
        var positions = new List<Vector2>();
       
        for (var i = 0; i < count; i++)
        {
            var angle = i * (360.0f / count);
            var direction = RotateVector(Vector2.right, angle);
            positions.Add(position + direction * distance);
        }

        return positions;
    }

    private Vector2 RotateVector(Vector2 vector, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * vector;
    }
}
