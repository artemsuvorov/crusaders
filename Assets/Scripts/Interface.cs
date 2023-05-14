using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    [SerializeField]
    private Text resourcesText;

    [SerializeField]
    private GameObject healthBarPrefab;

    private Transform canvasTransform;

    public UnityEvent<InstanceEventArgs> CreateBuildingButtonPressed;
    public UnityEvent<InstanceEventArgs> CreateUnitButtonReleased;

    private void Start()
    {
        canvasTransform = transform;
    }

    public void OnResourceIncreased(Resources resources)
    {
        resourcesText.text = "Resources\r\n" + resources.ToString();
    }

    public void OnEntityCreated(InstanceEventArgs args)
    {
        var entity = args.Instance.GetComponent<EntityController>();
        if (entity is null)
            return;

        var instance = Instantiate(healthBarPrefab, canvasTransform);
        var healthBar = instance.GetComponent<HealthBarController>();
        healthBar.Observe(entity);
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
