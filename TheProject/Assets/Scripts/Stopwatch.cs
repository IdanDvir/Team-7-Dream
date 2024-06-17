using System;
using System.Collections;
using UnityEngine;

public class Stopwatch
{
    private int seconds;
    private Action onStop;
    private MonoBehaviour context;
    private Coroutine countdownCoroutine;
    
    private float endTime;
    public Stopwatch(int seconds, Action onStop, MonoBehaviour context)
    {
        this.onStop = onStop;
        this.seconds = seconds;
        this.context = context;
    }
    
    public void Start()
    {
        endTime = Time.time + seconds;
        countdownCoroutine = context.StartCoroutine(Countdown());
    }

    public void Stop()
    {
        if (countdownCoroutine != null)
            context.StopCoroutine(countdownCoroutine);
    }

    private IEnumerator Countdown()
    {
        yield return new WaitUntil(() => Time.time >= endTime);
        onStop();
    }
}