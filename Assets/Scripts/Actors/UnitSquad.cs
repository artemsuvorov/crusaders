using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitSquad
{
    private readonly Faction faction;
    private readonly HashSet<UnitController> selectedUnits;

    public UnitSquad(Faction faction)
    {
        this.faction = faction;
        selectedUnits = new HashSet<UnitController>();
    }

    public void SelectUnitsInArea(Vector2 start, Vector2 end)
    {
        DeselectAllUnits();

        var colliders = Physics2D.OverlapAreaAll(start, end);
        foreach (var collider in colliders)
        {
            var unit = collider.GetComponent<UnitController>();
            if (unit is null || !unit.Alive || !unit.Selectable)
                continue;

            unit.Selected = true;
            selectedUnits.Add(unit);
        }
    }

    public void MoveUnitsAndAutoAttack(Vector2 position)
    {
        var target = FindClosestAttackTargetAround(position);
        if (target is null || faction.ConstainsAlly(target))
            MoveUnitsTo(position);
        else
            SelectAttackTarget(target);
    }

    public void Deselect(UnitController unit)
    {
        selectedUnits.Remove(unit);
        unit.Selected = false;
    }

    public void DeselectAllUnits()
    {
        foreach (var unit in selectedUnits)
            unit.Selected = false;
        selectedUnits.Clear();
    }

    private void MoveUnitsTo(Vector2 position)
    {
        var positions = PositionUtils.GetPositionsAround(position, selectedUnits.Count);

        var index = 0;
        foreach (var unit in selectedUnits)
        {
            unit.DeselectAttackTarget();
            unit.MoveTo(positions[index]);
            index++;
        }
    }

    private void SelectAttackTarget(EntityController entity)
    {
        foreach (var unit in selectedUnits)
        {
            if (unit != entity)
                unit.SelectAttackTarget(entity);
        }
    }

    private EntityController FindClosestAttackTargetAround(Vector2 position)
    {
        const float Radius = 0.5f;

        var colliders = Physics2D.OverlapCircleAll(position, Radius);
        if (colliders is null || colliders.Length <= 0)
            return null;

        //Debug.Log(string.Join(" ", colliders.Select(x => x.ToString())));
        var candidate = colliders.FirstOrDefault(e =>
        {
            var entity = e.GetComponent<EntityController>();
            return entity is not null && !faction.ConstainsAlly(entity);
        });
        if (candidate is null)
            return null;

        var entity = candidate.GetComponent<EntityController>();
        if (entity is null || !entity.Alive)
            return null;

        return entity;
    }
}
