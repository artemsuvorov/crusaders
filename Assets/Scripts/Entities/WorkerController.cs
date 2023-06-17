using UnityEngine;

public class WorkerController : UnitController
{
    public WorkerController() : base()
    {
        Health.Max = 30.0f;
        Health.Current = 30.0f;
        Damage = 10.0f;
    }
}
