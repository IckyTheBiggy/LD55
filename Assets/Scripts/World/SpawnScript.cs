using System;
using System.Collections;
using NnUtils.Scripts;
using UnityEngine;

namespace World
{
    public class SpawnScript : MonoBehaviour
    {
        [SerializeField] private Animator _mageAnimator;
        private static readonly int Spawn = Animator.StringToHash("Spawn");

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0)) Misc.RestartCoroutine(this, ref _spawnRoutine, SpawnRoutine());
        }

        private Coroutine _spawnRoutine;
        private IEnumerator SpawnRoutine()
        {
            _mageAnimator.SetTrigger(Spawn);
            yield return null;
        }
    }
}
