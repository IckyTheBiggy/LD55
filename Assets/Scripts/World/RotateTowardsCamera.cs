using System;
using Core;
using UnityEngine;

public class MoveTowardsCamera : MonoBehaviour
{
    private Transform _target;

    private void Start() => _target = GameManager.Instance.MainCam.transform;

    private void Update() => transform.LookAt(_target);
}
