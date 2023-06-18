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

    [SerializeField]
    private Transform factionParent;

    private readonly Faction faction;
    private readonly HashSet<UnitController> units;

    public Faction Faction => faction;
    public int AliveEnemyCount => units.Count(u => u.Alive);

    public event UnityAction UnitDied;

    public float ViewRadius { get; set; } = 10.0f;

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

    private void Awake()
    {
        LoadMissionEntities();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            SpawnEnemyUnit(Vector2.zero);
        foreach (var unit in units)
            MoveUnitAndAttackClosest(unit);
    }

    private void MoveUnitAndAttackClosest(UnitController unit)
    {
        if (!unit.Alive)
            return;
        var entities = GetVisibleEntities(unit);
        var closest = GetClosestHostileEntity(unit, entities);
        if (closest is null)
            return;
        unit.MoveTo(closest);
        unit.SelectAttackTarget(closest);
    }

    private IEnumerable<EntityController> GetVisibleEntities(UnitController unit)
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

    private void LoadMissionEntities()
    {
        foreach (Transform child in factionParent)
        {
            var entity = child.GetComponent<EntityController>();
            if (entity is null)
                continue;
            faction.AddAlly(entity);

            if (entity is not UnitController unit)
                continue;
            unit.Selectable = false;
            unit.Died += () => UnitDied?.Invoke();
            units.Add(unit);
        }
    }
}
