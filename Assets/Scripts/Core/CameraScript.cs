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
            if (Input.GetKeyDown(KeyCode.Mouse2)) Misc.RestartCoroutine(this, ref _rotateRoutine, RotateRoutine());
            if (Input.GetKeyUp(KeyCode.Mouse2)) Misc.StopCoroutine(this, ref _rotateRoutine);
        }

        private Coroutine _rotateRoutine;
        private IEnumerator RotateRoutine()
        {
            var startPos = Misc.GetPointerPos();
            var startRot = transform.localRotation.eulerAngles;
            Vector2 delta = (Misc.GetPointerPos() - startPos) * _rotationSpeed;
            while (true)
            {
                var newDelta = (Misc.GetPointerPos() - startPos) * _rotationSpeed;
                if (delta == newDelta) { yield return null; continue; }
                delta = newDelta;
                
                Vector3 targetRot = new(startRot.x - delta.y, startRot.y + delta.x, startRot.z);
                targetRot.x = Mathf.Clamp(targetRot.x, -75, 75);
                Misc.RestartCoroutine(this, ref _lerpCamRoutine, LerpCamRoutine(Quaternion.Euler(targetRot)));
                //transform.rotation = Quaternion.Euler(targetRot);
                yield return null;
            }
        }

        private Coroutine _lerpCamRoutine;
        private IEnumerator LerpCamRoutine(Quaternion targetRot)
        {
            var startRot = transform.localRotation;
            float lerpPos = 0;
            while (lerpPos < 1)
            {
                var t = Misc.UpdateLerpPos(ref lerpPos, 1f, false, Easings.Types.ExpoOut);
                transform.localRotation = Quaternion.Lerp(startRot, targetRot, t);
                var euler = transform.localRotation.eulerAngles;
                euler.z = 0;
                transform.localRotation = Quaternion.Euler(euler);
                yield return null;
            }
        }
    }
}
