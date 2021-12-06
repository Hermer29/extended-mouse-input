using Rubicks.Extensions;
using Rubicks.Extensions.Disposing;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rubicks.ExtendedPointerInput
{
    [RequireComponent(typeof(HoldPullSensor))]
    public class ObjectPicker : MonoBehaviour, IObservable<GameObject>
    {
        [SerializeField] private Camera _camera = null;
        
        private HoldPullSensor _sensor;
        private List<IObserver<GameObject>> _observers = new List<IObserver<GameObject>>();

        public IDisposable Subscribe(IObserver<GameObject> observer)
        {
            _observers.Add(observer);
            return Disposable.Create(() => _observers.Remove(observer));
        }

        private void Start()
        {
            if (_camera == null)
            {
                Debug.Log("Camera not assigned, will be assigned Camera.main property");
                _camera = Camera.main;
            }

            _sensor = GetComponent<HoldPullSensor>();
            _sensor.Where(x => x == PressState.Started).ToEvent().Subscribe(OnStart);
        }

        private void OnStart()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            var hit = Physics2D.GetRayIntersection(ray);
            Log(hit.transform);
            if (hit.transform != null)
                Notify(hit.transform.gameObject);
        }

        private void Log(Transform obstacledObject)
        {
            if (obstacledObject == null)
            {
                Debug.Log("Obstacled with nobody");
                return;
            }
            Debug.Log($"Obstacled with object with name: \"{obstacledObject.name}\" ");
        }

        private void Notify(GameObject gameObject)
        {
            _observers.Notify(gameObject);
        }
    }
}
