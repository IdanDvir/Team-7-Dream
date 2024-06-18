using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MiniGames.TentaclesExplode
{
    public class TentaclesJuggleBomb : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Rigidbody bombRigidbody;
        [SerializeField] private Vector2 minMaxJumpForceSide;
        [SerializeField] private float jumpForceUp;
        [SerializeField] private float finalForce;
        [SerializeField] private Vector2 xBounds;
        [SerializeField] private Vector2 yBounds;
        private Action onDrop;
        private bool isActive;

        public void Init(Action onDrop)
        {
            this.onDrop = onDrop;
            bombRigidbody.isKinematic = true;
        }

        public async UniTask FirstLaunch(Vector3 force, float range)
        {
            await UniTask.WaitForSeconds(range);
            bombRigidbody.isKinematic = false;
            bombRigidbody.AddForce(force, ForceMode.Impulse);
            await UniTask.WaitUntil(InBounds);
            isActive = true;
        }

        private bool InBounds()
        {
            var position = bombRigidbody.position;
            return position.x > xBounds.x
                   && position.x < xBounds.y
                   && position.y > yBounds.x
                   && position.y < yBounds.y;
        }

        public void Stop()
        {
            bombRigidbody.isKinematic = true;
            isActive = false;
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            var bombTransform = transform;
            var position = bombTransform.position;
            var mousePosition = eventData.pointerPressRaycast.worldPosition;
            var isLeft = position.x > mousePosition.x;
            var radius = bombTransform.localScale.x/2.0f;
            var distance = Vector3.Distance(mousePosition, position);
            var sideForce = (isLeft ? Vector3.left : Vector3.right) * (distance / radius);
            var upForce = Vector3.up * jumpForceUp;
            var normalizedForce = sideForce + upForce;
            bombRigidbody.AddForce(normalizedForce * finalForce, ForceMode.Impulse);
        }

        private void Update()
        {
            if (!isActive)
            {
                return;
            }
            
            if (bombRigidbody.position.x < xBounds.x)
            {
                var position = bombRigidbody.position;
                position = new Vector3(xBounds.x, position.y, position.z);
                bombRigidbody.position = position;

                var velocity = bombRigidbody.velocity;
                velocity = new Vector3(-velocity.x, velocity.y, 0);
                bombRigidbody.velocity = velocity;
            }
            else if (bombRigidbody.position.x > xBounds.y)
            {
                var position = bombRigidbody.position;
                position = new Vector3(xBounds.y, position.y, position.z);
                bombRigidbody.position = position;

                var velocity = bombRigidbody.velocity;
                velocity = new Vector3(-velocity.x, velocity.y, 0);
                bombRigidbody.velocity = velocity;
            } else if (bombRigidbody.position.y < yBounds.x)
            {
                onDrop.Invoke();
            } else if (bombRigidbody.position.y > yBounds.y)
            {
                var position = bombRigidbody.position;
                position = new Vector3(position.x, yBounds.y, position.z);
                bombRigidbody.position = position;

                var velocity = bombRigidbody.velocity;
                velocity = new Vector3(velocity.x, -velocity.y/2.0f, 0);
                bombRigidbody.velocity = velocity;
            }
        }
    }
}