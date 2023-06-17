using UnityEngine;
using UnityEngine.Events;

public abstract class UnitBuildingController : BuildingController
{
    private const float Radius = 2.5f;

    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private GameObject unitPrefab;

    protected virtual float Delay => 2.0f;
    protected virtual float RepeatRate => 10.0f;

    public event UnityAction<InstanceEventArgs> UnitCreated;

    public void CreateUnit()
    {
        CreateUnit(unitPrefab);
    }

    public void CreateUnit(GameObject unit)
    {
        if (!Alive)
            return;
        var position = GetRandomSpawnPosition(Radius);
        var args = new InstanceEventArgs(position, unit);
        UnitCreated?.Invoke(args);
    }

    private Vector2 GetRandomSpawnPosition(float radius)
    {
        Vector2 origin = spawnPoint.position;
        return origin + Random.insideUnitCircle * radius;
    }

    private void Start()
    {
        InvokeRepeating(nameof(CreateUnit), Delay, RepeatRate);
    }
}
