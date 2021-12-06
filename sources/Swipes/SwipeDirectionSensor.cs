using Rubicks.Extensions;
using Rubicks.Extensions.Disposing;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rubicks.ExtendedPointerInput
{
    [RequireComponent(typeof(SwipeSensor))]
    public class SwipeDirectionSensor : MonoBehaviour, IObservable<SwipeInfo>
    {
        private SwipeSensor _sensor;
        private IDisposable _subscription;
        private List<IObserver<SwipeInfo>> _observers = new List<IObserver<SwipeInfo>>();
        private int _lastDetectedSwipe = 0;

        public IDisposable Subscribe(IObserver<SwipeInfo> observer)
        {
            _observers.Add(observer);
            return Disposable.Create(() => _observers.Remove(observer));
        }

        private void Start()
        {
            _sensor = GetComponent<SwipeSensor>();

            _subscription = _sensor.Subscribe(OnSwipeContinue, onCompleted: OnSwipeEnded);
        }

        private void OnSwipeContinue(Vector2 direction)
        {
            var absVector = direction.Abs();
            if(absVector.x > absVector.y)
            {
                OnHorizontalSwipe(direction);
                return;
            }
            OnVerticalSwipe(direction);
        }

        private void OnSwipeEnded()
        {
            _observers.NotifyCompletion();
            _lastDetectedSwipe++;
        }

        private void OnHorizontalSwipe(Vector2 direction)
        {
            if (direction.x < 0)
            {
                Notify(SwipeDirection.Left, Math.Abs(direction.x));
                return;
            }
            Notify(SwipeDirection.Right, direction.x);
        }

        private void OnVerticalSwipe(Vector2 direction)
        {
            if (direction.y < 0)
            {
                Notify(SwipeDirection.Down, Math.Abs(direction.y));
                return;
            }
            Notify(SwipeDirection.Up, direction.y);
        }

        private void Notify(SwipeDirection direction, float strength)
        {
            _observers.Notify(new SwipeInfo(_lastDetectedSwipe, strength, direction));
        }

        private void OnDisable()
        {
            if (_subscription == null)
                return;
            _subscription.Dispose();
        }
    }
}
