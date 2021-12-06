using UnityEngine;

namespace Rubicks.ExtendedPointerInput
{
    public static class VectorExtensions
    {
        public static Vector2 Abs(this Vector2 vector)
        {
            if (vector.x < 0)
                vector.x = Mathf.Abs(vector.x);
            if (vector.y < 0)
                vector.y = Mathf.Abs(vector.y);

            return vector;        
        }
    }
}
