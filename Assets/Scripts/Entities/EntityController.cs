using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class HealthEventArgs
{
    public float Health { get; private set; }
    public float MaxHealth { get; private set; }

    public HealthEventArgs(float health, float maxHealth)
    {
        Health = health;
        MaxHealth = maxHealth;
    }
}

public class Health
{
    private float max = 100, current = 100;

    public float Max
    {
        get => max;
        set
        {
            max = Mathf.Max(0, value);
            current = Mathf.Min(current, max);
        }
    }

    public float Current
    {
        get => current;
        set => current = Mathf.Min(Mathf.Max(0, value), Max);
    }

    public Health(float amount)
    {
        Max = amount;
        Current = amount;
    }
}

public abstract class EntityController : MonoBehaviour
{
    private Collider2D entityCollider;

    protected abstract Health Health { get; }
    public abstract Cost Cost { get; }

    public float HealthPoints => Health.Current;
    public bool Alive => HealthPoints > 0;

    public Vector2 Position => transform.position;
    public Vector2 Size => entityCollider.bounds.size;

    public event UnityAction<HealthEventArgs> HealthChanged;
    public event UnityAction Died;

    private void OnEnable()
    {
        entityCollider = GetComponent<Collider2D>();
        HealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        HealthChanged -= OnHealthChanged;
    }

    public virtual void TakeDamage(float amount)
    {
        Health.Current -= amount;
        var args = new HealthEventArgs(Health.Current, Health.Max);
        HealthChanged?.Invoke(args);
    }

    public float DistanceTo(EntityController other)
    {
        return Vector2.Distance(this.Position, other.Position);
    }

    private void OnHealthChanged(HealthEventArgs arg)
    {
        if (arg.Health <= 0)
            Died?.Invoke();
    }
}
