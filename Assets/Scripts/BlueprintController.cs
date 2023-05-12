using UnityEngine;

public class BlueprintEventArgs
{
    public Vector2 InitialPosition { get; private set; }

    public GameObject Blueprint { get; private set; }

    public BlueprintEventArgs(Vector2 initialPosition, GameObject blueprint)
    {
        InitialPosition = initialPosition;
        Blueprint = blueprint;
    }

    public BlueprintEventArgs(GameObject blueprint)
        : this(new Vector2(), blueprint)
    { }
}

public class BlueprintController : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    void Update()
    {
        var position = GetMousePositionInWorld();
        transform.position = position;

        if (Input.GetMouseButtonDown(0))
        {
            var rotation = Quaternion.identity;
            Instantiate(prefab, position, rotation);
            Destroy(gameObject);
        }
    }

    private Vector2 GetMousePositionInWorld()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
