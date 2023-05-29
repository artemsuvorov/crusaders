using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    [SerializeField]
    private Text resourcesText, missionText;

    [SerializeField]
    private Player player;

    [SerializeField]
    private MissionController missionController;

    [SerializeField]
    private GameObject healthBarPrefab;

    private Transform canvasTransform;

    private Button sawmillButton, quarryButton;

    public UnityEvent<InstanceEventArgs> CreateBuildingButtonPressed;
    public UnityEvent<InstanceEventArgs> CreateUnitButtonReleased;

    private void OnEnable()
    {
        canvasTransform = transform;

        sawmillButton = transform.Find("Sawmill Button").GetComponent<Button>();
        quarryButton = transform.Find("Quarry Button").GetComponent<Button>();

        player.BuildingBecameAvailable += OnBuildingBecameAvailable;
        player.BuildingBecameUnavailable += OnBuildingBecameUnavailable;

        OnResourceChanged(player.Resources);
        player.Resources.ResourceChanged += OnResourceChanged;

        LoadMissionEntities();

        missionController.WaveAwaited += (s) => 
            StartCoroutine(UpdateMissionInfoTextRoutine(s));
        missionController.WaveStarted += () => 
            missionText.text = $"The Wave is coming!";
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

    private void OnResourceChanged(Resources resources)
    {
        resourcesText.text = "Resources\r\n" + resources.ToString();
    }

    private void OnBuildingBecameAvailable(BuildingController building)
    {
        if (building is SawmillController)
            sawmillButton.interactable = true;
        if (building is QuarryController)
            quarryButton.interactable = true;
    }

    private void OnBuildingBecameUnavailable(BuildingController building)
    {
        if (building is SawmillController)
            sawmillButton.interactable = false;
        if (building is QuarryController)
            quarryButton.interactable = false;
    }

    private IEnumerator<YieldInstruction> UpdateMissionInfoTextRoutine(float delay)
    {
        const float Step = 1.0f;
        for (var rem = delay; rem > 0; rem -= Step)
        {
            missionText.text = $"Next Wave in: {rem:0}s";
            yield return new WaitForSeconds(Step);
        }
    }

    private void LoadMissionEntities()
    {
        var entities = FindObjectsOfType<EntityController>();

        foreach (var entity in entities)
        {
            var instance = Instantiate(healthBarPrefab, canvasTransform);
            var healthBar = instance.GetComponent<HealthBarController>();
            healthBar.Observe(entity);
        }
    }
}
