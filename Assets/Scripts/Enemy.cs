using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Instantiator instantiator;

    [SerializeField]
    private GameObject unitPrefab;

    private readonly HashSet<UnitController> units = new();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            var instance = instantiator.Instantiate(unitPrefab, Vector2.zero);
            var unit = instance.GetComponent<UnitController>();
            unit.Selectable = false;
            units.Add(unit);
        }

        var entities = FindObjectsOfType<EntityController>();
        foreach (var unit in units)
        {
            if (!unit.Alive)
                continue;
            var closest = GetClosestHostileEntity(unit, entities);
            if (closest is null)
                continue;
            unit.MoveTo(closest);
            unit.SelectAttackTarget(closest);
        }
    }

    private EntityController GetClosestHostileEntity(
        UnitController unit, EntityController[] entities)
    {
        EntityController closest = null;
        var minDistance = Mathf.Infinity;
        
        foreach (var entity in entities)
        {
            if (unit == entity || !entity.Alive || IsAlly(entity))
                continue;
            var distance = Distance(unit, entity);
            if (distance >= minDistance)
                continue;
            closest = entity;
            minDistance = distance;
        }

        return closest;
    }

    private bool IsAlly(EntityController entity)
    {
        return entity is UnitController otherUnit && units.Contains(otherUnit);
    }

    private float Distance(EntityController entity1, EntityController entity2)
    {
        return Vector2.Distance(entity1.Position, entity2.Position);
    }
}
