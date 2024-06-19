using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
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
    private Screen creditsScreen;
    
    [SerializeField] 
    private StopwatchView stopwatchViewPrefab;

    [SerializeField] 
    private LifeView lifeViewPrefab;

    [SerializeField]
    private MiniGameTitleView miniGameTitleViewPrefab;
    
    [SerializeField]
    private List<MinigameScreen> minigames;

    [SerializeField] 
    private GameObject uiGradient;
    
    [SerializeField] 
    private int maxLives;
    
    [SerializeField] 
    private int winsToWin;

    [SerializeField]
    private AudioSource sfxSource;
    
    [SerializeField] 
    private AudioSource mainTheme;

    [SerializeField] 
    private AudioSource gameTheme;

    [SerializeField] 
    private AudioClip doorsOpen;

    [SerializeField] 
    private AudioClip doorsClose;

    [SerializeField] 
    private AudioClip gameWon;

    [SerializeField] 
    private AudioClip gameLost;

    [SerializeField] private float timeScaleDamp;
    
    private Screen transitionInstance;
    private List<MinigameScreen> availableMinigames = new();
    private List<MinigameScreen> playedMinigames = new();
    private Screen currentScreen;
    private int lives;
    private int wins;
    private StopwatchView stopwatchView;
    private LifeView lifeView;
    private MiniGameTitleView miniGameTitleView;
    private float gameThemeVolume;

    void Start()
    {
        Application.targetFrameRate = 120;
        miniGameTitleView= Instantiate(miniGameTitleViewPrefab);
        miniGameTitleView.gameObject.SetActive(false);
        lifeView = Instantiate(lifeViewPrefab);
        lifeView.gameObject.SetActive(false);
        stopwatchView = Instantiate(stopwatchViewPrefab);
        stopwatchView.gameObject.SetActive(false);
        transitionInstance = Instantiate(transitionScreen);
        transitionInstance.gameObject.SetActive(false);
        uiGradient.SetActive(false);
        currentScreen = Instantiate(mainMenu);
        mainTheme.Play();
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
        mainTheme.DOFade(0.0f, 0.5f).onComplete += () =>
        {
            mainTheme.Stop();
        };
        gameThemeVolume = gameTheme.volume;
        gameTheme.volume = 0.0f;
        gameTheme.Play();
        gameTheme.DOFade(gameThemeVolume, 0.5f);
        lives = maxLives;
        availableMinigames.Clear();
        availableMinigames.AddRange(minigames);
        var nextMinigame = PickRandomMinigame(availableMinigames);
        await SwitchTo(nextMinigame);
    }

    private async UniTask SwitchTo(Screen screen)
    {
        var prevScreen = currentScreen;
        
        stopwatchView.Pause();
        gameTheme.DOFade(gameThemeVolume/2.0F, 0.5f);
        await currentScreen.DoExtraEnd();
        await UniTask.WaitForSeconds(currentScreen.hideDelay);
        transitionInstance.gameObject.SetActive(true);
        sfxSource.PlayOneShot(doorsOpen);
        await transitionInstance.Show();
        await currentScreen.Hide();
        lifeView.Set(lives);
        Destroy(prevScreen.gameObject);
        currentScreen = Instantiate(screen);

        var minigame = currentScreen.GetComponent<MinigameScreen>();
        if (minigame)
        {
            availableMinigames.Remove(screen as MinigameScreen);
            if (availableMinigames.Count <= 0)
                availableMinigames.AddRange(minigames);
            
            Time.timeScale += timeScaleDamp;
            stopwatchView.Reset();
            stopwatchView.gameObject.SetActive(true);
            lifeView.gameObject.SetActive(true);
            uiGradient.SetActive(true);
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
            uiGradient.SetActive(false);
            currentScreen.ShowCredits += ShowCredits;
        }
        
        await currentScreen.Show();
        sfxSource.PlayOneShot(doorsClose);
        gameTheme.DOFade(gameThemeVolume, 0.5f);
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
        sfxSource.PlayOneShot(gameLost);
        if (lives <= 0)
        {
            var nextMinigame = lossScreen;
            var task = SwitchTo(nextMinigame);
            gameTheme.Stop();
        }
        else
        {
            var nextMinigame = PickRandomMinigame(availableMinigames);
            var task = SwitchTo(nextMinigame);
        }
    }

    private void ShowCredits()
    {
        var nextMinigame = creditsScreen;
        var task = SwitchTo(nextMinigame);
    }

    private void OnMinigameWin()
    {
        wins++;
        sfxSource.PlayOneShot(gameWon);
        if (wins >= winsToWin)
        {
            var nextMinigame = victoryScreen;
            var task = SwitchTo(nextMinigame);
            gameTheme.Stop();
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
