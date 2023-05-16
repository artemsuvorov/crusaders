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

    public float Health => health.Value;
    public bool Alive => Health > 0;

    public UnityAction<HealthEventArgs> HealthChanged;

    public void TakeDamage(float amount)
    {
        health.Value -= amount;
        var args = new HealthEventArgs(health.Value, health.Max);
        HealthChanged?.Invoke(args);
    }
}
