using UnityEngine;
using UnityEngine.Events;

public abstract class UnitBuildingController : BuildingController
{
    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private GameObject unitPrefab;

    protected virtual float Delay => 2.0f;
    protected virtual float RepeatRate => 10.0f;

    public event UnityAction<InstanceEventArgs> UnitCreated;

    protected void CreateUnit()
    {
        if (!Alive)
            return;
        var position = (Vector2)spawnPoint.position;
        var args = new InstanceEventArgs(position, unitPrefab);
        UnitCreated?.Invoke(args);
    }

    private void Start()
    {
        InvokeRepeating(nameof(CreateUnit), Delay, RepeatRate);
    }
}
