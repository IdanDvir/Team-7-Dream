using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils;

public delegate void OnFinished();
public abstract class Screen : MonoBehaviour
{
    [SerializeField] public float hideDelay = 0.7f;
    public event OnFinished OnFinished;
    public event OnFinished ShowCredits;
    public UniTaskCompletionSource OnFinishedTask = new UniTaskCompletionSource();
    
    public virtual async UniTask Show()
    {
        await UniTask.CompletedTask;
    }

    public virtual async UniTask DoExtraStart()
    {
        await UniTask.CompletedTask;
    }
    
    public virtual async UniTask DoExtraEnd()
    {
        await UniTask.CompletedTask;
    }

    public virtual async UniTask Hide()
    {
        await UniTask.WaitForSeconds(hideDelay);
            OnFinished?.Invoke();
            OnFinished = null;
    }

    protected void DisplayCredits()
    {
        ShowCredits?.Invoke();
    }
    
    public abstract void StartScreen(StopwatchView stopWatchView);
}