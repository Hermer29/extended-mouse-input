using Rubicks.Extensions;
using Rubicks.Extensions.Disposing;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rubicks.ExtendedPointerInput
{
    public class HoldPullSensor : MonoBehaviour, IObservable<PressState>
    {
        private PressState _pressState;
        private List<IObserver<PressState>> _observers = new List<IObserver<PressState>>();

        public event Action Started;
        public event Action Pressed;
        public event Action Finished;

        public IDisposable Subscribe(IObserver<PressState> observer)
        {
            _observers.Add(observer);
            return Disposable.Create(() => _observers.Remove(observer));
        }

        private void Update()
        {
            var isMouseButtonPressed = Input.GetMouseButton(0);
            if (isMouseButtonPressed && _pressState == PressState.None)
            {
                _pressState = PressState.Hold;
                Started?.Invoke();
                Notify(PressState.Started);
            }
            else if (isMouseButtonPressed && _pressState == PressState.Hold)
            {
                Pressed?.Invoke();
                Notify(PressState.Hold);
            }
            else if (!isMouseButtonPressed && _pressState == PressState.Hold)
            {
                _pressState = PressState.None;
                Finished?.Invoke();
                Notify(PressState.Canceled);
            }
        }

        private void Notify(PressState state)
        {
            _observers.Notify(state);
        }
    }
}