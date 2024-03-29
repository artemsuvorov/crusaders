using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
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

    private RectTransform menu;
    private bool menuIsActive = false;

    private Text missionText;
    private Text goldText, woodText, stoneText;
    private RectTransform buildingsPanel, unitsPanel;
    private Button sawmillButton, quarryButton;
    private Button workerButton, knightButton;
    private Button woodSellButton, stoneSellButton;

    public UnityEvent<InstanceEventArgs> CreateBuildingButtonPressed;
    public UnityEvent<InstanceEventArgs> CreateUnitButtonReleased;
    public UnityEvent<Resource> ResourceSellButtonPressed;

    private void OnEnable()
    {
        barParentTansform = transform.Find("Bars");

        menu = transform.Find("Menu Panel").GetComponent<RectTransform>();

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
        workerButton = unitsPanel.transform.Find("Worker Button").GetComponent<Button>();
        knightButton = unitsPanel.transform.Find("Knight Button").GetComponent<Button>();

        woodSellButton = unitsPanel.transform
            .Find("Sell Wood Button").GetComponent<Button>();
        stoneSellButton = unitsPanel.transform
            .Find("Sell Stone Button").GetComponent<Button>();

        player.TownhallSelected += OnTownhallSelected;
        player.TownhallDeselected += OnTownhallDeselected;

        player.UnitBecameAvailable += OnUnitBecameAvailable;
        player.UnitBecameUnavailable += OnUnitBecameUnavailable;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ToggleMenu();
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
        FindObjectOfType<AudioManager>()?.Play("Button Knob");

        var args = new InstanceEventArgs(buildingBlueprint);
        CreateBuildingButtonPressed?.Invoke(args);
    }

    public void OnCreateUnitButtonReleased(GameObject unit)
    {
        FindObjectOfType<AudioManager>()?.Play("Button Knob");

        if (player.Faction.HasTownhall)
        {
            player.Faction.Townhall.CreateUnit(unit);
            return;
        }

        var args = new InstanceEventArgs(unit);
        CreateUnitButtonReleased?.Invoke(args);
    }

    public void OnResourceSellButtonPressed(ResourceComponent component)
    {
        FindObjectOfType<AudioManager>()?.Play("Button Knob");

        ResourceSellButtonPressed?.Invoke(component.Resource);
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

        woodSellButton.interactable = resources.Values[Resource.Wood] > 0;
        stoneSellButton.interactable = resources.Values[Resource.Stone] > 0;
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

    private void OnUnitBecameAvailable(UnitController unit)
    {
        if (unit is WorkerController)
            workerButton.interactable = true;
        if (unit is KnightController)
            knightButton.interactable = true;
    }

    private void OnUnitBecameUnavailable(UnitController unit)
    {
        if (unit is WorkerController)
            workerButton.interactable = false;
        if (unit is KnightController)
            knightButton.interactable = false;
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

    private void ToggleMenu()
    {
        if (menuIsActive)
        {
            menu.localScale = Vector3.zero;
            menuIsActive = false;
            Time.timeScale = 1.0f;
        } 
        else
        {
            menu.localScale = Vector3.one;
            menuIsActive = true;
            Time.timeScale = 0.0f;
        }
    }

    public void RestartMission()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("Menu");
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
