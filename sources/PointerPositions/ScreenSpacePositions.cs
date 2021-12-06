using System;
using UnityEngine;

namespace Rubicks.ExtendedPointerInput
{
    [RequireComponent(typeof(HoldPullSensor))]
    public class ScreenSpacePositions : PointerPositions
    {
        private Vector3 _currentMousePosition;
        private Vector3 _pressMousePosition;
        private Vector3 _deltaMousePosition;
        private Vector3 _lastTickPosition;
        private Vector3 _fromLastTickDelta;
        private Vector3 _mousePositionInPercent;

        public override event Action PointerMoved;

        public override Vector3 PressMousePosition => _pressMousePosition;

        public override Vector3 CurrentMousePosition => _currentMousePosition;

        public override Vector3 DeltaPosition => _deltaMousePosition;

        public override Vector3 DeltaThisTick => _fromLastTickDelta;

        public Vector3 MousePositionInPercent => _mousePositionInPercent;

        protected sealed override void OnHold()
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

        protected sealed override void OnFinish()
        {
            _pressMousePosition = default;
            _currentMousePosition = default;
            _deltaMousePosition = default;
        }

        protected sealed override void OnStart()
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
