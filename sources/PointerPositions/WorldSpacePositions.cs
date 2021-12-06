using System;
using UnityEngine;

namespace Rubicks.ExtendedPointerInput
{
    [RequireComponent(typeof(HoldPullSensor))]
    public class WorldSpacePositions : PointerPositions
    {
        private Camera _activeCamera;
        private Vector3 _pressMousePosition;
        private Vector3 _currentMousePosition;
        private Vector3 _deltaMousePosition;
        private Vector3 _lastTickPosition;
        private Vector3 _fromLastTickDelta;

        public override event Action PointerMoved;

        public override Vector3 PressMousePosition => _pressMousePosition;
        public override Vector3 CurrentMousePosition => _currentMousePosition;
        public override Vector3 DeltaPosition => _deltaMousePosition;
        public override Vector3 DeltaThisTick => _fromLastTickDelta;

        public sealed override void Initialize()
        {
            _activeCamera = Camera.main;
        }

        protected sealed override void OnStart()
        {
            _pressMousePosition = _activeCamera.ScreenToWorldPoint(Input.mousePosition);
            _lastTickPosition = _pressMousePosition;
        }

        protected sealed override void OnHold()
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

        protected sealed override void OnFinish()
        {
            _pressMousePosition = default;
            _currentMousePosition = default;
            _deltaMousePosition = default;
        }
    }
}
