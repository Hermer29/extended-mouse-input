using System;
using UnityEngine;

namespace Sensors
{
    public class MousePositionWorldPointSensor : HoldPullSensor
    {
        private Camera _activeCamera;

        private Vector3 _pressMousePosition;
        private Vector3 _currentMousePosition;
        private Vector3 _deltaMousePosition;
        private Vector3 _lastTickPosition;
        private Vector3 _fromLastTickDelta;

        public Vector3 PressMousePosition => _pressMousePosition;
        public Vector3 CurrentMousePosition => _currentMousePosition;
        public Vector3 DeltaMousePosition => _deltaMousePosition;
        public Vector3 DeltaThisTick => _fromLastTickDelta;

        public event Action PointerMoved;

        private void Start()
        {
            _activeCamera = Camera.main;

            Started += OnPress;
            Holds += OnHold;
            Finished += OnFinish;
        }

        private void OnPress()
        {
            _pressMousePosition = _activeCamera.ScreenToWorldPoint(Input.mousePosition);
            _lastTickPosition = _pressMousePosition;
        }

        private void OnHold()
        {
            _currentMousePosition = _activeCamera.ScreenToWorldPoint(Input.mousePosition);
            _deltaMousePosition = _currentMousePosition - _pressMousePosition;
            _fromLastTickDelta = _currentMousePosition - _lastTickPosition;
            _lastTickPosition = _currentMousePosition;

            if (_fromLastTickDelta != Vector3.zero)
            {
                PointerMoved?.Invoke();
            }
        }

        private void OnFinish()
        {
            _pressMousePosition = default;
            _currentMousePosition = default;
            _deltaMousePosition = default;
        }

        private void OnDisable()
        {
            Started -= OnPress;
            Holds -= OnHold;
            Finished -= OnFinish;
        }
    }
}
