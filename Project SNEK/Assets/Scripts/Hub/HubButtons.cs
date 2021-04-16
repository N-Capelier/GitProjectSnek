using UnityEngine;
using UnityEngine.SceneManagement;
using Saving;

namespace Hub
{
    /// <summary>
    /// Nico
    /// </summary>
    public class HubButtons : MonoBehaviour
    {

        public void StartLevel1()
        {
            //GameManager.Instance.gameState.Set(GameState.Run, "Level1_1");
            SaveManager.Instance.Save();
            SceneManager.LoadScene("Level1_1");
        }

        public void StartLevel2()
        {
            //GameManager.Instance.gameState.Set(GameState.Run, "Level1_2");
            SaveManager.Instance.Save();
            SceneManager.LoadScene("Level1_2");
        }

        public void StartLevel3()
        {
            //GameManager.Instance.gameState.Set(GameState.Run, "Level1_3");
            SaveManager.Instance.Save();
            SceneManager.LoadScene("Level1_3");
        }

    }
}