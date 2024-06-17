using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MiniGames.FeedSmash
{
    public class DraggableObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private float dragSpeed;
        
        private Action onStopDrag;
        private bool isActive;
        private Transform cacheTransform;
        private bool isDragging;

        public void Activate(Action onStopDrag)
        {
            this.onStopDrag = onStopDrag;
            isActive = true;
            cacheTransform = transform;
        }

        public void Deactivate()
        {
            isActive = false;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!isActive)
            {
                return;
            }

            isDragging = true;
            
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!isActive || !isDragging)
            {
                return;
            }

            isDragging = false;
            onStopDrag.Invoke();
        }

        private void Update()
        {
            if (!isActive || !isDragging)
            {
                return;
            }

            cacheTransform.position = Vector3.Slerp(cacheTransform.position, Input.mousePosition, dragSpeed * Time.deltaTime);
        }
    }
}