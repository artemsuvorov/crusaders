using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    [SerializeField]
    private Text resourcesText;

    public UnityEvent<BlueprintEventArgs> CreateBuildingButtonPressed;
    public UnityEvent<GameObject> CreateUnitButtonReleased;

    public void OnResourceIncreased(Resources resources)
    {
        resourcesText.text = "Resources\r\n" + resources.ToString();
    }

    public void OnCreateBuildingButtonPressed(GameObject buildingBlueprint)
    {
        var args = new BlueprintEventArgs(buildingBlueprint);
        CreateBuildingButtonPressed?.Invoke(args);
    }

    public void OnCreateUnitButtonReleased(GameObject unit)
    {
        CreateUnitButtonReleased?.Invoke(unit);
    }

    public void OnResourceIncreaseButtonPressed(Resources resources)
    {
        resources.IncreaseResource(Resource.Wood, 100);
        resources.IncreaseResource(Resource.Stone, 100);
    }
}
