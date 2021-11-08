using System;
using UnityEngine;

namespace Sensors
{
    public class HoldPullSensor : MonoBehaviour
    {
        private PressState _pressState;

        public event Action Started;
        public event Action Holds;
        public event Action Finished;

        private void Update()
        {
            var isMouseButtonPressed = Input.GetMouseButton(0);
            if (isMouseButtonPressed && _pressState == PressState.None)
            {
                _pressState = PressState.Hold;
                Started?.Invoke();
            }
            else if (isMouseButtonPressed && _pressState == PressState.Hold)
            {
                Holds?.Invoke();
            }
            else if (!isMouseButtonPressed && _pressState == PressState.Hold)
            {
                _pressState = PressState.None;
                Finished?.Invoke();
            }
        }
    }

    public enum PressState
    {
        Hold,
        None
    }
}