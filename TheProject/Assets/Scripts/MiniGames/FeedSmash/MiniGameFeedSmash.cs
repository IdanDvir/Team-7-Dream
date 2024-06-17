using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils;

namespace MiniGames.FeedSmash
{
    public class MiniGameFeedSmash : MinigameScreen
    {
        [SerializeField] private OnTargetReachedDraggable bananaObject;
        [SerializeField] private Transform targetTransform;
        [SerializeField] private float minDistanceToWin;

        public override async UniTask Show()
        {
            await base.Show();
            gameObject.SetActive(true);
        }

        public override async UniTask Hide()
        {
            await base.Hide();
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