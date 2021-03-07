using UnityEngine;
using GameManagement;
using UnityEngine.SceneManagement;

namespace MainMenu
{
    public class MainMenuButtons : MonoBehaviour
    {
        public void LaunchGame()
        {
            SceneManager.LoadScene("Hub");
        }

    }
}