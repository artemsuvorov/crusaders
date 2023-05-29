using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Instantiator instantiator;

    [SerializeField]
    private GameObject unitPrefab;

    private const float ViewRadius = 10.0f;

    private readonly Faction faction;
    private readonly HashSet<UnitController> units;

    public Faction Faction => faction;
    public int AliveEnemyCount => units.Count(u => u.Alive);

    public event UnityAction UnitDied;

    public Enemy()
    {
        faction = new Faction(FactionName.Muslim);
        units = new HashSet<UnitController>();
    }

    public void SpawnEnemyUnit(Vector2 position)
    {
        var instance = instantiator.Instantiate(unitPrefab, position);
        var unit = instance.GetComponent<UnitController>();
        faction.AddAlly(unit);
        unit.Selectable = false;
        unit.Died += () => UnitDied?.Invoke();
        units.Add(unit);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            SpawnEnemyUnit(Vector2.zero);

        foreach (var unit in units)
        {
            if (!unit.Alive)
                continue;
            var entities = GetVisibleEntities(unit);
            var closest = GetClosestHostileEntity(unit, entities);
            if (closest is null)
                continue;
            unit.MoveTo(closest);
            unit.SelectAttackTarget(closest);
        }
    }

    private static IEnumerable<EntityController> GetVisibleEntities(UnitController unit)
    {
        return Physics2D.OverlapCircleAll(unit.transform.position, ViewRadius)
            .Select(c => c.GetComponent<EntityController>())
            .Where(c => c is not null);
    }

    private EntityController GetClosestHostileEntity(
        UnitController unit, IEnumerable<EntityController> entities)
    {
        EntityController closest = null;
        var minDistance = Mathf.Infinity;
        
        foreach (var entity in entities)
        {
            if (unit == entity || !entity.Alive || faction.ConstainsAlly(entity))
                continue;
            var distance = unit.DistanceTo(entity);
            if (distance >= minDistance)
                continue;
            closest = entity;
            minDistance = distance;
        }

        return closest;
    }
}
