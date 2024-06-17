using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utils
{
    public class DraggableObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private float dragSpeed;
        
        protected Transform cacheTransform;
        protected bool isActive;
        protected bool isDragging;

        public void Activate()
        {
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
            OnDragStart();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!isActive || !isDragging)
            {
                return;
            }

            isDragging = false;
            OnDragEnd();
        }

        private void Update()
        {
            if (!isActive || !isDragging)
            {
                return;
            }

            cacheTransform.position = Vector3.Slerp(cacheTransform.position, Input.mousePosition, dragSpeed * Time.deltaTime);
            UpdateDrag();
        }

        protected virtual void OnDragEnd(){}
        protected virtual void OnDragStart(){}
        protected virtual void UpdateDrag(){}
    }
}