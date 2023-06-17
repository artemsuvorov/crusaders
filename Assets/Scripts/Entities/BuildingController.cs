using UnityEngine;

public abstract class BuildingController : EntityController
{
    public abstract Cost Cost { get; }

    public BuildingController()
    {
        Health.Max = 200.0f;
        Health.Current = 200.0f;
    }
}
