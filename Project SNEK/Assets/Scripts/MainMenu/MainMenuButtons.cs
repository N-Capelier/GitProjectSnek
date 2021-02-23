using UnityEngine;
using GameManagement;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class MainMenuButtons : MonoBehaviour
    {
        public void LaunchGame()
        {
            GameManager.Instance.StartCoroutine(GameManager.Instance.levelLauncher.StartLevel("Hub"));
        }

    }
}