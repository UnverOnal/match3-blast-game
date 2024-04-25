using UnityEngine;

namespace Helpers.Extensions
{
    public static class TransformExtensions
    {
        #region Position
            #region World
                public static void SetPositionX(this Transform transform, float value)
                {
                    transform.position = transform.position.WithX(value);
                }

                public static void SetPositionY(this Transform transform, float value)
                {
                    transform.position = transform.position.WithY(value);
                }

                public static void SetPositionZ(this Transform transform, float value)
                {
                    transform.position = transform.position.WithZ(value);
                }

                public static void AddToPositionX(this Transform transform, float value)
                {
                    transform.position = transform.position.AddToX(value);
                }

                public static void AddToPositionY(this Transform transform, float value)
                {
                    transform.position = transform.position.AddToY(value);
                }

                public static void AddToPositionZ(this Transform transform, float value)
                {
                    transform.position = transform.position.AddToZ(value);
                }
            #endregion

            #region Local
                public static void SetLocalPositionX(this Transform transform, float value)
                {
                    transform.localPosition = transform.localPosition.WithX(value);
                }

                public static void SetLocalPositionY(this Transform transform, float value)
                {
                    transform.localPosition = transform.localPosition.WithY(value);
                }

                public static void SetLocalPositionZ(this Transform transform, float value)
                {
                    transform.localPosition = transform.localPosition.WithZ(value);
                }

                public static void AddToLocalPositionX(this Transform transform, float value)
                {
                    transform.localPosition = transform.localPosition.AddToX(value);
                }

                public static void AddToLocalPositionY(this Transform transform, float value)
                {
                    transform.localPosition = transform.localPosition.AddToY(value);
                }

                public static void AddToLocalPositionZ(this Transform transform, float value)
                {
                    transform.localPosition = transform.localPosition.AddToZ(value);
                }
            #endregion
        #endregion

        #region Scale
            public static void SetScaleX(this Transform transform, float value)
            {
                transform.localScale = transform.localScale.WithX(value);
            }

            public static void SetScaleY(this Transform transform, float value)
            {
                transform.localScale = transform.localScale.WithY(value);
            }

            public static void SetScaleZ(this Transform transform, float value)
            {
                transform.localScale = transform.localScale.WithZ(value);
            }
        #endregion
    }  
}

