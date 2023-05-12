using UnityEngine;

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

public class Instantiator : MonoBehaviour
{
    [SerializeField]
    private UnitController unitPrefab;

    [SerializeField]
    private GameObject sawmillPrefab;

    public void Instantiate(InstanceEventArgs args)
    {
        var rotation = Quaternion.identity;
        Instantiate(args.Instance, args.Position, rotation);
    }
}