using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float _waves;
    [SerializeField] private float _timeBetweenWaves;
    [SerializeField] private float _timeBetweenEnemySpawns;
    [SerializeField] private List<GameObject> _enemyPrefabs;
    [SerializeField] private List<Transform> _enemySpawnPoints;
    [SerializeField] private int _initialEnemiesPerWave;
    [SerializeField] private int _maxEnemiesPerWave;
    [SerializeField] private float _enemySpawnRateIncrease;
    [SerializeField] private int _enemiesPerWave;
    
    private int _currentWave;
    
    private void Start()
    {
        StartCoroutine(SpawnWavesRoutine());
    }

    private IEnumerator SpawnWavesRoutine()
    {
        int enemiesPerWave = _initialEnemiesPerWave;
        
        while (_currentWave < _waves)
        {
            yield return new WaitForSecondsRealtime(_timeBetweenWaves);

            for (int i = 0; i < _enemiesPerWave; i++)
            {
                GameObject enemyPrefab = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Count)];
                Transform spawnPoint = _enemySpawnPoints[Random.Range(0, _enemySpawnPoints.Count)];
                
                GameObject enemyInstance = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                
                yield return new WaitForSecondsRealtime(_timeBetweenEnemySpawns);
            }

            _currentWave++;
            enemiesPerWave =
                Mathf.Min(_initialEnemiesPerWave + Mathf.FloorToInt(_enemySpawnRateIncrease * _currentWave),
                    _maxEnemiesPerWave);
        }
    }
}
