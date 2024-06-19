using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace MiniGames.BreakALeg
{
    public class BreakALeg : MinigameScreen
    {
        [SerializeField] private GameObject death;
        [SerializeField] private GameObject win;
        [SerializeField] private DraggableObject bouncyBounce;
        [SerializeField] private Rigidbody bellaRina;
        [SerializeField] private float startXForceMin;
        [SerializeField] private float startXForceMax;
        [SerializeField] private float startYForceMin;
        [SerializeField] private float startYForceMax;
        [SerializeField] private float gravityForce;

        private Vector3 oldGravity;
        private CollisionEvents bellaRinaEvents;
        private float previousTimescape;
        
        public override async UniTask Show()
        {
            await base.Show();
            gameObject.SetActive(true);
        }

        public override async UniTask Hide()
        {
            await base.Hide();
            Physics.gravity = oldGravity;
            Time.timeScale = Time.timeScale;
            gameObject.SetActive(false);
        }

        public override void StartScreen(StopwatchView stopWatchView)
        {
            base.StartScreen(stopWatchView);
            bellaRinaEvents = bellaRina.GetComponent<CollisionEvents>();
            bellaRinaEvents.TriggerEnter += OnBellaRinaTriggered;
            bouncyBounce.Activate();
            bellaRina.useGravity = true;
            previousTimescape = Time.timeScale;
            Time.timeScale += 2;
            oldGravity = Physics.gravity;
            Physics.gravity = Vector3.down * gravityForce;
            var startingXForce = Random.Range(startXForceMin, startXForceMax) * bellaRina.mass;
            var startingYForce = Random.Range(startYForceMin, startYForceMax) * bellaRina.mass;
            bellaRina.AddForce(Vector3.up * startingYForce);
            bellaRina.AddForce(Vector3.right * startingXForce);
        }

        private void OnBellaRinaTriggered(Collider bellaRinaCollider, Collider other)
        {
            if (other.gameObject == death)
            {
                Physics.gravity = oldGravity;
                Time.timeScale = previousTimescape;
                bellaRina.useGravity = false;
                bellaRina.velocity = Vector3.zero;
                OnLose();
            }

            if (other.gameObject == win)
            {
                Physics.gravity = oldGravity;
                Time.timeScale = previousTimescape;
                bellaRina.useGravity = false;
                bellaRina.velocity = Vector3.zero;
                OnWin();
            }

            if (other.gameObject == bouncyBounce.gameObject)
            {
                var reversedPolarity = bellaRina.velocity;
                reversedPolarity.y = -reversedPolarity.y;
                bellaRina.velocity = reversedPolarity;
            }
                
        }

        protected override void OnTimerEnd()
        {
            Physics.gravity = oldGravity;
            Time.timeScale = Time.timeScale;
            OnLose();
        }
    }
}