using Cysharp.Threading.Tasks;

public class TransitionScreen : Screen
{
    public override void StartScreen()
    {
        
    }
    
    public override async UniTask Show()
    {
        await base.Show();
        await UniTask.Delay(1000);
    }
}