using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Instantiator instantiator;

    [SerializeField]
    private GameObject unitPrefab;

    private readonly Faction faction;
    private readonly HashSet<UnitController> units;

    public Faction Faction => faction;

    public Enemy()
    {
        faction = new Faction(FactionName.Muslim);
        units = new HashSet<UnitController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            var instance = instantiator.Instantiate(unitPrefab, Vector2.zero);
            var unit = instance.GetComponent<UnitController>();
            faction.AddAlly(unit);
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
