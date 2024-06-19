using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Screens
{
    public class CreditsScreen : Screen
    {
        [SerializeField] private AudioSource thankYou;

        public override UniTask Show()
        {
            WaitForSecondsPlease().Forget();
            return base.Show();
        }

        private async UniTask WaitForSecondsPlease()
        {
            thankYou.Play();
            await UniTask.WaitForSeconds(12.0f);
            SceneManager.LoadScene(0);
        }

        public override void StartScreen(StopwatchView stopWatchView)
        {
        }
    }
}