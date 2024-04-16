using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using NnUtils.Scripts;
using UnityEngine;

namespace World
{
    public class SpawnScript : MonoBehaviour
    {
        private static readonly int Spawn = Animator.StringToHash("Spawn");
        [SerializeField] private Animator _mageAnimator;
        [SerializeField] private List<GameObject> _troopPrefabs;
        [SerializeField] private List<int> _troopCosts;
        [SerializeField] private float _spawnTime = 5;
        [SerializeField] private LayerMask _selectionLayerMask;
        

        private void Update()
        {
            if (GameManager.Instance.IsRelocating) return;
            if (GameManager.Instance.IsSpawning) return;
            if (Input.GetKeyDown(KeyCode.Mouse1)) Misc.RestartCoroutine(this, ref _spawnRoutine, SpawnRoutine());
        }

        private Coroutine _spawnRoutine;
        private IEnumerator SpawnRoutine()
        {
            var hit = GetSpawnHit();
            if (hit == null) yield break;

            if (GameManager.Instance.MoneyManager.Money >= _troopCosts[GameManager.Instance.SelectedTroop] && GameManager.Instance._canSpawnTroops)
            {
                GameManager.Instance.IsSpawning = true;
                _mageAnimator.SetTrigger(Spawn);
                var troop =
                    Instantiate(_troopPrefabs[GameManager.Instance.SelectedTroop], hit.Value.point, Quaternion.identity);
                GameManager.Instance.Troops.Add(troop);
                GameManager.Instance.MoneyManager.SubtractMoney(_troopCosts[GameManager.Instance.SelectedTroop]);
                yield return new WaitForSeconds(_spawnTime);
                GameManager.Instance.IsSpawning = false;
                _spawnRoutine = null;
            }
        }

        private RaycastHit? GetSpawnHit()
        {
            if (Misc.IsPointerOverUI) return null;
            var mainCamera = GameManager.Instance.MainCam;
            var pointerPos = Misc.GetPointerPos();
            var ray = mainCamera.ScreenPointToRay(pointerPos);
            return Physics.Raycast(ray, out var hit, 300, _selectionLayerMask) ?
                hit : null;
        }
    }
}
