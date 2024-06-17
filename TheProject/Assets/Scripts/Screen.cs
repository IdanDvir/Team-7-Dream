using UnityEngine;

public abstract class Screen : MonoBehaviour
{
    public abstract void OnShow();
    public abstract void OnHide();
    public abstract void StartScreen();
}