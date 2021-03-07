using UnityEngine;
using GameManagement;
using UnityEngine.SceneManagement;

namespace Hub
{
    public class HubButtons : MonoBehaviour
    {

        public void StartLevel1()
        {
            //GameManager.Instance.gameState.Set(GameState.Run, "Level1_1");
            SceneManager.LoadScene("Level1_1");
        }

        public void StartLevel2()
        {
            //GameManager.Instance.gameState.Set(GameState.Run, "Level1_2");
            SceneManager.LoadScene("Level1_2");
        }

        public void StartLevel3()
        {
            //GameManager.Instance.gameState.Set(GameState.Run, "Level1_3");
            SceneManager.LoadScene("Level1_3");
        }

    }
}