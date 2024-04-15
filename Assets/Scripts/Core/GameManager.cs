using UI;
using UnityEngine;

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

        public MoneyManager MoneyManager;
    }
}
