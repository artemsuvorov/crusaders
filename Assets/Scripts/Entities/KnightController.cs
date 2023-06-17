using UnityEngine;

public class KnightController : UnitController
{
    public KnightController() : base()
    {
        Health.Max = 100.0f;
        Health.Current = 100.0f;
        Damage = 20.0f;
    }
}
