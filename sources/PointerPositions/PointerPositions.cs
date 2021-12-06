using Rubicks.Extensions;
using Rubicks.Extensions.Disposing;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rubicks.ExtendedPointerInput
{
    [RequireComponent(typeof(HoldPullSensor))]
    public abstract class PointerPositions : MonoBehaviour, IObservable<PointerPositions>
    {
        private HoldPullSensor _sensor;
        private List<IObserver<PointerPositions>> _observers = new List<IObserver<PointerPositions>>();

        public abstract Vector3 PressMousePosition { get; }
        public abstract Vector3 CurrentMousePosition { get; }
        public abstract Vector3 DeltaPosition { get; }
        public abstract Vector3 DeltaThisTick { get; }

        public abstract event Action PointerMoved;

        private void Start()
        {
            Initialize();
            _sensor = GetComponent<HoldPullSensor>();
            _sensor.Started += OnStart;
            _sensor.Pressed += OnHold;
            _sensor.Finished += OnFinish;
        }

        public virtual void Initialize()
        {

        }

        protected abstract void OnStart();

        protected abstract void OnHold();

        protected abstract void OnFinish();


        private void OnDisable()
        {
            _sensor.Started -= OnStart;
            _sensor.Pressed -= OnHold;
            _sensor.Finished -= OnFinish;
        }

        public IDisposable Subscribe(IObserver<PointerPositions> observer)
        {
            _observers.Add(observer);
            return Disposable.Create(() => _observers.Remove(observer));        
        }

        protected void Notify(PointerPositions pointerPositions)
        {
            _observers.Notify(pointerPositions);
        }
    }
}