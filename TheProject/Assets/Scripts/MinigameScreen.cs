using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils;

public delegate void OnGameComplete(); 

public abstract class MinigameScreen : Screen
{
    public bool dont;
    public event OnGameComplete Win; 
    public event OnGameComplete Lose;
    
    [SerializeField]
    private int minigameLengthSeconds;

    [SerializeField] private string title;
    
    protected Stopwatch stopwatch;

    public string Title => title;
    public override async UniTask Show()
    {
        await base.Show();
        stopwatch ??= new Stopwatch(minigameLengthSeconds, OnTimerEnd, this);
    }

    public override void StartScreen(StopwatchView stopWatchView)
    {
        if (stopWatchView)
        {
            stopwatch.Start();
            stopWatchView.Activate(stopwatch);
        }
    }

    public override async UniTask Hide()
    {
        stopwatch.Stop();
        await base.Hide();
        
        Win = null;
        Lose = null;
    }

    protected abstract void OnTimerEnd();

    protected void OnWin()
    {
        if (dont)
            Lose?.Invoke();
        else
            Win?.Invoke();
    }

    protected void OnLose()
    {
        if (dont)
            Win?.Invoke();
        else
            Lose?.Invoke();
    }
}