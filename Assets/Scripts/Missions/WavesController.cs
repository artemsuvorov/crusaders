using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public enum SpawnSide
{

    Left,
    Right,
    Top,
    Bottom,
    All
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
    private float waveDelayInSeconds = 10.0f;

    [SerializeField]
    private int waveCount = 3;

    [SerializeField]
    private int[] enemiesPerWave;

    [SerializeField]
    private SpawnSide[] spawnSides;

    private int currentWaveIndex = 0;

    public event UnityAction WaveStarted;
    public event UnityAction WaveEnded;
    public event UnityAction AllWavesEnded;

    private void Start()
    {
        enemyController = GetComponent<Enemy>();
        StartCoroutine(StartWaveRoutine(waveDelayInSeconds));
    }

    private IEnumerator StartWaveRoutine(float delayInSeconds)
    {
        yield return AwaitWaveRoutine(delayInSeconds);
        
        WaveStarted?.Invoke();

        var enemyCount = enemiesPerWave[currentWaveIndex % enemiesPerWave.Length];
        var spawnSide = spawnSides[currentWaveIndex % spawnSides.Length];
        yield return SpawnEnemiesRoutine(enemyCount, spawnSide);
    }

    private IEnumerator SpawnEnemiesRoutine(
        int enemyCount, SpawnSide side)
    {
        if (enemyCount <= 0)
            yield break;

        enemyController.UnitDied += OnEnemyUnitDied;

        for (var i = 0; i < enemyCount; i++)
        {
            // TODO: select position smarter
            var position = Vector2.zero;
            enemyController.SpawnEnemyUnit(position);
            yield return new WaitForSeconds(1.0f);
        }
    }

    private void OnEnemyUnitDied()
    {
        if (enemyController.AliveEnemyCount > 0)
            return;

        WaveEnded?.Invoke();

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
