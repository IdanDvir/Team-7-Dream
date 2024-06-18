using UnityEngine;
using Utils;

namespace MiniGames.NotTheBees
{
    public class MiniGameQueenB : MinigameScreen
    {
        [SerializeField] private Bee[] bees;
        [SerializeField] private Tentacle tentacle;

        public override void StartScreen(StopwatchView stopWatchView)
        {
            base.StartScreen(stopWatchView);
            foreach (var bee in bees)
            {
                bee.Activate(tentacle.transform.position, OnReachedTarget, OnDeath);
            }
            
            tentacle.Activate();
        }

        private void OnDeath()
        {
            foreach (var bee in bees)
            {
                if (bee != null)
                {
                    return;
                }
            }

            stopwatch.Stop();
            tentacle.Stop();
            OnWin();
        }

        private void OnReachedTarget()
        {
            foreach (var bee in bees)
            {
                bee.Stop();
            }
            
            tentacle.Stop();
            stopwatch.Stop();
            OnLose();
        }

        protected override void OnTimerEnd()
        {
            foreach (var bee in bees)
            {
                bee.Stop();
            }
            
            tentacle.Stop();
            OnWin();
        }
    }
}