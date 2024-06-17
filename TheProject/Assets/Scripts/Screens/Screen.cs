using Cysharp.Threading.Tasks;
using UnityEngine;

public delegate void OnFinished();
public abstract class Screen : MonoBehaviour
{
    public event OnFinished OnFinished;
    public UniTaskCompletionSource OnFinishedTask = new UniTaskCompletionSource();
    
    public virtual async UniTask Show()
    {
        await UniTask.CompletedTask;
    }

    public virtual async UniTask Hide()
    {
        OnFinished?.Invoke();
        OnFinished = null;
        await UniTask.CompletedTask;
    }
    
    public abstract void StartScreen();
}