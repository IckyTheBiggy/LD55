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
    [SerializeField] private int _enemiesPerWave;
    
    private int _currentWave;
    
    private void Start()
    {
        StartCoroutine(SpawnWavesRoutine());
    }

    private IEnumerator SpawnWavesRoutine()
    {
        while (_currentWave < _waves)
        {
            yield return new WaitForSecondsRealtime(_timeBetweenWaves);

            int enemiesToSpawn = Mathf.Min(_enemiesPerWave + _currentWave, 50);

            for (int i = 0; i < _enemiesPerWave; i++)
            {
                GameObject enemyPrefab = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Count)];
                Transform spawnPoint = _enemySpawnPoints[Random.Range(0, _enemySpawnPoints.Count)];
                
                Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                yield return new WaitForSecondsRealtime(_timeBetweenEnemySpawns);
            }

            _currentWave++;
        }
    }
}
