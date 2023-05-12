using UnityEngine;

public class Instantiator : MonoBehaviour
{
    [SerializeField]
    private UnitController unitPrefab;

    [SerializeField]
    private GameObject sawmillPrefab;

    public void InstantiateUnit()
    {
        var position = new Vector3(0.0f, 0.0f);
        var rotation = Quaternion.identity;
        Instantiate(unitPrefab, position, rotation);
    }
}