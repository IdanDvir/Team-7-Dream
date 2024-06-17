using System;

namespace Utils
{
    public class OnStopDraggable : DraggableObject
    {
        private Action onDragStopped;

        public void Activate(Action onDragStopped)
        {
            this.onDragStopped = onDragStopped;
            Activate();
        }

        protected override void OnDragEnd()
        {
            onDragStopped.Invoke();
        }
    }
}