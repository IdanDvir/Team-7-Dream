using UnityEngine;
using UnityEngine.EventSystems;

namespace Utils
{
    public delegate void Triggered2D(Collider2D source, Collider2D other);
    public class Clickable : MonoBehaviour, IPointerDownHandler
    {
        public event Triggered2D Triggered2D;
        protected Transform cacheTransform;
        [SerializeField] private float forceAmount = 10f; 
        protected Rigidbody2D rb2d;
        protected Collider2D collider2d;
        protected bool isActive;
        
        public void Activate()
        {
            isActive = true;
            cacheTransform = transform;
            rb2d = GetComponent<Rigidbody2D>();
            collider2d = GetComponent<Collider2D>();
        }

        public void Deactivate()
        {
            isActive = false;
            rb2d.simulated = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Triggered2D?.Invoke(collider2d, other);
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!isActive || !rb2d)
            {
                return;
            }

            if (!collider2d.OverlapPoint(eventData.position)) 
                return;
            
            var direction = eventData.position.x >= cacheTransform.position.x ? Vector2.left : Vector2.right;
            rb2d.AddForce(direction * forceAmount * rb2d.mass * rb2d.gravityScale, ForceMode2D.Impulse);
        }

        public void Stop()
        {
            rb2d.simulated = false;
        }
    }
}