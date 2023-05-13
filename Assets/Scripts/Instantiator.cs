﻿using UnityEngine;

public class InstanceEventArgs
{
    public Vector2 Position { get; private set; }

    public GameObject Instance { get; private set; }

    public InstanceEventArgs(Vector2 initialPosition, GameObject instance)
    {
        Position = initialPosition;
        Instance = instance;
    }

    public InstanceEventArgs(GameObject instance)
        : this(new Vector2(), instance)
    { }
}

// TODO: maybe there is no need in this class
public class Instantiator : MonoBehaviour
{
    public GameObject Instantiate(GameObject instance, Vector2 position)
    {
        var rotation = Quaternion.identity;
        return Instantiate(instance, position, rotation);
    }
}