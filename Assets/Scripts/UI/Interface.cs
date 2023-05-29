using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    [SerializeField]
    private Text resourcesText, missionText;

    [SerializeField]
    private GameObject healthBarPrefab;

    [SerializeField]
    private MissionController missionController;

    private Transform canvasTransform;

    public UnityEvent<InstanceEventArgs> CreateBuildingButtonPressed;
    public UnityEvent<InstanceEventArgs> CreateUnitButtonReleased;

    private void Start()
    {
        canvasTransform = transform;
        LoadMissionEntities();

        missionController.WaveAwaited += (s) => 
            StartCoroutine(UpdateMissionInfoTextRoutine(s));
        missionController.WaveStarted += () => 
            missionText.text = $"The Wave is coming!";
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

    private IEnumerator<YieldInstruction> UpdateMissionInfoTextRoutine(float delay)
    {
        const float Step = 1.0f;
        for (var rem = delay; rem > 0; rem -= Step)
        {
            missionText.text = $"Next Wave in: {rem:0}s";
            yield return new WaitForSeconds(Step);
        }
    }
}
