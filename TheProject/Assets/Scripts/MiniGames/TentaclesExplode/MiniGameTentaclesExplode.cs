using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils;

namespace MiniGames.TentaclesExplode
{
    public class MiniGameTentaclesExplode : MinigameScreen
    {
        [SerializeField] private TentaclesJuggleBomb firstJuggleBomb;
        [SerializeField] private TentaclesJuggleBomb secondJuggleBomb;
        [SerializeField] private TentaclesJuggleBomb thirdJuggleBomb;
        [SerializeField] private Transform target;
        [SerializeField] private float firstLaunchForce;
        [SerializeField] private float gravity = -4.81f;
        [SerializeField] private AudioSource clinicalDeath;
        [SerializeField] private GameObject explosionEffect;
        
        private Vector3 previousGravity;
        private GameObject[] bombEffects;

        public override async UniTask Show()
        {
            await base.Show();
            firstJuggleBomb.Init(OnDrop);
            secondJuggleBomb.Init(OnDrop);
            thirdJuggleBomb.Init(OnDrop);
            previousGravity = Physics.gravity;
            Physics.gravity = new Vector3(0.0f, gravity, 0.0f);
        }

        public override async UniTask Hide()
        {
            await base.Hide();
            Physics.gravity = previousGravity;
            foreach (var bomb in bombEffects)
            {
                Destroy(bomb);
            }
        }

        private void OnDrop()
        {
            firstJuggleBomb.Stop();
            secondJuggleBomb.Stop();
            thirdJuggleBomb.Stop();
            stopwatch.Stop();
            OnLose();
        }

        public override void StartScreen(StopwatchView stopWatchView)
        {
            base.StartScreen(stopWatchView);
            StartGame().Forget();
        }

        private async UniTask StartGame()
        {
            var firstJuggleForce = GetLaunchForce(firstJuggleBomb.transform.position);
            var secondJuggleForce = GetLaunchForce(secondJuggleBomb.transform.position);
            var thirdJuggleForce = GetLaunchForce(thirdJuggleBomb.transform.position);
            await UniTask.WhenAll(firstJuggleBomb.FirstLaunch(firstJuggleForce, Random.Range(0.5f, 1.0f)),
                secondJuggleBomb.FirstLaunch(secondJuggleForce, Random.Range(0.5f, 1.0f)),
                thirdJuggleBomb.FirstLaunch(thirdJuggleForce, Random.Range(0.5f, 1.0f)));
        }

        private Vector3 GetLaunchForce(Vector3 position)
        {
            return (Vector3.up + (target.position - position)) * firstLaunchForce;
        }

        protected override void OnTimerEnd()
        {
            firstJuggleBomb.Stop();
            secondJuggleBomb.Stop();
            thirdJuggleBomb.Stop();
            stopwatch.Stop();
            OnWin();
        }

        public override async UniTask DoExtraEnd()
        {
            await base.DoExtraEnd();
            var locations = new[]
            {
                firstJuggleBomb.transform.position,
                secondJuggleBomb.transform.position,
                thirdJuggleBomb.transform.position
            };
            Destroy(firstJuggleBomb.gameObject);
            Destroy(secondJuggleBomb.gameObject);
            Destroy(thirdJuggleBomb.gameObject);
            bombEffects = new[]
            {
                Instantiate(explosionEffect, locations[0], Quaternion.identity),
                Instantiate(explosionEffect, locations[1], Quaternion.identity),
                Instantiate(explosionEffect, locations[2], Quaternion.identity),
            };
            await UniTask.WaitForSeconds(0.2f);
            clinicalDeath.Play();
            await UniTask.WaitForSeconds(1f);
        }
    }
}