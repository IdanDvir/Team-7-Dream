using UnityEngine;

namespace MiniGames.NotTheBees
{
    public class Tentacle : MonoBehaviour
    {
        [SerializeField] private Transform tentacles;
        private Bee currentBee;
        private bool isActive;

        public void Activate()
        {
            isActive = true;
        }

        public void Stop()
        {
            isActive = false;
        }
        
        private void Update()
        {
            if (!isActive)
            {
                return;
            }

            HandleInput();
            var raycast = Physics2D.Raycast(tentacles.position + tentacles.up * 50, -tentacles.up);
            Debug.DrawLine(tentacles.position + tentacles.up * 50, tentacles.position + tentacles.up * 50 + -tentacles.up * 500, Color.black, Time.deltaTime);
            if (currentBee != null)
            {
                if (raycast.collider != null) return;
                
                currentBee.Resume();
                currentBee = null;
                return;
            }

            if (raycast.collider == null) return;
            
            if (!raycast.transform.gameObject.TryGetComponent<Bee>(out var bee)) return;
                
            currentBee = bee;
            currentBee.PushBack();
        }

        private void HandleInput()
        {
            if (!Input.GetKey(KeyCode.Mouse0))
            {
                return;
            }

            tentacles.up = tentacles.position - Input.mousePosition;
        }
    }
}