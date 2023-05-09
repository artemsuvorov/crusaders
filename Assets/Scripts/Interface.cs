using UnityEngine;

public class Interface : MonoBehaviour
{
    [SerializeField]
    private UnitController unitPrefab;

    public void OnCreateWorkerButtonPressed()
    {
        InstantiateUnit();
    } 

    public void OnCreateKnightButtonPressed()
    {
        InstantiateUnit();
    }

    private void InstantiateUnit()
    {
        var position = new Vector3(0.0f, 0.0f);
        var rotation = Quaternion.identity;
        Instantiate(unitPrefab, position, rotation);
    }
}
