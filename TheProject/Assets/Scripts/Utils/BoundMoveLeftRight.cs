using System;
using UnityEngine;

namespace Utils
{
    public class BoundMoveLeftRight : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float step;
        [SerializeField] private float boundsDistance;
        private Vector2 relativeBounds;
        private bool isActive;
        private bool movingLeft;
        private Transform cacheTransform;

        public void Activate()
        {
            isActive = true;
            cacheTransform = transform;
            var xPosition = cacheTransform.position.x;
            relativeBounds = new Vector2(xPosition - boundsDistance, xPosition + boundsDistance);
        }

        public void Deactivate()
        {
            isActive = false;
        }
        
        private void Update()
        {
            if (!isActive)
            {
                return;
            }

            var newPosition = movingLeft ? GetLeftPosition() : GetRightPosition();
            cacheTransform.position = Vector3.Lerp(cacheTransform.position, newPosition, speed * Time.deltaTime);
            if (cacheTransform.position.x < relativeBounds.x)
            {
                movingLeft = false;
                newPosition.x = relativeBounds.x;
                cacheTransform.position = newPosition;
            } else if(cacheTransform.position.x > relativeBounds.y)
            {
                movingLeft = true;
                newPosition.x = relativeBounds.y;
                cacheTransform.position = newPosition;
            }
        }

        private Vector3 GetRightPosition()
        {
            return cacheTransform.position + Vector3.right * step;
        }

        private Vector3 GetLeftPosition()
        {
            return cacheTransform.position + Vector3.left * step;
        }
    }
}