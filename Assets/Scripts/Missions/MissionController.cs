using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MissionController : MonoBehaviour
{
    private WavesController wavesController;
    private DialogueController dialogueController;
    private DialogueContainer dialogues;
    private Mission mission;

    [SerializeField]
    private Player player;

    [SerializeField]
    private Enemy enemy;

    [SerializeField]
    private GameObject gameUiPanel, dialoguePanel;

    [SerializeField]
    private MissionType type;

    public event UnityAction<float> WaveAwaited;
    public event UnityAction WaveStarted;
    public event UnityAction WaveEnded;

    private void Start()
    {
        mission = type switch
        {
            MissionType.Offense => new OffenseMission(),
            MissionType.Defense => new DefenseMission(),
            _ => throw new ArgumentException(
                $"Unexpected mission type {type}.")
        };

        mission.ResultChanged += OnMissionResultChanged;
        //SceneManager.LoadScene("AfterGameWin");

        if (player.Faction.HasTownhall)
            player.Faction.Townhall.Died += mission.OnPlayerTownhallDestroyed;
        if (enemy.Faction.HasTownhall)
            enemy.Faction.Townhall.Died += mission.OnEnemyTownhallDestroyed;

        wavesController = enemy.GetComponent<WavesController>();
        if (wavesController is not null)
        {
            wavesController.WaveAwaited += (s) => WaveAwaited?.Invoke(s);
            wavesController.WaveStarted += () => WaveStarted?.Invoke();
            wavesController.WaveEnded += () => WaveEnded?.Invoke();
            wavesController.AllWavesEnded += mission.OnEnemyDefeated;
        }

        dialogueController = dialoguePanel.GetComponent<DialogueController>();
        dialogues = GetComponent<DialogueContainer>();

        dialogueController.DialogueStarted += OnDialogueStarted;
        dialogueController.DialogueEnded += OnDialogueEnded;

        dialogueController.StartDialogue(dialogues.StartMissionDialogue);
    }

    private void OnDialogueStarted()
    {
        gameUiPanel.SetActive(false);
        dialoguePanel.SetActive(true);
        Time.timeScale = 0.0f;
    }

    private void OnDialogueEnded()
    {
        gameUiPanel.SetActive(true);
        dialoguePanel.SetActive(false);
        Time.timeScale = 1.0f;

        RedirectToNextSceneIfNeeded();
    }

    private void OnMissionResultChanged()
    {
        if (mission.Result == MissionResult.Win)
            dialogueController.StartDialogue(dialogues.VictoryDialogue);
            //SceneManager.LoadScene("AfterGameWin");

        if (mission.Result == MissionResult.Lose)
            dialogueController.StartDialogue(dialogues.DefeatDialogue);
        //SceneManager.LoadScene("AfterGameLose");
    }

    private void RedirectToNextSceneIfNeeded()
    {
        if (mission.Result == MissionResult.Win)
            SceneManager.LoadScene("AfterGameWin");

        if (mission.Result == MissionResult.Lose)
            SceneManager.LoadScene("AfterGameLose");
    }
}
