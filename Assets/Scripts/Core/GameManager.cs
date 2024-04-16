using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public bool IsRelocating;
        public bool IsSpawning;
        public int SelectedTroop;
        public TroopSelection SelectedSelection; //I AM SO SORRY FOR THIS BANANA I RLY CBA TO LIVE :sob:
        
        [SerializeField] public float AnimationSpeed = 1;
        
        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameObject("Game Manager", typeof(GameManager)).GetComponent<GameManager>();
                return _instance;
            }
            private set
            {
                if (_instance != null && _instance != value)
                {
                    Destroy(value.gameObject);
                    return;
                }

                _instance = value;
            }
        }

        private void Awake() => Instance = GetComponent<GameManager>();

        private Camera _mainCam;
        public Camera MainCam => _mainCam = _mainCam == null ? Camera.main : _mainCam;

        private Transform _base;
        public Transform Base => _base = _base == null ? GameObject.FindGameObjectWithTag("Base").transform : _base;

        private SelectionManager _selectionManager;
        public SelectionManager SelectionManager => _selectionManager =
            _selectionManager == null ? GetComponent<SelectionManager>() : _selectionManager;

        public AudioManager _audioManager;
        
        public MoneyManager MoneyManager;
        public GameObject WaveCounter;

        [SerializeField] private int _maxTroopCount;
        
        [HideInInspector] public List<GameObject> Troops;
        [HideInInspector] public bool _canSpawnTroops;

        [SerializeField] private GameObject _endGameCamera;
        [SerializeField] private GameObject _mage;
        [SerializeField] private GameObject _dancingMage;
        [SerializeField] private List<GameObject> _objectsToDisable;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private GameObject _victoryUI;
        [SerializeField] private Button _playAgainButton;

        private void Update()
        {
            if (Troops.Count < _maxTroopCount)
                _canSpawnTroops = true;
            else
                _canSpawnTroops = false;
        }

        public void EndGame()
        {
            _enemySpawner.enabled = false;

            foreach (var disableObject in _objectsToDisable)
                disableObject.SetActive(false);
            
            _mainCam.gameObject.SetActive(false);
            _endGameCamera.SetActive(true);
            _endGameCamera.GetComponent<Animator>().Play("CameraAnimation");
            _mage.SetActive(false);
            _dancingMage.SetActive(true);
            _victoryUI.SetActive(true);
            
            _playAgainButton.onClick.AddListener(RestartLevel);
        }

        private void RestartLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
