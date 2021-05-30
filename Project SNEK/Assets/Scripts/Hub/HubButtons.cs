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

        public void StartLevel(string levelName)
        {
            //GameManager.Instance.gameState.Set(GameState.Run, "Level1_1");
            SaveManager.Instance.Save();
            SceneManager.LoadScene(levelName);
        }
    }
}