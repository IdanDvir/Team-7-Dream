using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private MainMenuScreen mainMenu;
    
    [SerializeField]
    private Screen victoryScreen;
    
    [SerializeField]
    private Screen lossScreen;
    
    [SerializeField]
    private Screen transitionScreen;
    
    [SerializeField]
    private List<MinigameScreen> minigames;

    [SerializeField] 
    private int maxLives;
    
    [SerializeField] 
    private int winsToWin;

    private Screen transitionInstance;
    private List<MinigameScreen> availableMinigames = new();
    private List<MinigameScreen> playedMinigames = new();
    private Screen currentScreen;
    private int lives;
    
    void Start()
    {
        transitionInstance = Instantiate(transitionScreen);
        transitionInstance.gameObject.SetActive(false);
        currentScreen = Instantiate(mainMenu);
        currentScreen.OnFinished += OnGameStart;
    }

    private void OnGameStart()
    {
        if (minigames == null || minigames.Count <= 0)
            return;
     
        var task = StartGame();
    }

    private async UniTask StartGame()
    {
        lives = maxLives;
        availableMinigames.Clear();
        availableMinigames.AddRange(minigames);
        playedMinigames.Clear();
        var nextMinigame = PickRandomMinigame(availableMinigames);
        await SwitchTo(nextMinigame);
    }

    private async UniTask SwitchTo(Screen screen)
    {
        var prevScreen = currentScreen;

        transitionInstance.gameObject.SetActive(true);
        await transitionInstance.Show();
        Destroy(prevScreen.gameObject);
        currentScreen = Instantiate(screen);

        var minigame = currentScreen.GetComponent<MinigameScreen>();
        if (minigame)
        {
            minigame.Win += OnMinigameWin;
            minigame.Lose += OnMinigameLose;
            playedMinigames.Add(screen as MinigameScreen);
        }
        
        await transitionInstance.Hide();
        transitionInstance.gameObject.SetActive(false);
        await currentScreen.Show();
        currentScreen.StartScreen();
    }

    private void OnMinigameLose()
    {
        lives -= 1;
        if (lives <= 0)
        {
            var nextMinigame = lossScreen;//PickRandomMinigame(availableMinigames);
            // var task = SwitchTo(nextMinigame);
        }
        else
        {
            var nextMinigame = lossScreen;//PickRandomMinigame(availableMinigames);
            var task = SwitchTo(nextMinigame);
        }
    }

    private void OnMinigameWin()
    {
        if (playedMinigames.Count >= winsToWin)
        {
            var nextMinigame = victoryScreen;//PickRandomMinigame(availableMinigames);
            var task = SwitchTo(nextMinigame);
        }
        else
        {
            var nextMinigame = victoryScreen;//PickRandomMinigame(availableMinigames);
            var task = SwitchTo(nextMinigame);
        }
    }

    private MinigameScreen PickRandomMinigame(List<MinigameScreen> minigames)
    {
        var randomIndex = Random.Range(0, minigames.Count);
        return minigames[randomIndex];
    }
}
