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

    private int[] _enemyTypesPerWave = { 1, 1, 1, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4 };
    
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

            List<int> availableEnemyTypes = new();
            for (int i = 0; i < availableEnemyTypes[_currentWave]; i++)
            {
                availableEnemyTypes.Add(i);
            }

            int enemiesToSpawn = Mathf.Min(_enemiesPerWave + _currentWave, 50);

            for (int i = 0; i < _enemiesPerWave; i++)
            {
                int randomIndex = Random.Range(0, availableEnemyTypes.Count);
                int enemyTypeIndex = availableEnemyTypes[randomIndex];

                GameObject enemyPrefab = _enemyPrefabs[enemyTypeIndex];
                Transform spawnPoint = _enemySpawnPoints[Random.Range(0, _enemySpawnPoints.Count)];
                
                Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                yield return new WaitForSecondsRealtime(_timeBetweenEnemySpawns);
            }

            _currentWave++;
        }
    }
}
