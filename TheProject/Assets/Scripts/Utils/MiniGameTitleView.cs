using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Utils
{
    public class MiniGameTitleView : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private GameObject dontObject;
        [SerializeField] private CanvasGroup group;
        [SerializeField] private float fadeDuration;
        private bool finishedFade;

        public async UniTask Show(string text, bool dont)
        {
            titleText.text = text;
            dontObject.SetActive(dont);
            group.alpha = 0.0f;
            await Fade(1.0f);
            await UniTask.WaitForSeconds(0.5f);
            await Fade(0.0f);
        }

        private async UniTask Fade(float endValue)
        {
            group.DOFade(endValue, fadeDuration).onComplete += () =>
            {
                finishedFade = true;
            };
            await UniTask.WaitUntil(FinishedFade);
            finishedFade = false;
        }
        private bool FinishedFade()
        {
            return finishedFade;
        }
    }
}