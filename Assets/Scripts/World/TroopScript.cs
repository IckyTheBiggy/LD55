using System;
using System.Collections;
using Core;
using NnUtils.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace World
{
    public class TroopScript : MonoBehaviour, ISelectable, IDisplayable, IDamageable
    {
        public string Name;
        [HideInInspector] public int Health;
        public int MaxHealth;
        public Sprite Sprite;
        [SerializeField] private TroopAI _ai;
        [SerializeField] private LayerMask _relocationMask;

        private void Start()
        {
            Health = MaxHealth;
        }

        private Coroutine _selectedRoutine;
        private IEnumerator SelectedRoutine()
        {
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.R)) GameManager.Instance.IsRelocating = !GameManager.Instance.IsRelocating;
                if (GameManager.Instance.IsRelocating) Relocate();
                yield return null;
            }
        }

        private void Relocate()
        {
            var mainCamera = GameManager.Instance.MainCam;
            var ray = mainCamera.ScreenPointToRay(Misc.GetPointerPos());
            if (!Input.GetKeyUp(KeyCode.Mouse0) || Misc.IsPointerOverUI) return;
            if (!Physics.Raycast(ray, out var hit, 300, _relocationMask)) return;
            _ai.Relocate(hit.point);
            GameManager.Instance.IsRelocating = false;
        }
        
        #region ISelectable
        public void PointerEnter()
        {
        }

        public void PointerExit()
        {
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
            
            if (Health <= 0)
                Destroy(gameObject);
        }
    }
}
