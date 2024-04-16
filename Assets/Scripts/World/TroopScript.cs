using System.Collections;
using Assets.NnUtils.Scripts;
using Core;
using NnUtils.Scripts;
using UnityEngine;
using UnityEngine.AI;

namespace World
{
    public class TroopScript : MonoBehaviour, ISelectable, IDisplayable, IDamageable
    {
        public string Name;
        [HideInInspector] public int Health;
        public int MaxHealth;
        public Sprite Sprite;
        public bool IsSpawning;
        [SerializeField] private GameObject _meshObject;
        [SerializeField] private TroopAI _ai;
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] private LayerMask _relocationMask;
        [SerializeField] private ParticleSystem _spawnParticles;
        [SerializeField] private ParticleSystem _hitParticles;
        [SerializeField] private ParticleSystem _destroyParticles;

        private void Start()
        {
            Health = MaxHealth;
            IsSpawning = true;
            _ai.enabled = false;
            _navMeshAgent.enabled = false;
            Misc.RestartCoroutine(this, ref _spawnRoutine, SpawnRoutine());
        }

        private Coroutine _spawnRoutine;
        private IEnumerator SpawnRoutine()
        {
            float lerpPos = 0;
            var targetPos = _meshObject.transform.localPosition;
            var startPos = targetPos;
            startPos.y -= 4;
            _spawnParticles.Play();
            
            while (lerpPos < 1)
            {
                var t = Misc.UpdateLerpPos(ref lerpPos, 3, false, Easings.Types.CubicOut);
                _meshObject.transform.localPosition = Vector3.Lerp(startPos, targetPos, t);
                yield return null;
            }
            
            _spawnParticles.Stop();
            _ai.enabled = true;
            _navMeshAgent.enabled = true;
            IsSpawning = false;
            _spawnRoutine = null;
        }
        
        private Coroutine _selectedRoutine;
        private IEnumerator SelectedRoutine()
        {
            if (IsSpawning) yield break;
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0)) GameManager.Instance.IsRelocating = !GameManager.Instance.IsRelocating;
                if (GameManager.Instance.IsRelocating) Relocate();
                yield return null;
            }
        }

        private void Relocate()
        {
            var mainCamera = GameManager.Instance.MainCam;
            var ray = mainCamera.ScreenPointToRay(Misc.GetPointerPos());
            if (Misc.IsPointerOverUI) return;
            if (!Physics.Raycast(ray, out var hit, 300, _relocationMask)) return;
            _ai.Relocate(hit.point);
            GameManager.Instance.IsRelocating = false;
        }

        #region ISelectable
        public void PointerEnter()
        {
            GameManager.Instance.WaveCounter.SetActive(false);
        }

        public void PointerExit()
        {
            GameManager.Instance.WaveCounter.SetActive(true);
        }

        public void PointerDown()
        {
        }

        public void Pointer()
        {
        }

        public void PointerUp()
        {
        }

        public void Select()
        {
            Misc.RestartCoroutine(this, ref _selectedRoutine, SelectedRoutine());
        }


        public void Deselect()
        {
            if (_selectedRoutine != null)
            {
                StopCoroutine(_selectedRoutine);
                _selectedRoutine = null;
            }
        }

        #endregion

        #region IDisplayable

        public string GetName() => Name;
        public int GetHealth() => Health;
        public int GetMaxHealth() => MaxHealth;
        public Sprite GetSprite() => Sprite;

        #endregion

        public void Damage(int damage)
        {
            Health -= damage;
            Instantiate(_hitParticles, transform.position, Quaternion.identity);

            if (Health <= 0)
            {
                Instantiate(_destroyParticles, transform.position, Quaternion.identity);
                GameManager.Instance.Troops.Remove(gameObject);
                Destroy(gameObject);
            }
        }
    }
}
