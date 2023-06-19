using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public enum Resource
{
    Gold,
    Wood,
    Stone,
}

public class ResourceEventArgs
{
    public Resource Resource { get; private set; }

    public int Amount { get; private set; }

    public ResourceEventArgs(Resource resource, int amount)
    {
        Resource = resource;
        Amount = amount;
    }
}

public class Cost
{
    private readonly Dictionary<Resource, int> requiredResources = new();

    public Dictionary<Resource, int> RequiredResources => requiredResources;

    public Cost()
    {
        var resources = Enum.GetValues(typeof(Resource));
        foreach (Resource resource in resources)
            requiredResources.Add(resource, 0);
    }

    public Cost(int gold = 0, int wood = 0, int stone = 0) 
        : this()
    {
        requiredResources[Resource.Gold] = gold;
        requiredResources[Resource.Wood] = wood;
        requiredResources[Resource.Stone] = stone;
    }
}

public class Resources : MonoBehaviour
{
    private readonly Dictionary<Resource, int> values = new();

    public IReadOnlyDictionary<Resource, int> Values => values;

    public event UnityAction<Resources> ResourceChanged;

    public Resources()
    {
        var resources = Enum.GetValues(typeof(Resource));

        foreach (Resource resource in resources)
            values.Add(resource, 0);

        InitializeWithDefaultValues();
    }

    public void IncreaseResource(Resource resource, int amount)
    {
        values[resource] += amount;
        ResourceChanged?.Invoke(this);
    }

    public bool CanAfford(Cost cost)
    {
        foreach (var requiredResource in cost.RequiredResources)
        {
            var actualAmount = values[requiredResource.Key];
            var requiredAmount = requiredResource.Value;
            if (actualAmount < requiredAmount)
                return false;
        }

        return true;
    }

    public void DecreaseResource(Cost cost)
    {
        foreach (var requiredResource in cost.RequiredResources)
            IncreaseResource(requiredResource.Key, -requiredResource.Value);
    }

    public override string ToString()
    {
        var result = new StringBuilder();
        foreach (var pair in values)
        {
            result
                .Append(pair.Key)
                .Append(": ")
                .Append(pair.Value)
                .AppendLine();
        }
        return result.ToString();
    }

    private void InitializeWithDefaultValues()
    {
        values[Resource.Gold] = 150;
        values[Resource.Wood] = 20;
        values[Resource.Stone] = 0;
    }
}
