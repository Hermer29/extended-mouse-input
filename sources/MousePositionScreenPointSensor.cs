using System;
using UnityEngine;

namespace Sensors
{
    public class MousePositionScreenPointSensor : HoldPullSensor
    {
        private Vector3 _currentMousePosition;
        private Vector3 _pressMousePosition;
        private Vector3 _deltaMousePosition;
        private Vector3 _lastTickPosition;
        private Vector3 _fromLastTickDelta;
        private Vector3 _mousePositionInPercent;

        public Vector3 PressMousePosition => _pressMousePosition;
        public Vector3 CurrentMousePosition => _currentMousePosition;
        public Vector3 DeltaMousePosition => _deltaMousePosition;
        public Vector3 DeltaThisTick => _fromLastTickDelta;
        public Vector3 MousePositionInPercent => _mousePositionInPercent;

        public event Action PointerMoved;

        private void Start()
        {
            Holds += OnHold;
            Finished += OnHoldFinish;
            Started += OnHoldStart;
        }

        private void OnHold()
        {
            _currentMousePosition = Input.mousePosition;
            _deltaMousePosition = _currentMousePosition - _pressMousePosition;
            _fromLastTickDelta = _currentMousePosition - _lastTickPosition;
            _lastTickPosition = _currentMousePosition;

            if(_fromLastTickDelta != Vector3.zero)
            {
                PointerMoved?.Invoke();
            }

            _mousePositionInPercent =
                new Vector3(
                        Input.mousePosition.x / Screen.width,
                        Input.mousePosition.y / Screen.height,
                        0
                    );
        }

        private void OnHoldFinish()
        {
            _pressMousePosition = default;
            _currentMousePosition = default;
            _deltaMousePosition = default;
        }

        private void OnHoldStart()
        {
            _currentMousePosition = 
                _lastTickPosition = 
                _pressMousePosition = 
                    Input.mousePosition;

            _mousePositionInPercent =
                new Vector3(
                        Input.mousePosition.x / Screen.width,
                        Input.mousePosition.y / Screen.height,
                        0
                    );
        }
    }
}
