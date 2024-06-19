using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace MiniGames.ZiggyProbe
{
    public class MiniGameZiggyProbe : MinigameScreen
    {
        [SerializeField] private DraggableObject ziggyObject;
        [SerializeField] private BoundMoveLeftRight boundTargetTransform;
        [SerializeField] private float minDistanceToWin;
        [SerializeField] private Animator mustacheAnimator;
        [SerializeField] private Animator ziggyAnimator;
        private bool snached;
        private bool finishedMoving;

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
            ziggyObject.Activate();
            boundTargetTransform.Activate();
            Debug.Log("Started Ziggy mini game.");
        }

        protected override void OnTimerEnd()
        {
            ziggyObject.Deactivate();
            boundTargetTransform.Deactivate();
            var ziggyPosition = ziggyObject.transform.position;
            var targetPosition = boundTargetTransform.transform.position;
            var ziggyPlanarPosition = new Vector3(ziggyPosition.x, targetPosition.y, targetPosition.z);
            var distance = Vector3.Distance(ziggyPlanarPosition, targetPosition);
            if (distance > minDistanceToWin)
            {
                OnLose();
                Debug.Log("Lost Ziggy mini game");
                return;
            }

            snached = true;
            OnWin();
            Debug.Log("Won Ziggy mini game");
        }

        public override async UniTask DoExtraEnd()
        {
            await base.DoExtraEnd();
            mustacheAnimator.SetTrigger("Idle");
            boundTargetTransform.transform.localEulerAngles = Vector3.zero;
            ziggyAnimator.SetTrigger("Snatch");
            await UniTask.WaitForSeconds(1f);
            if (snached)
            {
                boundTargetTransform.transform.DOMove(ziggyObject.transform.position, 0.5f).onComplete += () =>
                {
                    finishedMoving = true;;
                };
                boundTargetTransform.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutCirc);
                await UniTask.WaitUntil(MoveMustacheToZiggy);
            }
            await UniTask.WaitForSeconds(0.2f);
        }

        private bool MoveMustacheToZiggy()
        {
            return finishedMoving;
        }
    }
}