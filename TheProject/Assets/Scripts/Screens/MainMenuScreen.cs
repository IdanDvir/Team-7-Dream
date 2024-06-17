using UnityEngine;
using UnityEngine.UI;

public class MainMenuScreen : Screen
{
    [SerializeField]
    public Button startButton;
    
    public void Awake()
    {
        startButton.onClick.AddListener(OnStartButtonClick);
    }

    private void OnStartButtonClick()
    {
        var task = Hide();
    }

    public override void StartScreen()
    {
    }
}