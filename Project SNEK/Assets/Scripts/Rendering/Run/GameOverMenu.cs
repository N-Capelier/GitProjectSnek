using UnityEngine;
using UnityEngine.SceneManagement;
using Saving;
using GameManagement;

namespace Rendering.Run
{
    public class GameOverMenu : Singleton<GameOverMenu>
    {
        public GameObject menuCanvas;

        private void Awake()
        {
            CreateSingleton();
        }

        public void Retry()
        {
            SaveManager.Instance.Save();
            GameManager.Instance.gameState.Set(GameState.Dialog);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void BackToHub()
        {
            GameManagement.GameManager.Instance.gameState.Set(GameManagement.GameState.Hub, "Hub");
        }

    }
}

