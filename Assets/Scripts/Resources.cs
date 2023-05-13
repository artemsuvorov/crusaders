using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public enum Resource
{
    Wood,
    Stone
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

public class Resources : MonoBehaviour
{
    private readonly Dictionary<Resource, int> values = new();

    public IReadOnlyDictionary<Resource, int> Values => values;

    public UnityEvent<Resources> ResourceIncreased;

    public Resources()
    {
        values.Add(Resource.Wood, 0);
        values.Add(Resource.Stone, 0);
    }

    public void IncreaseResource(Resource resource, int amount)
    {
        values[resource] += amount;
        ResourceIncreased?.Invoke(this);
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
}
