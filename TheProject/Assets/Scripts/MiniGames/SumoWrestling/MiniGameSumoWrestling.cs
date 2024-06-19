using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace MiniGames.SumoWrestling
{
    public class MiniGameSumoWrestling : MinigameScreen
    {
        [SerializeField] private Clickable wrestlers;
        [SerializeField] private GameObject deathLeft;
        [SerializeField] private GameObject deathRight;
        [SerializeField] private List<Sprite> heads;
        [SerializeField] private Image blueHead;
        [SerializeField] private Image redHead;
        
        public override async UniTask Show()
        {
            await base.Show();
            var randomBlueHead = Random.Range(0, heads.Count);
            var randomRedHead = Random.Range(0, heads.Count);
            
            while (randomRedHead == randomBlueHead)
            {
                randomRedHead = Random.Range(0, heads.Count);
            }
            blueHead.sprite = heads[randomBlueHead];
            redHead.sprite = heads[randomRedHead];
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
            wrestlers.Activate();
            wrestlers.Triggered2D += OnTriggered2D;
        }

        private void OnTriggered2D(Collider2D source, Collider2D other)
        {
            var win = other.gameObject == deathRight;
            wrestlers.Stop();
            stopwatch.Stop();

            if (win) 
                OnWin();
            else 
                OnLose();
        }

        protected override void OnTimerEnd()
        {
            OnLose();
            wrestlers.Deactivate();
        }
    }
}