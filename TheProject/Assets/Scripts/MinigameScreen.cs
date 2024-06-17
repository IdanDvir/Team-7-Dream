using System;
using UnityEngine;

public delegate void OnGameComplete(); 

public abstract class MinigameScreen : Screen
{
    public event OnGameComplete Win; 
    public event OnGameComplete Lose;
    
    [SerializeField]
    private int minigameLengthSeconds;
    protected Stopwatch stopwatch;
    
    
    public override void OnShow()
    {
        stopwatch ??= new Stopwatch(minigameLengthSeconds, OnTimerEnd, this);
    }

    public override void StartScreen()
    {
        stopwatch.Start();
    }

    public override void OnHide()
    {
        stopwatch.Stop();
    }

    protected abstract void OnTimerEnd();
}