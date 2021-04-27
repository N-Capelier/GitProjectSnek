using UnityEngine;
using UnityEngine.SceneManagement;
using Saving;
using AudioManagement;
using GameManagement;

namespace MainMenu
{
    /// <summary>
    /// Nico
    /// </summary>
    public class MainMenuButtons : MonoBehaviour
    {
        public void LaunchGame()
        {
            AudioManager.Instance.PlaySoundEffect("UIConfirm");
            if (SaveManager.Instance.state.bergamotState > 1)
            {
                GameManager.Instance.gameState.Set(GameManager.Instance.gameState.ActiveState, "Hub");
                //SceneManager.LoadScene("Hub");
            }
            else
            {
                GameManager.Instance.gameState.Set(GameManager.Instance.gameState.ActiveState, "TutorialIntro");
                //SceneManager.LoadScene("TutorialIntro");
            }
        }

        public void ResetSave()
        {
            AudioManager.Instance.PlaySoundEffect("UIClick");
            Debug.Log("Reseting Save");
            SaveManager.Instance.state = new SaveState();
            SaveManager.Instance.Save();
        }
    }
}