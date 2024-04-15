using System;
using System.Collections;
using Assets.NnUtils.Scripts;
using NnUtils.Scripts;
using UnityEngine;

namespace Core
{
    public class CameraScript : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 1;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse2))
            {
                Misc.StopCoroutine(this, ref _lerpCamRoutine);
                Misc.RestartCoroutine(this, ref _rotateRoutine, RotateRoutine());
            }
            if (Input.GetKeyUp(KeyCode.Mouse2)) Misc.StopCoroutine(this, ref _rotateRoutine);
        }

        private Coroutine _rotateRoutine;
        private IEnumerator RotateRoutine()
        {
            var startPos = Misc.GetPointerPos();
            var delta = (Misc.GetPointerPos() - startPos) * _rotationSpeed;
            var startRot = transform.localRotation.eulerAngles;
            startRot.x -= startRot.x > 180 ? 360 : 0;
            startRot.y -= startRot.y > 180 ? 360 : 0;
            
            while (true)
            {
                var newDelta = (Misc.GetPointerPos() - startPos) * _rotationSpeed;
                if (delta == newDelta) { yield return null; continue; }
                delta = newDelta;
                
                Vector3 targetRot = new(startRot.x - newDelta.y, startRot.y + newDelta.x, startRot.z);
                targetRot.x = Mathf.Clamp(targetRot.x, -30, 75);
                targetRot.y = Mathf.Clamp(targetRot.y, -60, 60);
                
                Misc.RestartCoroutine(this, ref _lerpCamRoutine, LerpCamRoutine(targetRot));
                yield return null;
            }
        }

        private Coroutine _lerpCamRoutine;
        private IEnumerator LerpCamRoutine(Vector3 targetRotEuler)
        {
            var startRotEuler = transform.localRotation.eulerAngles;
            startRotEuler.z = targetRotEuler.z = 0;
            var startRot = Quaternion.Euler(startRotEuler);
            var targetRot = Quaternion.Euler(targetRotEuler);
            float lerpPos = 0;
            
            while (lerpPos < 1)
            {
                var t = Misc.UpdateLerpPos(ref lerpPos, 1f, false, Easings.Types.ExpoOut);
                transform.localRotation = Quaternion.Lerp(startRot, targetRot, t);
                yield return null;
            }
        }
    }
}
