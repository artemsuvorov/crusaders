using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private const float UnitAttackRange = 1.0f;
    private const float BuildingAttackRange = 2.0f;

    [SerializeField]
    private Instantiator instantiator;

    [SerializeField]
    private GameObject unitPrefab;

    private HashSet<UnitController> units = new();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            var instance = instantiator.Instantiate(unitPrefab, Vector2.zero);
            var unit = instance.GetComponent<UnitController>();
            units.Add(unit);
        }

        var entities = FindObjectsOfType<EntityController>();
        foreach (var unit in units)
        {
            if (!unit.Alive)
                continue;
            var closest = GetClosestHostileEntity(unit, entities);
            if (closest)
                MoveAndAttack(unit, closest);
        }
    }

    private void MoveAndAttack(UnitController unit, EntityController closest)
    {
        var distance = Distance(unit, closest);
        // TODO: make more generic via enitity.Size property
        if (closest is UnitController && distance <= UnitAttackRange)
            unit.Attack(closest);
        else if (closest is BuildingController && distance <= BuildingAttackRange)
            unit.Attack(closest);
        else
            unit.MoveTo(closest.Position);
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
