using UnityEngine.Events;

public enum MissionType
{
    None,
    Defense,
    Offense
}

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
    public abstract void OnEnemyTownhallDestroyed(EntityController e);
    public abstract void OnPlayerTownhallDestroyed(EntityController e);
}

public class DefenseMission : Mission
{
    public override void OnEnemyDefeated()
    {
        Result = MissionResult.Win;
    }

    public override void OnPlayerTownhallDestroyed(EntityController e)
    {
        Result = MissionResult.Lose;
    }

    public override void OnEnemyTownhallDestroyed(EntityController e) { }
}

public class OffenseMission : Mission
{
    public override void OnEnemyDefeated() { }

    public override void OnPlayerTownhallDestroyed(EntityController e)
    {
        Result = MissionResult.Lose;
    }

    public override void OnEnemyTownhallDestroyed(EntityController e)
    {
        Result = MissionResult.Win;
    }
}
