using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum SpawnSide
{
    Left,
    Right,
    Top,
    Bottom
}

public struct Wave
{
    public int EnemyCount { get; private set; }
    public SpawnSide SpawnSide { get; private set; }
}

public class WavesController : MonoBehaviour
{
    private Enemy enemyController;

    [SerializeField]
    private float initialDelayInSeconds = 2.0f;

    [SerializeField]
    private float waveDelayInSeconds = 10.0f;
    
    [SerializeField]
    private int waveCount = 3;

    [SerializeField]
    private int[] enemiesPerWave;

    [SerializeField]
    private SpawnSide[] spawnSides;

    private int currentWaveIndex = 0;

    private readonly Dictionary<SpawnSide, Transform> spawnPositions = new();

    public event UnityAction<float> WaveAwaited;
    public event UnityAction WaveStarted;
    public event UnityAction WaveEnded;
    public event UnityAction AllWavesEnded;

    private void Start()
    {
        enemyController = GetComponent<Enemy>();

        StartCoroutine(StartMissionWavesRoutine());

        spawnPositions[SpawnSide.Left]   = transform.Find("Left Position");
        spawnPositions[SpawnSide.Right]  = transform.Find("Right Position");
        spawnPositions[SpawnSide.Top]    = transform.Find("Top Position");
        spawnPositions[SpawnSide.Bottom] = transform.Find("Bottom Position");
    }

    private IEnumerator StartMissionWavesRoutine()
    {
        yield return new WaitForSeconds(initialDelayInSeconds);
        yield return StartWaveRoutine(waveDelayInSeconds);
    }

    private IEnumerator StartWaveRoutine(float delayInSeconds)
    {
        WaveAwaited?.Invoke(delayInSeconds);

        yield return AwaitWaveRoutine(delayInSeconds);
        
        FindObjectOfType<AudioManager>()?.Play("Warning");

        WaveStarted?.Invoke();
        enemyController.ViewRadius = 1000.0f;

        var enemyCount = enemiesPerWave[currentWaveIndex % enemiesPerWave.Length];
        var spawnSide = spawnSides[currentWaveIndex % spawnSides.Length];
        yield return SpawnEnemiesRoutine(enemyCount, spawnSide);
    }

    private IEnumerator SpawnEnemiesRoutine(int enemyCount, SpawnSide side)
    {
        if (enemyCount <= 0)
            yield break;

        enemyController.UnitDied += OnEnemyUnitDied;

        var spawnPosition = spawnPositions[side].position;
        var positions = PositionUtils.GetPositionsAround(spawnPosition, enemyCount);

        for (var i = 0; i < enemyCount; i++)
        {
            var position = positions[i % positions.Count];
            enemyController.SpawnEnemyUnit(position);
        }
    }

    private void OnEnemyUnitDied()
    {
        if (enemyController.AliveEnemyCount > 0)
            return;

        WaveEnded?.Invoke();
        enemyController.ViewRadius = 10.0f;

        enemyController.UnitDied -= OnEnemyUnitDied;
        if (currentWaveIndex + 1 >= waveCount)
        {
            AllWavesEnded?.Invoke();
            return;
        }

        currentWaveIndex++;
        StartCoroutine(StartWaveRoutine(waveDelayInSeconds));
    }

    private IEnumerator AwaitWaveRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
    }
}
