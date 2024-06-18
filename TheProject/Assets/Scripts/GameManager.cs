using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

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
    private StopwatchView stopwatchViewPrefab;

    [SerializeField] 
    private LifeView lifeViewPrefab;

    [SerializeField]
    private MiniGameTitleView miniGameTitleViewPrefab;
    
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
    private int wins;
    private StopwatchView stopwatchView;
    private LifeView lifeView;
    private MiniGameTitleView miniGameTitleView;

    void Start()
    {
        miniGameTitleView= Instantiate(miniGameTitleViewPrefab);
        miniGameTitleView.gameObject.SetActive(false);
        lifeView = Instantiate(lifeViewPrefab);
        lifeView.gameObject.SetActive(false);
        stopwatchView = Instantiate(stopwatchViewPrefab);
        stopwatchView.gameObject.SetActive(false);
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
        
        stopwatchView.Pause();
        await UniTask.WaitForSeconds(currentScreen.hideDelay);
        transitionInstance.gameObject.SetActive(true);
        await transitionInstance.Show();
        lifeView.Set(lives);
        Destroy(prevScreen.gameObject);
        currentScreen = Instantiate(screen);

        var minigame = currentScreen.GetComponent<MinigameScreen>();
        if (minigame)
        {
            stopwatchView.Reset();
            stopwatchView.gameObject.SetActive(true);
            lifeView.gameObject.SetActive(true);
            minigame.Win += OnMinigameWin;
            minigame.Lose += OnMinigameLose;
            playedMinigames.Add(screen as MinigameScreen);
            minigame.dont = Random.Range(0.0f, 100.0f) > 50.0f;
        }
        else
        {
            lifeView.gameObject.SetActive(false);
            stopwatchView.gameObject.SetActive(false);
            miniGameTitleView.gameObject.SetActive(false);
        }
        
        await currentScreen.Show();
        await transitionInstance.Hide();
        if (minigame)
        {
            miniGameTitleView.gameObject.SetActive(true);
            await miniGameTitleView.Show(minigame.Title, minigame.dont);
            miniGameTitleView.gameObject.SetActive(false);
        }
        transitionInstance.gameObject.SetActive(false);

        currentScreen.StartScreen(stopwatchView);
    }

    private void OnMinigameLose()
    {
        lives -= 1;
        if (lives <= 0)
        {
            var nextMinigame = lossScreen;//;
            var task = SwitchTo(nextMinigame);
        }
        else
        {
            var nextMinigame = PickRandomMinigame(availableMinigames);
            var task = SwitchTo(nextMinigame);
        }
    }

    private void OnMinigameWin()
    {
        wins++;
        if (wins >= winsToWin)
        {
            var nextMinigame = victoryScreen;
            var task = SwitchTo(nextMinigame);
        }
        else
        {
            var nextMinigame = PickRandomMinigame(availableMinigames);
            var task = SwitchTo(nextMinigame);
        }
    }

    private MinigameScreen PickRandomMinigame(List<MinigameScreen> minigames)
    {
        var randomIndex = Random.Range(0, minigames.Count);
        return minigames[randomIndex];
    }
}
