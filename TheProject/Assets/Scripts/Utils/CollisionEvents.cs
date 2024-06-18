using System;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    public delegate void TriggerEvent(Collider source, Collider other);
    public delegate void CollisionEvent(Collision collision);
    
    public class CollisionEvents : MonoBehaviour
    {
        private Collider colliderCache;
        public event TriggerEvent TriggerEnter;
        public event TriggerEvent TriggerExit;
        public event CollisionEvent CollisionEnter;
        public event CollisionEvent CollisionExit;

        private void Awake()
        {
            colliderCache = GetComponent<Collider>();
        }
        private void OnTriggerEnter(Collider other)
        {
            TriggerEnter?.Invoke(colliderCache, other);
        }

        private void OnTriggerExit(Collider other)
        {
            TriggerExit?.Invoke(colliderCache, other);
        }

        private void OnCollisionEnter(Collision collision)
        {
            CollisionEnter?.Invoke(collision);
        }
        
        private void OnCollisionExit(Collision collision)
        {
            CollisionExit?.Invoke(collision);
        }
    }
}