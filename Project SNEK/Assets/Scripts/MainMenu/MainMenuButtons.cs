using UnityEngine;
using UnityEngine.SceneManagement;
using Saving;

namespace MainMenu
{
    /// <summary>
    /// Nico
    /// </summary>
    public class MainMenuButtons : MonoBehaviour
    {
        public void LaunchGame()
        {
            if(SaveManager.Instance.state.bergamotState > 1)
            {
                print("Hub");
                SceneManager.LoadScene("Hub");
            }
            else
            {
                print("intro");
                SceneManager.LoadScene("TutorialIntro");
            }
        }
    }
}