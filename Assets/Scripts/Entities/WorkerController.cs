using UnityEngine;

public class WorkerController : UnitController
{
    private readonly Health health = new(30.0f);

    public override Cost Cost => new(gold: 30);
    protected override Health Health => health;
    public override float Damage { get; protected set; } = 10.0f;
}
