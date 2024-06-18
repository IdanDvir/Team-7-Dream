using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils;

namespace MiniGames.SumoWrestling
{
    public class MiniGameSumoWrestling : MinigameScreen
    {
        [SerializeField] private Clickable wrestlers;
        [SerializeField] private GameObject deathLeft;
        [SerializeField] private GameObject deathRight;
        
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
            wrestlers.Activate();
            wrestlers.Triggered2D += OnTriggered2D;
        }

        private void OnTriggered2D(Collider2D source, Collider2D other)
        {
            var win = other.gameObject == deathRight;
            wrestlers.Stop();

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