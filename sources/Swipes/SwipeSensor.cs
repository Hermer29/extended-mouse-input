using Rubicks.Extensions;
using Rubicks.Extensions.Disposing;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rubicks.ExtendedPointerInput
{
    public class SwipeSensor : MonoBehaviour, IObservable<Vector2>
    {
        [SerializeField] private PointerPositions _pointerPositions = null;
        [SerializeField] private HoldPullSensor _holdPullSensor = null;
        private List<IObserver<Vector2>> _observers = new List<IObserver<Vector2>>();

        public event Action<Vector2> SwipeDelta;

        public IDisposable Subscribe(IObserver<Vector2> observer)
        {
            _observers.Add(observer);
            return Disposable.Create(() => _observers.Remove(observer));
        }

        private void Start()
        {
            if (_pointerPositions == null)
                throw new UnassignedReferenceException(nameof(_pointerPositions));

            if (_holdPullSensor == null)
                throw new UnassignedReferenceException(nameof(_holdPullSensor));

            _holdPullSensor.Pressed += OnDrag;
            _holdPullSensor.Finished += OnFinish;
        }

        private void OnFinish()
        {
            _observers.NotifyCompletion();
        }

        private void OnDrag()
        {
            SwipeDelta?.Invoke(_pointerPositions.DeltaPosition);
            _observers.Notify(_pointerPositions.DeltaPosition);
        }

        private void OnDisable()
        {
            _holdPullSensor.Pressed -= OnDrag;
        }
    }
}
