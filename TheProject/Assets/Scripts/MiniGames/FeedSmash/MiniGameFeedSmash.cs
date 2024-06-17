using UnityEngine;
using Utils;

namespace MiniGames.FeedSmash
{
    public class MiniGameFeedSmash : MinigameScreen
    {
        [SerializeField] private OnTargetReachedDraggable bananaObject;
        [SerializeField] private Transform targetTransform;
        [SerializeField] private float minDistanceToWin;

        public override void OnShow()
        {
            base.OnShow();
            gameObject.SetActive(true);
        }

        public override void OnHide()
        {
            base.OnHide();
            gameObject.SetActive(false);
        }

        public override void StartScreen()
        {
            base.StartScreen();
            bananaObject.Activate(OnReachedTarget, minDistanceToWin, targetTransform);
            Debug.Log("Started smash mini game");
        }

        private void OnReachedTarget()
        {
            bananaObject.Deactivate();
            stopwatch.Stop();
            OnWin();
            Debug.Log("Won smash mini game");
        }

        protected override void OnTimerEnd()
        {
            bananaObject.Deactivate();
            OnLose();
            Debug.Log("Lost smash mini game");
        }
    }
}