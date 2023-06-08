using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PreGameController : MonoBehaviour
{
    public Button buttonMission;
    public Button buttonMenu;
    public GameObject popUpPanel;
    public Button buttonStartMission;
    public Button buttonHideMissionInfo;
    
    private readonly Dictionary<int, string> levelKeys = new Dictionary<int, string>()
    {
        { 0, "DefenseMission" },
        { 1, "Mission2" },
    };

    // Start is called before the first frame update
    void Start()
    {
        string missionName = buttonMission.name;
        var id = int.Parse(missionName.Substring(missionName.Length - 1));
        buttonMission.onClick.AddListener(() => ShowMissionInfo(id));
        buttonMenu.onClick.AddListener(ExitPreGame);
        buttonStartMission.onClick.AddListener(() => PlayMission(id));
        buttonHideMissionInfo.onClick.AddListener(HideMissionInfo);
    }

    private void HideMissionInfo()
    {
        popUpPanel.SetActive(false);
    }

    private void PlayMission(int id)
    {
        SceneManager.LoadScene(levelKeys[id]);
    }

    private void ExitPreGame()
    {
        SceneManager.LoadScene("Menu");
    }

    private void ShowMissionInfo(int id)
    {
        popUpPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
