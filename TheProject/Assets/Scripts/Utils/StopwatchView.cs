using System;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    public class StopwatchView : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private RectTransform edgeRect;
        private Stopwatch stopwatch;
        private bool isActive;

        public void Activate(Stopwatch stopwatch)
        {
            this.stopwatch = stopwatch;
            isActive = true;
        }

        public void Reset()
        {
            fillImage.fillAmount = 1;
            var tempV = GetEdgePosition(1);
            edgeRect.anchoredPosition = tempV;
            isActive = false;
        }

        private void Update()
        {
            if (!isActive)
            {
                return;
            }

            var fillAmount = stopwatch.Ratio();
            fillImage.fillAmount = fillAmount;
            
            var tempV = GetEdgePosition(fillAmount);
            edgeRect.anchoredPosition = tempV;
        }

        private Vector3 GetEdgePosition(float fillAmount)
        {
            var width = fillImage.rectTransform.rect.width;
            Vector3 tempV = edgeRect.anchoredPosition;
            tempV.x = -width / 2;
            tempV.x += width * fillAmount;
            return tempV;
        }
    }
}