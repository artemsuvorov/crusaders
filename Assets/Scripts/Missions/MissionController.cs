using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    [SerializeField]
    private Player player;

    [SerializeField]
    private Enemy enemy;

    private readonly Mission mission = new DefenseMission();

    private void Start()
    {
        mission.ResultChanged += OnMissionResultChanged;

        if (player.Faction.HasTownhall)
            player.Faction.Townhall.Died += mission.OnPlayerTownhallDestroyed;
        if (enemy.Faction.HasTownhall)
            enemy.Faction.Townhall.Died += mission.OnEnemyTownhallDestroyed;
    }

    private void OnMissionResultChanged()
    {
        if (mission.Result == MissionResult.Win)
        {
            Time.timeScale = 0;
            Debug.Log("You won!");
        }

        if (mission.Result == MissionResult.Lose)
        {
            Time.timeScale = 0;
            Debug.Log("You lost!");
        }
    }
}
