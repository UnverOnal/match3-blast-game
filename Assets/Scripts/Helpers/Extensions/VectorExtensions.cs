using UnityEngine;

namespace Helpers.Extensions
{
    public static class VectorExtensions
    {
        #region Vector3
            public static Vector3 WithX(this Vector3 vector, float value)
            {
                vector.x = value;
                return vector;
            }

            public static Vector3 WithY(this Vector3 vector, float value)
            {
                vector.y = value;
                return vector;
            }  

            public static Vector3 WithZ(this Vector3 vector, float value)
            {
                vector.z = value;
                return vector;
            }

            public static Vector3 AddToX(this Vector3 vector, float value)
            {
                vector.x += value;
                return vector;
            }

            public static Vector3 AddToY(this Vector3 vector, float value)
            {
                vector.y += value;
                return vector;
            }  

            public static Vector3 AddToZ(this Vector3 vector, float value)
            {
                vector.z += value;
                return vector;
            }  
        #endregion

        #region Vector2
            public static Vector2 WithX(this Vector2 vector, float value)
            {
                vector.x = value;
                return vector;
            }

            public static Vector2 WithY(this Vector2 vector, float value)
            {
                vector.y = value;
                return vector;
            }  

            public static Vector2 AddToX(this Vector2 vector, float value)
            {
                vector.x += value;
                return vector;
            }

            public static Vector2 AddToY(this Vector2 vector, float value)
            {
                vector.y += value;
                return vector;
            } 
        #endregion  
    }
}
