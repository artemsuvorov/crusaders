using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    [SerializeField]
    private Text resourcesText;

    public UnityEvent<InstanceEventArgs> CreateBuildingButtonPressed;
    public UnityEvent<InstanceEventArgs> CreateUnitButtonReleased;

    public void OnResourceIncreased(Resources resources)
    {
        resourcesText.text = "Resources\r\n" + resources.ToString();
    }

    public void OnCreateBuildingButtonPressed(GameObject buildingBlueprint)
    {
        var args = new InstanceEventArgs(buildingBlueprint);
        CreateBuildingButtonPressed?.Invoke(args);
    }

    public void OnCreateUnitButtonReleased(GameObject unit)
    {
        var args = new InstanceEventArgs(unit);
        CreateUnitButtonReleased?.Invoke(args);
    }

    public void OnResourceIncreaseButtonPressed(Resources resources)
    {
        resources.IncreaseResource(Resource.Wood, 100);
        resources.IncreaseResource(Resource.Stone, 100);
    }
}
