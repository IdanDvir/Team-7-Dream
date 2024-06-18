using Cysharp.Threading.Tasks;
using UnityEngine;

public class TransitionScreen : Screen
{
    [SerializeField] private Animator transitionAnimator;
    private static readonly int Out = Animator.StringToHash("Out");

    public override void StartScreen()
    {
        
    }
    
    public override async UniTask Show()
    {
        await base.Show();
        await UniTask.Delay(1000);
    }

    public override async UniTask Hide()
    {
        await base.Hide();
        transitionAnimator.SetTrigger(Out);
        await UniTask.WaitForSeconds(2);
    }
}