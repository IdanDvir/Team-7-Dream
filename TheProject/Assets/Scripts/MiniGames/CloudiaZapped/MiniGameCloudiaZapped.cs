using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace MiniGames.CloudiaZapped
{
    public class MiniGameCloudiaZapped : MinigameScreen
    {
        [SerializeField] private DraggableObject tentaclesObject;
        [SerializeField] private BoundMoveLeftRight cloudiaTransform;
        [SerializeField] private Image cloudiaImage;
        [SerializeField] private Sprite cloudiaHapy;
        [SerializeField] private Sprite cloudiaAngy;
        [SerializeField] private GameObject lightningBolt;
        [SerializeField] private GameObject lightningBoltSmall;
        [SerializeField] private int[] lightningTimings;
        [SerializeField] private float minDistanceToLose;

        private List<Stopwatch> lightningStopwatches = new();
        public override async UniTask Show()
        {
            await base.Show();
            gameObject.SetActive(true);
            foreach (var lightningTiming in lightningTimings)
            {
                var lightningStopwatch = new Stopwatch(lightningTiming, FireLightning, this);
                lightningStopwatches.Add(lightningStopwatch);
            }
        }

        public async UniTask Angy()
        {
            cloudiaImage.sprite = cloudiaAngy;
            await UniTask.WaitForSeconds(0.1f);
            lightningBoltSmall.SetActive(true);
            await UniTask.WaitForSeconds(0.35f);
            lightningBoltSmall.SetActive(false);
            await UniTask.WaitForSeconds(0.15f);
            lightningBolt.SetActive(true);
            await UniTask.WaitForSeconds(0.15f);
            lightningBolt.SetActive(false);
            cloudiaImage.sprite = cloudiaHapy;
        }

        private void FireLightning()
        {
            var cloudiaPosition = tentaclesObject.transform.position;
            var targetPosition = cloudiaTransform.transform.position;
            var cloudiaPlanarPosition = new Vector3(cloudiaPosition.x, targetPosition.y, targetPosition.z);
            var distance = Vector3.Distance(cloudiaPlanarPosition, targetPosition);
            Angy().Forget();
            if (distance > minDistanceToLose) return;
            
            tentaclesObject.Deactivate();
            cloudiaTransform.Deactivate();
            foreach (var lightningStopwatch in lightningStopwatches)
            {
                lightningStopwatch.Stop();
            }
            OnLose();
            Debug.Log("Lost Cloudia mini game");
        }

        public override async UniTask Hide()
        {
            await base.Hide();
            gameObject.SetActive(false);
        }

        public override void StartScreen(StopwatchView stopWatchView)
        {
            base.StartScreen(stopWatchView);
            tentaclesObject.Activate();
            cloudiaTransform.Activate();
            foreach (var lightningStopwatch in lightningStopwatches)
            {
                lightningStopwatch.Start();
            }
            Debug.Log("Started Cloudia mini game.");
        }

        protected override void OnTimerEnd()
        {
            tentaclesObject.Deactivate();
            cloudiaTransform.Deactivate();
           OnWin();
           Debug.Log("Won Cloudia mini game");
        }
    }
}