using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum MissionResult
{
    None,
    Win,
    Lose,
}

public abstract class Mission
{
    private MissionResult result = MissionResult.None;

    public string Name { get; private set; }

    public MissionResult Result
    { 
        get => result; 
        protected set
        {
            result = value;
            ResultChanged?.Invoke();
        } 
    }

    public event UnityAction ResultChanged;

    public abstract void OnEnemyDefeated();
    public abstract void OnEnemyTownhallDestroyed();
    public abstract void OnPlayerTownhallDestroyed();
}

public class DefenseMission : Mission
{
    public override void OnEnemyDefeated()
    {
        Result = MissionResult.Win;
    }

    public override void OnPlayerTownhallDestroyed()
    {
        Result = MissionResult.Lose;
    }

    public override void OnEnemyTownhallDestroyed() { }
}

public class MissionController : MonoBehaviour
{
    private WavesController wavesController;
    private DialogueController dialogueController;
    private DialogueContainer dialogueContainer;

    [SerializeField]
    private Player player;

    [SerializeField]
    private Enemy enemy;

    [SerializeField]
    private GameObject gameUiPanel, dialoguePanel;

    private readonly Mission mission = new DefenseMission();

    public event UnityAction<float> WaveAwaited;
    public event UnityAction WaveStarted;
    public event UnityAction WaveEnded;

    private void Start()
    {
        mission.ResultChanged += OnMissionResultChanged;
        //SceneManager.LoadScene("AfterGameWin");

        if (player.Faction.HasTownhall)
            player.Faction.Townhall.Died += mission.OnPlayerTownhallDestroyed;
        if (enemy.Faction.HasTownhall)
            enemy.Faction.Townhall.Died += mission.OnEnemyTownhallDestroyed;

        wavesController = enemy.GetComponent<WavesController>();
        if (wavesController is null)
            return;

        wavesController.WaveAwaited += (s) => WaveAwaited?.Invoke(s);
        wavesController.WaveStarted += () => WaveStarted?.Invoke();
        wavesController.WaveEnded += () => WaveEnded?.Invoke();
        wavesController.AllWavesEnded += mission.OnEnemyDefeated;

        dialogueController = dialoguePanel.GetComponent<DialogueController>();
        dialogueContainer = GetComponent<DialogueContainer>();

        dialogueController.DialogueStarted += OnDialogueStarted;
        dialogueController.DialogueEnded += OnDialogueEnded;

        OnGameStarted();
    }

    private void OnGameStarted()
    {
        var startDialogue = dialogueContainer.StartMissionDialogue;
        dialogueController.StartDialogue(startDialogue);
    }

    private void OnDialogueStarted()
    {
        gameUiPanel.SetActive(false);
        dialoguePanel.SetActive(true);
        Time.timeScale = 0.0f;
        Debug.Log("Dialogue Start");
    }

    private void OnDialogueEnded()
    {
        gameUiPanel.SetActive(true);
        dialoguePanel.SetActive(false);
        Time.timeScale = 1.0f;
        Debug.Log("Dialogue Ended");
    }

    private void OnMissionResultChanged()
    {
        if (mission.Result == MissionResult.Win)
            SceneManager.LoadScene("AfterGameWin");

        if (mission.Result == MissionResult.Lose)
            SceneManager.LoadScene("AfterGameLose");
    }
}
