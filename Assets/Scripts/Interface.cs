using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    [SerializeField]
    private Text resourcesText;

    [SerializeField]
    private UnitController unitPrefab;

    public void OnResourceIncreaseButtonPressed(Resources resources)
    {
        resources.IncreaseResource(Resource.Wood, 100);
        resources.IncreaseResource(Resource.Stone, 100);
    }

    public void OnCreateWorkerButtonPressed()
    {
        InstantiateUnit();
    } 

    public void OnCreateKnightButtonPressed()
    {
        InstantiateUnit();
    }

    public void OnResourceIncreased(Resources resources)
    {
        resourcesText.text = "Resources\r\n" + resources.ToString();
    }

    private void InstantiateUnit()
    {
        var position = new Vector3(0.0f, 0.0f);
        var rotation = Quaternion.identity;
        Instantiate(unitPrefab, position, rotation);
    }
}
