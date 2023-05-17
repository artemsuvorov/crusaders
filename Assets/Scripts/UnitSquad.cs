using System.Collections.Generic;
using UnityEngine;

// TODO: abstract out common code in UnitSquad and Enemy classes
public class UnitSquad
{
    private const float UnitAttackRange = 1.0f;
    private const float BuildingAttackRange = 2.0f;

    // TODO: change class to HashSet<UnitController>
    private readonly List<UnitController> selectedUnits = new();

    public void SelectUnitsInArea(Vector2 start, Vector2 end)
    {
        DeselectAllUnits();

        var colliders = Physics2D.OverlapAreaAll(start, end);

        foreach (var collider in colliders)
        {
            var unit = collider.GetComponent<UnitController>();
            if (unit is null || !unit.Alive)
                continue;

            selectedUnits.Add(unit);
            unit.Select();
            //Debug.Log(unit);
        }
    }

    public void MoveUnitsTo(Vector2 position)
    {
        var positions = GetPositionsAround(position, selectedUnits.Count);

        for (var i = 0; i < selectedUnits.Count; i++)
        {
            var unit = selectedUnits[i];
            unit.MoveTo(positions[i % positions.Count]);
        }
    }

    public void MoveUnitsAndAttack(EntityController target)
    {
        foreach (var unit in selectedUnits)
        {
            if (!unit.Alive)
                return;
            if (unit != target)
                MoveUnitAndAttack(unit, target);
        }
    }

    private void MoveUnitAndAttack(UnitController unit, EntityController target)
    {
        var distance = Distance(unit, target);
        // TODO: make more generic via enitity.Size property
        if (target is UnitController && distance <= UnitAttackRange)
            unit.Attack(target);
        else if (target is BuildingController && distance <= BuildingAttackRange)
            unit.Attack(target);
        else
            unit.MoveTo(target.Position);
    }

    private void DeselectAllUnits()
    {
        foreach (var unit in selectedUnits)
            unit.Deselect();
        selectedUnits.Clear();
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
                position, (i + 1) * FirstRingCount, (i + 1) * BetweenUnitDistance);
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

    private float Distance(EntityController entity1, EntityController entity2)
    {
        return Vector2.Distance(entity1.Position, entity2.Position);
    }

    private Vector2 RotateVector(Vector2 vector, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * vector;
    }
}
