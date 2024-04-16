using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using NnUtils.Scripts.UI;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private NBar _currentWaveText;
    
    [SerializeField] private float _waves;
    [SerializeField] private float _timeBetweenWaves;
    [SerializeField] private float _timeBetweenEnemySpawns;
    [SerializeField] private List<GameObject> _enemyPrefabs;
    [SerializeField] private List<Transform> _enemySpawnPoints;
    [SerializeField] private int _initialEnemiesPerWave;
    [SerializeField] private int _maxEnemiesPerWave;
    [SerializeField] private float _enemySpawnRateIncrease;
    [SerializeField] private int _enemyHealthIncrease;
    [SerializeField] private int _enemyDamageIncrease;
    
    private int _currentWave;
    
    private void Start()
    {
        StartCoroutine(SpawnWavesRoutine());
    }

    private void Update()
    {
        if (_currentWave >= 20)
            GameManager.Instance.EndGame();
    }

    private IEnumerator SpawnWavesRoutine()
    {
        int enemiesPerWave = _initialEnemiesPerWave;
        
        while (_currentWave < _waves)
        {
            yield return new WaitForSecondsRealtime(_timeBetweenWaves);

            for (int i = 0; i < enemiesPerWave; i++)
            {
                GameObject enemyPrefab = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Count)];
                Transform spawnPoint = _enemySpawnPoints[Random.Range(0, _enemySpawnPoints.Count)];
                
                GameObject enemyInstance = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                
                enemyInstance.GetComponent<EnemyScript>().IncreaseHealthAmount(_enemyHealthIncrease * _currentWave);
                enemyInstance.GetComponent<EnemyScript>().IncreaseDamageAmount(_enemyDamageIncrease * _currentWave);
                
                yield return new WaitForSecondsRealtime(_timeBetweenEnemySpawns);
            }

            _currentWave++;
            enemiesPerWave =
                Mathf.Min(_initialEnemiesPerWave + Mathf.FloorToInt(_enemySpawnRateIncrease * _currentWave),
                    _maxEnemiesPerWave);
            UpdateCurrentWaveText(_currentWave);
            GameManager.Instance.MoneyManager.AddMoney(400);
        }
    }

    private void UpdateCurrentWaveText(int wave)
    {
        _currentWaveText.Value = wave;
    }
}
