using UnityEngine;

namespace Utils
{
    public class LifeView : MonoBehaviour
    {
        [SerializeField] private GameObject[] hearts;

        public void Set(int lifes)
        {
            for (var i = hearts.Length - 1; i > lifes - 1; i--)
            {
                hearts[i].SetActive(false);
            }
        }
    }
}