using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    [SerializeField]
    private Player player;

    [SerializeField]
    private MissionController missionController;

    [SerializeField]
    private GameObject healthBarPrefab;

    private Transform barParentTansform;

    private Text missionText;
    private Text goldText, woodText, stoneText;
    private RectTransform buildingsPanel, unitsPanel;
    private Button sawmillButton, quarryButton;

    public UnityEvent<InstanceEventArgs> CreateBuildingButtonPressed;
    public UnityEvent<InstanceEventArgs> CreateUnitButtonReleased;

    private void OnEnable()
    {
        barParentTansform = transform.Find("Bars");

        var gameUi = transform.Find("Game UI");

        missionText = gameUi.Find("Mission Text").GetComponent<Text>();

        var resourcesPanel = gameUi.Find("Resources Panel");
        goldText = resourcesPanel.Find("Gold Number").GetComponent<Text>();
        woodText = resourcesPanel.Find("Wood Number").GetComponent<Text>();
        stoneText = resourcesPanel.Find("Stone Number").GetComponent<Text>();

        buildingsPanel = gameUi.Find("Buildings Panel").GetComponent<RectTransform>();
        sawmillButton = buildingsPanel.transform.Find("Sawmill Button").GetComponent<Button>();
        quarryButton = buildingsPanel.transform.Find("Quarry Button").GetComponent<Button>();

        unitsPanel = gameUi.Find("Units Panel").GetComponent<RectTransform>();

        player.TownhallSelected += OnTownhallSelected;
        player.TownhallDeselected += OnTownhallDeselected;

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

        var instance = Instantiate(healthBarPrefab, barParentTansform);
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
        goldText.text = resources.Values[Resource.Gold].ToString();
        woodText.text = resources.Values[Resource.Wood].ToString();
        stoneText.text = resources.Values[Resource.Stone].ToString();
    }

    private void OnTownhallSelected()
    {
        buildingsPanel.localScale = Vector3.zero;
        unitsPanel.localScale = Vector3.one;
    }

    private void OnTownhallDeselected()
    {
        buildingsPanel.localScale = Vector3.one;
        unitsPanel.localScale = Vector3.zero;
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
            var instance = Instantiate(healthBarPrefab, barParentTansform);
            var healthBar = instance.GetComponent<HealthBarController>();
            healthBar.Observe(entity);
        }
    }
}
