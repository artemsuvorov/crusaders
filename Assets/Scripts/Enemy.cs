using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private const float CloseDistance = 2.0f;

    [SerializeField]
    private Instantiator instantiator;

    [SerializeField]
    private GameObject unitPrefab;

    private List<UnitController> units = new();

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
            var closest = GetClosestAliveEntity(unit, entities);
            if (closest && Distance(unit, closest) <= CloseDistance)
                unit.Attack(closest);
            else if (closest)
                unit.MoveTo(closest.transform.position);
        }
    }

    private EntityController GetClosestAliveEntity(
        UnitController unit, EntityController[] entities)
    {
        EntityController closest = null;
        var minDistance = Mathf.Infinity;
        
        foreach (var entity in entities)
        {
            if (unit == entity || !entity.Alive)
                continue;
            var distance = Distance(unit, entity);
            if (distance >= minDistance)
                continue;
            closest = entity;
            minDistance = distance;
        }

        return closest;
    }

    private float Distance(EntityController entity1, EntityController entity2)
    {
        return Vector2.Distance(
            entity1.transform.position,
            entity2.transform.position
            );
    }

}
