using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class LossScreen : Screen
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private AudioSource darkSouls;
    public override UniTask Show()
    {
        group.alpha = 0;
        return base.Show();
    }

    public override void StartScreen(StopwatchView stopWatchView)
    {
        WaitBeforeLoadingScene().Forget();
    }

    private async UniTask WaitBeforeLoadingScene()
    {
        darkSouls.Play();
        group.DOFade(1.0f, 3.0f);
        await UniTask.WaitForSeconds(5.0f);
        SceneManager.LoadScene(0);
    }
}