using UnityEngine;

namespace Sensors
{
    [RequireComponent(typeof(MousePositionScreenPointSensor))]
    public class SwipeSensor : MonoBehaviour
    {
        [SerializeField] private Vector4 _swipeStep = new Vector4(1,1,1,1);
        private MousePositionScreenPointSensor _sensor;

        private void Start()
        {
            _sensor.PointerMoved += PointerMoved; 
        }

        private void PointerMoved()
        {
           
        }
    }
}
