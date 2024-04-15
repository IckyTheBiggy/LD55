using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _distance;
    [SerializeField] private float _sensitivity;
    [SerializeField] private float _zoomSpeed;
    [SerializeField] private float _minDistance;
    [SerializeField] private float _maxDistance;

    private bool _rotating;
    
    private float _xRotation;
    private float _yRotation;
    
    private void Start()
    {
        Vector3 angles = transform.eulerAngles;
        _xRotation = angles.y;
        _yRotation = angles.x;
        UpdatePosition();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _rotating = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            _rotating = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void UpdatePosition()
    {
        _xRotation += Input.GetAxis("Mouse X") * _sensitivity;
        _yRotation -= Input.GetAxis("Mouse Y") * _sensitivity;

        _yRotation = Mathf.Clamp(_yRotation, -90f, 90f);

        _distance -= Input.GetAxis("Mouse ScrollWheel") * _zoomSpeed;
        _distance = Mathf.Clamp(_distance, _minDistance, _maxDistance);

        Quaternion rotation = Quaternion.Euler(_yRotation, _xRotation, 0);
        Vector3 negativeDistance = new Vector3(0f, 0f, -_distance);
        Vector3 position = rotation * negativeDistance + (_target != null ? _target.position : Vector3.zero);

        transform.rotation = rotation;
        transform.position = position;
    }

    private void LateUpdate()
    {
        if (!_rotating)
            return;
        
        UpdatePosition();
    }
}
