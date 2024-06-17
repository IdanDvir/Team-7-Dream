using Cysharp.Threading.Tasks;
using UnityEngine;

public class MiniGameTester : MonoBehaviour
{
    [SerializeField] private float showAfterSeconds;
    [SerializeField] private float startAfterSeconds;
    [SerializeField] private MinigameScreen minigameScreen;
    private bool minigameLost;
    private bool minigameWon;

    private void Start()
    {
        if (minigameScreen == null)
        {
            return;
        }
        
        StartMiniGame().ContinueWith(OnMinigameComplete);
    }
    
    private async UniTask StartMiniGame()
    {
        await UniTask.WaitForSeconds(showAfterSeconds);
        await minigameScreen.Show();
        await UniTask.WaitForSeconds(startAfterSeconds);
        minigameScreen.Win += OnWon;
        minigameScreen.Lose += OnLose;
        minigameScreen.StartScreen();
        await UniTask.WaitUntil(MinigameOver);
        await minigameScreen.Hide();
    }

    private bool MinigameOver()
    {
        return minigameLost || minigameWon;
    }
    
    private void OnLose()
    {
        minigameLost = true;
    }

    private void OnWon()
    {
        minigameWon = true;
    }

    private void OnMinigameComplete()
    {
        Debug.Log("Finished minigame");
    }

   
}