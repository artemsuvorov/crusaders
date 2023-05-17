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
    private float value = 100;

    public float Max { get; set; } = 100;

    public float Value
    {
        get => value;
        set => this.value = Mathf.Min(Mathf.Max(0, value), Max);
    }
}

public class EntityController : MonoBehaviour
{
    private readonly Health health = new();
    private Collider2D entityCollider;

    public float Health => health.Value;
    public bool Alive => Health > 0;

    public Vector2 Position => transform.position;
    public Vector2 Size => entityCollider.bounds.size;

    public UnityAction<HealthEventArgs> HealthChanged;

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
        health.Value -= amount;
        var args = new HealthEventArgs(health.Value, health.Max);
        HealthChanged?.Invoke(args);
    }

    protected virtual void OnDead() { }

    private void OnHealthChanged(HealthEventArgs arg)
    {
        if (arg.Health <= 0)
            OnDead();
    }
}
