using UnityEngine.Events;

public abstract class ResourceBuildingController : BuildingController
{
    protected virtual float Delay => 2.0f;
    protected virtual float RepeatRate => 2.0f;

    protected abstract Resource Resource { get; }
    protected abstract int Amount { get; }

    public event UnityAction<ResourceEventArgs> ResourceProduced;

    protected void IncreaseResource()
    {
        var args = new ResourceEventArgs(Resource, Amount);
        ResourceProduced?.Invoke(args);
    }

    private void Start()
    {
        InvokeRepeating(nameof(IncreaseResource), Delay, RepeatRate);
    }
}