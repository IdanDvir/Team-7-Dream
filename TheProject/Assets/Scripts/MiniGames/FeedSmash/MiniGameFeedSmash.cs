using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils;

namespace MiniGames.FeedSmash
{
    public class MiniGameFeedSmash : MinigameScreen
    {
        [SerializeField] private Animation smashAnimations;
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

        public override void StartScreen(StopwatchView stopWatchView)
        {
            base.StartScreen(stopWatchView);
            bananaObject.Activate(OnReachedTarget, minDistanceToWin, targetTransform);
            Debug.Log("Started smash mini game");
        }

        private async UniTask AnimateSmash()
        {
            smashAnimations.Play("Smash_Mouth_Close");
            await UniTask.WaitForSeconds(0.4f);
            smashAnimations.Play("Smash_Mouth_Open");
            await UniTask.WaitForSeconds(0.3f);
            smashAnimations.Play("Smash_Mouth_Close");
        }

        private void OnReachedTarget()
        {
            bananaObject.Deactivate();
            stopwatch.Stop();
            
            OnWin();
            Debug.Log("Won smash mini game");
        }

        public override async UniTask DoExtraEnd()
        {
            await base.DoExtraEnd();
            await AnimateSmash();
        }

        protected override void OnTimerEnd()
        {
            bananaObject.Deactivate();
            OnLose();
            Debug.Log("Lost smash mini game");
        }
    }
}