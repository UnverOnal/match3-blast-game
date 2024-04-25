using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Helpers.Movement
{
    public static class Movement
    {
        #region Position
            public static void MoveTo(this Transform transform, Vector3 targetPosition, float smoothness)
            {
                var currentPosition = transform.position;

                var smoothedPosition = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * smoothness);
                transform.position = smoothedPosition;
            }
            
            public static async UniTask MoveToTarget(this Transform transform, Vector3 targetPosition, float duration)
            {
                var currentPosition = transform.position;
            
                var elapsedTime = 0f;
            
                while(elapsedTime < duration)
                {
                    transform.position = Vector3.Lerp(currentPosition, targetPosition, elapsedTime/duration);
            
                    elapsedTime += Time.deltaTime;

                    await UniTask.Yield();
                }
                transform.position = targetPosition;
            }

            public static async UniTask MoveToTarget(this Transform transform, Vector3 targetPosition, float duration, Action endAction)
            {
                var currentPosition = transform.position;

                var elapsedTime = 0f;

                while(elapsedTime < duration)
                {
                    transform.position = Vector3.Lerp(currentPosition, targetPosition, elapsedTime/duration);

                    elapsedTime += Time.deltaTime;

                    await UniTask.Yield();
                }
                transform.position = targetPosition;

                endAction.Invoke();
            }

            public static async UniTask MoveToTarget(this Transform transform, Transform targetTransform, float duration)
            {
                var currentPosition = transform.position;

                var elapsedTime = 0f;

                while(elapsedTime < duration)
                {
                    transform.position = Vector3.Lerp(currentPosition, targetTransform.position, elapsedTime/duration);

                    elapsedTime += Time.deltaTime;

                    await UniTask.Yield();
                }
                transform.position = targetTransform.position;
            }

            public static async UniTask MoveToTarget(this Transform transform, Transform targetTransform, float duration, Action endAction)
            {
                var currentPosition = transform.position;

                var elapsedTime = 0f;

                while(elapsedTime < duration)
                {
                    transform.position = Vector3.Lerp(currentPosition, targetTransform.position, elapsedTime/duration);

                    elapsedTime += Time.deltaTime;

                    await UniTask.Yield();
                }
                transform.position = targetTransform.position;

                endAction.Invoke();
            }
            
            /// <param name="endDistance">Stopping distance.</param>
            public static async UniTask MoveToTarget(this Transform transform, Vector3 targetPosition, float smoothness, float endDistance)
            {
                Vector3 currentPosition;

                while(Vector3.Distance(transform.position, targetPosition) > endDistance)
                {
                    currentPosition = transform.position;
                    transform.position = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * smoothness);

                    await UniTask.Yield();
                }
                transform.position = targetPosition;
            }
        #endregion
        
        #region Rotation
            public static async UniTask RotateTo(this Transform transform, float duration, Vector3 targetAngles)
            {
                var currentRotation = transform.rotation;
                var targetRotation = Quaternion.Euler(targetAngles);

                var elapsedTime = 0f;

                while(elapsedTime < duration)
                {
                    transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, elapsedTime/duration);

                    elapsedTime += Time.deltaTime;

                    await UniTask.Yield();
                }
                transform.rotation = targetRotation;
            }

            public static async UniTask RotateTo(this Transform transform, float duration, Vector3 targetAngles, Action endAction)
            {
                var currentRotation = transform.rotation;
                var targetRotation = Quaternion.Euler(targetAngles);

                var elapsedTime = 0f;

                while(elapsedTime < duration)
                {
                    transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, elapsedTime/duration);

                    elapsedTime += Time.deltaTime;

                    await UniTask.Yield();
                }
                transform.rotation = targetRotation;

                endAction.Invoke();
            }

            public static async UniTask Rotate(this Transform transform, float duration, Vector3 axis, float speed)
            {
                var elapsedTime = 0f;

                while(elapsedTime < duration)
                {
                    transform.Rotate(axis * Time.deltaTime * speed);

                    elapsedTime += Time.deltaTime;

                    await UniTask.Yield();
                }
            }

            public static void Rotate(this Transform transform, Vector3 axis, float rotateSpeed)
            {
                transform.Rotate(axis * Time.deltaTime * rotateSpeed);
            }
        
            public static async UniTask RotateToLocal(this Transform transform, float duration, Vector3 targetAngles)
            {
                var currentRotation = transform.localRotation;
                var targetRotation = Quaternion.Euler(targetAngles);

                var elapsedTime = 0f;

                while(elapsedTime < duration)
                {
                    transform.localRotation = Quaternion.Lerp(currentRotation, targetRotation, elapsedTime/duration);

                    elapsedTime += Time.deltaTime;

                    await UniTask.Yield();
                }
                transform.localRotation = targetRotation;
            }

            public static async UniTask RotateToLocal(this Transform transform, float duration, Vector3 targetAngles, Action endAction)
            {
                var currentRotation = transform.localRotation;
                var targetRotation = Quaternion.Euler(targetAngles);

                var elapsedTime = 0f;

                while(elapsedTime < duration)
                {
                    transform.localRotation = Quaternion.Lerp(currentRotation, targetRotation, elapsedTime/duration);

                    elapsedTime += Time.deltaTime;

                    await UniTask.Yield();
                }
                transform.localRotation = targetRotation;

                endAction.Invoke();
            }

            public static async UniTask RotateLocal(this Transform transform, float duration, Vector3 axis, float speed)
            {
                var elapsedTime = 0f;

                while(elapsedTime < duration)
                {
                    transform.Rotate(axis * Time.deltaTime * speed, Space.Self);

                    elapsedTime += Time.deltaTime;

                    await UniTask.Yield();
                }
            }

            public static void RotateLocal(this Transform transform, Vector3 axis, float rotateSpeed)
            {
                transform.Rotate(axis * Time.deltaTime * rotateSpeed, Space.Self);
            }
        #endregion

        #region Scale
            public static void Scale(this Transform transform, Vector3 targetScale, float sensitivity)
            {
                var smoothedScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * sensitivity);
                transform.localScale = smoothedScale;
            }

            public static async UniTask Scale(this Transform transform, float duration, Vector3 targetScale)
            {
                var currentScale = transform.localScale;

                var elapsedTime = 0f;

                while(elapsedTime < duration)
                {
                    transform.localScale = Vector3.Lerp(currentScale, targetScale, elapsedTime/duration);

                    elapsedTime += Time.deltaTime;

                    await UniTask.Yield();
                }
                transform.localScale = targetScale;
            }

            public static async UniTask Scale(this Transform transform, float duration, Vector3 targetScale, Action endAction)
            {
                var currentScale = transform.localScale;

                var elapsedTime = 0f;

                while(elapsedTime < duration)
                {
                    transform.localScale = Vector3.Lerp(currentScale, targetScale, elapsedTime/duration);

                    elapsedTime += Time.deltaTime;

                    await UniTask.Yield();
                }
                transform.localScale = targetScale;

                endAction.Invoke();
            }
        #endregion
        
        public static void MoveForward(this Transform transform, Vector3 direction, float moveFactor, float smoothness = 1.5f)
        {
            Vector3 targetPosition = transform.position + direction * moveFactor;

            transform.MoveTo(targetPosition, smoothness);
        }

        public static void MoveSides(this Transform transform, float input ,Vector3 axis, float moveFactor, float border, Transform platform, float smoothness = 3f)
        {
            Vector3 targetPosition = transform.position + axis * input * moveFactor;
            targetPosition.x = Mathf.Clamp(targetPosition.x, platform.position.x - border, platform.position.x + border);

            transform.MoveTo(targetPosition, smoothness);
        }
    }
}

