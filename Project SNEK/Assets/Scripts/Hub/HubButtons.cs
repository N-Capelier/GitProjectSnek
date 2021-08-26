using UnityEngine;
using UnityEngine.SceneManagement;
using Saving;
using GameManagement;

namespace Hub
{
    /// <summary>
    /// Nico
    /// </summary>
    public class HubButtons : MonoBehaviour
    {

        public void StartLevel(string levelName)
        {
            SaveManager.Instance.Save();
            StartCoroutine(GameManager.Instance.gameState.sceneTransition.AlphaUp(0.05f, levelName));
        }
    }
}