using System;
using UnityEngine;
using UnityEngine.UI;

namespace MiniGames.NotTheBees
{
    public class Bee : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float smoothness;
        [SerializeField] private float pushBackMultiplier = 0.5f;
        [SerializeField] private float minDistanceToLose;
        
        private Vector3 targetPosition;
        private Transform cacheTransform;
        private bool isActive;
        private bool moveForward;
        private Action onReachedTarget;
        private RectTransform rectTranform;
        private Action onDeath;

        public void Activate(Vector3 targetPosition, Action onReachedTarget, Action onDeath)
        {
            this.onReachedTarget = onReachedTarget;
            this.targetPosition = targetPosition;
            this.onDeath = onDeath;
            cacheTransform = transform;
            isActive = true;
            moveForward = true;
            rectTranform = GetComponent<RectTransform>();
        }

        public void Stop()
        {
            isActive = false;
        }

        public void PushBack()
        {
            moveForward = false;
        }

        public void Resume()
        {
            moveForward = true;
        }
        
        private void Update()
        {
            if (!isActive)
            {
                return;
            }

            var position = cacheTransform.position;
            var direction = (targetPosition - position).normalized;
            var force = (moveForward ? direction : -direction * pushBackMultiplier) * speed * Time.deltaTime;
            var newPosition = position + force;
            position = Vector3.Lerp(position, newPosition, smoothness);
            cacheTransform.position = position;
            if (Vector3.Distance(targetPosition, position) < minDistanceToLose)
            {
                onReachedTarget.Invoke();
            }

            if (moveForward || IsVisible()) return;
            
            onDeath.Invoke();
            Destroy(gameObject);
        }

        private bool IsVisible()
        {
            Rect screenBounds = new Rect(0f, 0f, UnityEngine.Screen.width, UnityEngine.Screen.height); // Screen space bounds (assumes camera renders across the entire screen)
            Vector3[] objectCorners = new Vector3[4];
            rectTranform.GetWorldCorners(objectCorners);
 
            var visibleCorners = 0;
            foreach (var t in objectCorners)
            {// Transform world space position of corner to screen space
                if (screenBounds.Contains(t)) // If the corner is inside the screen
                {
                    visibleCorners++;
                }
            }
            return visibleCorners > 0;
        }
    }
}