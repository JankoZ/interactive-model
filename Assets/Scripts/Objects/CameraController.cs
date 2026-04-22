using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Objects
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float sensitivity = 3f;
        [SerializeField] private float zoomSpeed = 3f;
        [SerializeField] private float smoothTime = 0.2f;

        public Transform target;
        
        private float _targetX, _targetY, _targetDistance;
        private Vector3 _targetPanOffset = Vector3.zero;
        
        private float _currentX, _currentY, _currentDistance;
        private Vector3 _currentPivotPoint;

        private float _xVelocity, _yVelocity, _zoomVelocity;
        private Vector3 _pivotVelocity;

        private void Awake()
        {
            Vector3 angles = transform.eulerAngles;
            _targetX = _currentX = angles.y;
            _targetY = _currentY = angles.x;
            _targetDistance = _currentDistance = 5f;
            
            if (target) _currentPivotPoint = target.position;
        }

        private void Update()
        {
            if (!target) return;

            if (!EventSystem.current || !EventSystem.current.IsPointerOverGameObject())
            {
                CameraRotation();
                CameraMoving();
                CameraZoom();
            }
            
            _currentX = Mathf.SmoothDamp(_currentX, _targetX, ref _xVelocity, smoothTime);
            _currentY = Mathf.SmoothDamp(_currentY, _targetY, ref _yVelocity, smoothTime);
            
            _currentDistance = Mathf.SmoothDamp(_currentDistance, _targetDistance, ref _zoomVelocity, smoothTime);
            
            _currentPivotPoint = Vector3.SmoothDamp(_currentPivotPoint, target.position + _targetPanOffset,
                                                    ref _pivotVelocity, smoothTime);
            
            ApplyCameraTransform();
        }

        private void CameraRotation()
        {
            if (!Input.GetMouseButton(1)) return;
            
            _targetX += Input.GetAxis("Mouse X") * sensitivity;
            _targetY -= Input.GetAxis("Mouse Y") * sensitivity;
            _targetY = Math.Clamp(_targetY, -80, 80);
        }

        private void CameraMoving()
        {
            if (!Input.GetMouseButton(0)) return;
            
            float panY = Input.GetAxis("Mouse Y") * sensitivity * 0.05f;
            _targetPanOffset -= transform.up * panY;
        }

        private void CameraZoom()
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");

            if (!(Math.Abs(scroll) > 0.01f)) return;
            
            _targetDistance -= scroll * zoomSpeed;
            _targetDistance = Math.Clamp(_targetDistance, 1, 20);
        }

        private void ApplyCameraTransform()
        {
            Quaternion rotation = Quaternion.Euler(_currentY, _currentX, 0);
            transform.rotation = rotation;
            transform.position = rotation * new Vector3(0f, 0f, -_currentDistance) + _currentPivotPoint;
        }

        public void FocusOn(Transform newTarget)
        {
            target = newTarget;
            _targetPanOffset = Vector3.zero;
        }
    }
}
