using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    private Slider slider;
    private Image fill;

    private EntityController entity;
    private Transform entityTransform;

    private void Start()
    {
        slider = GetComponent<Slider>();
        fill = transform.Find("Fill").GetComponent<Image>();
    }

    private void Update()
    {
        var position = entityTransform.position + 
            0.8f * entityTransform.localScale.y * Vector3.up;
        transform.position = ToScreenPosition(position);

        fill.color = entity.FactionName switch
        {
            FactionName.English => Color.red,
            FactionName.French => Color.blue,
            FactionName.German => Color.grey,
            FactionName.Muslim => Color.green,
            _ => Color.red
        };
    }

    public void Observe(EntityController entity)
    {
        this.entity = entity;
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
