using UnityEngine;

public class Instantiator : MonoBehaviour
{
    [SerializeField]
    private UnitController unitPrefab;

    [SerializeField]
    private GameObject sawmillPrefab;

    public void Blueprint(BlueprintEventArgs args)
    {
        Instantiate(args.Blueprint);
    }

    public void Instantiate(GameObject instance)
    {
        Instantiate(new Vector2(0.0f, 0.0f), instance);
    }

    public void Instantiate(Vector2 position, GameObject instance)
    {
        var rotation = Quaternion.identity;
        Instantiate(instance, position, rotation);
    }
}