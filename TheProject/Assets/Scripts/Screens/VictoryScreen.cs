using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

public class VictoryScreen : Screen
{
    [SerializeField] private AudioSource pirate;

    public override UniTask Show()
    {
        WaitBeforeLoadingScene().Forget();
        return base.Show();
    }

    public override void StartScreen(StopwatchView stopWatchView)
    {
        
    }

    private async UniTask WaitBeforeLoadingScene()
    {
        pirate.Play();
        var length = pirate.clip.length;
        await UniTask.WaitForSeconds(length + 1.0f);
        DisplayCredits();
    }
}