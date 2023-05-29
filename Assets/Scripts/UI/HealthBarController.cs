using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private Slider slider;

    private Transform entityTransform;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        var position = entityTransform.position + 
            0.8f * entityTransform.localScale.y * Vector3.up;
        transform.position = ToScreenPosition(position);
    }

    public void Observe(EntityController entity)
    {
        entity.HealthChanged += OnHealthChanged;
        entityTransform = entity.transform;
    }

    private void OnHealthChanged(HealthEventArgs args)
    {
        slider.maxValue = args.MaxHealth;
        slider.value = args.Health;
    }

    private Vector3 ToScreenPosition(Vector3 position)
    {
        return Camera.main.WorldToScreenPoint(position);
    }
}
