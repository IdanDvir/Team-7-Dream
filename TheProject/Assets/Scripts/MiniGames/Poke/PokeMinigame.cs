using Cysharp.Threading.Tasks;
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
        private bool didWin;

        public override void StartScreen(StopwatchView stopWatchView)
        {
            base.StartScreen(stopWatchView);
            isActive = true;
        }

        protected override void OnTimerEnd()
        {
            isActive = false;
            OnLose();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (isActive)
            {
                didWin = true;
                isActive = false;
                OnWin();
            }
        }

        public override async UniTask DoExtraEnd()
        {
            await base.DoExtraEnd();
            if (didWin)
            {
                THE_EYE.SetActive(false);
                OWWW.SetActive(true);
                await UniTask.WaitForSeconds(0.7f);
            }
        }
    }
}