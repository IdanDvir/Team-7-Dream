using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

namespace MiniGames.Poke
{
    public class PokeMinigame : MinigameScreen, IPointerDownHandler
    {
        [SerializeField] private GameObject THE_EYE;
        [SerializeField] private GameObject OWWW;

        private bool isActive = false;

        public override void StartScreen(StopwatchView stopWatchView)
        {
            base.StartScreen(stopWatchView);
            isActive = true;
        }

        protected override void OnTimerEnd()
        {
            OnLose();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (isActive)
            {
                THE_EYE.SetActive(false);
                OWWW.SetActive(true);
                OnWin();
            }
                
        }
    }
}