using System;
using UnityEngine;

namespace Utils
{
    public class OnTargetReachedDraggable : DraggableObject
    {
        private Action onTargetReached;
        private float minDistanceToTarget;
        private Transform target;

        public void Activate(Action onTargetReached, float minDistanceToTarget, Transform target)
        {
            this.onTargetReached = onTargetReached;
            this.minDistanceToTarget = minDistanceToTarget;
            this.target = target;
            Activate();
        }

        protected override void UpdateDrag()
        {
            base.UpdateDrag();
            var distance = Vector3.Distance(cacheTransform.position, target.position);
            if (distance > minDistanceToTarget)
            {
                return;
            }
            
            onTargetReached.Invoke();
        }
    }
}