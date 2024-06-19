using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utils
{
    public class DraggableObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private float dragSpeed;
        [SerializeField] private Vector3 axis;
        [SerializeField] private bool is3D;
        
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

            var mousePosition = is3D? Camera.main.ScreenToWorldPoint(Input.mousePosition) : Input.mousePosition;
            var position = cacheTransform.position;
            var moveDirection = mousePosition - position;
            var targetPosition = position + new Vector3(moveDirection.x * axis.x, moveDirection.y * axis.y, moveDirection.z * axis.z);
            position = Vector3.Lerp(position, targetPosition, dragSpeed * Time.deltaTime);
            cacheTransform.position = position;
            UpdateDrag();
        }

        protected virtual void OnDragEnd(){}
        protected virtual void OnDragStart(){}
        protected virtual void UpdateDrag(){}
    }
}