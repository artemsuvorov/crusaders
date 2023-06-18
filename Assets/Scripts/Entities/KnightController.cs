using UnityEngine;

public class KnightController : UnitController
{
    private readonly Health health = new(60.0f);

    public override Cost Cost => new(gold: 100);
    protected override Health Health => health;
    public override float Damage { get; protected set; } = 20.0f;
}
