using Cysharp.Threading.Tasks;
using UnityEngine;

public delegate void OnGameComplete(); 

public abstract class MinigameScreen : Screen
{
    public bool dont;
    public event OnGameComplete Win; 
    public event OnGameComplete Lose;
    
    [SerializeField]
    private int minigameLengthSeconds;
    protected Stopwatch stopwatch;
    
    
    public override async UniTask Show()
    {
        await base.Show();
        stopwatch ??= new Stopwatch(minigameLengthSeconds, OnTimerEnd, this);
    }

    public override void StartScreen()
    {
        stopwatch.Start();
    }

    public override async UniTask Hide()
    {
        await base.Hide();
        stopwatch.Stop();
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