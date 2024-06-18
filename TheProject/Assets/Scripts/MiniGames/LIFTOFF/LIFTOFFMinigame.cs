using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

namespace MiniGames.LIFTOFF
{
    public class LIFTOFFMinigame : MinigameScreen
    {
        [SerializeField] private Animator gizmoAnimation;
        [SerializeField] private Rigidbody gizmo;
        [SerializeField] private GameObject ziggy;
        [SerializeField] private float acceleration = 1f;
        [SerializeField] private ParticleSystem EXPLOSIOOOONNNN;

        private bool isLaunching = false;
        private float velocity = 0;
        private bool isActive = false;

        public override void StartScreen(StopwatchView stopWatchView)
        {
            isActive = true;
            base.StartScreen(stopWatchView);
        }

        protected override void OnTimerEnd()
        {
            OnLose();
        }

        public void Update()
        {
            if (!isActive)
                return;
            
            if (Input.GetKeyDown(KeyCode.Mouse0) && !isLaunching)
            {
                isLaunching = true;
                gizmoAnimation.SetTrigger("Liftoff");
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                isLaunching = false;
                gizmoAnimation.SetTrigger("Idle");
            }

            var force = isLaunching ? acceleration : -acceleration;
            gizmo.AddForce(Vector3.up * force);

            if (gizmo.transform.position.y >= ziggy.transform.position.y)
            {
                EXPLOSIOOOONNNN.GameObject().SetActive(true);
                EXPLOSIOOOONNNN.transform.position = gizmo.transform.position;
                EXPLOSIOOOONNNN.Play(true);
                OnWin();
                isActive = false;
                var direction = Random.Range(0, 1) == 1 ? Vector3.left : Vector3.right;
                gizmo.AddForce((Vector3.up + direction) * 1000);
            }
        }
    }
}