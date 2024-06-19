using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils;

namespace MiniGames.BananaMinigame
{
    public class BananaMinigame : MinigameScreen, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Image bananananana;
        [SerializeField] private List<Sprite> banananananaBananananas;
        [SerializeField] private Sprite thisIsMyFinalForm;
        [SerializeField] private float dragDistance;

        private float yDragDiff = 0;
        private bool isDragging;
        private bool isActive;
        private int currentBanananana = 0;
        private Vector3 lastPosition;
        private bool peeledBanana;

        public override void StartScreen(StopwatchView stopWatchView)
        {
            base.StartScreen(stopWatchView);
            isActive = true;
            currentBanananana = 0;
            isDragging = false;
            yDragDiff = 0;
        }

        protected override void OnTimerEnd()
        {
            OnLose();
        }

        void Update()
        {
            if (!isDragging)
                return;
            
            var mousePosition = Input.mousePosition;
            var moveDirection = mousePosition - lastPosition;
            lastPosition = mousePosition;
            yDragDiff += moveDirection.y;

            if (-yDragDiff > dragDistance)
            {
                currentBanananana++;
                yDragDiff = 0;
                isDragging = false;
                bananananana.sprite = banananananaBananananas[currentBanananana];
                if (currentBanananana == banananananaBananananas.Count - 1)
                {
                    peeledBanana = true;
                    isActive = false;
                    stopwatch.Stop();
                    OnWin();
                }
            }
        }

        private async UniTask finalTransformation()
        {
            await UniTask.WaitForSeconds(0.5f);
            bananananana.sprite = thisIsMyFinalForm;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!isActive)
                return;

            lastPosition = eventData.position;
            yDragDiff = 0;
            isDragging = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!isActive || !isDragging)
            {
                return;
            }

            lastPosition = Vector3.zero;
            yDragDiff = 0;
            isDragging = false;
        }

        public override async UniTask DoExtraEnd()
        {
            await base.DoExtraEnd();
            if (peeledBanana)
            {
                await finalTransformation();
            }
        }
    }
}