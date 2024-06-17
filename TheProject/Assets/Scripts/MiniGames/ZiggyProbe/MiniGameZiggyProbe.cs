using Cysharp.Threading.Tasks;
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
            
            OnWin();
            Debug.Log("Won Ziggy mini game");
        }
    }
}