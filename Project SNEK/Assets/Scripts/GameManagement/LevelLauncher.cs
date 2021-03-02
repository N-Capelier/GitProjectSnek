using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagement
{
    /// <summary>
    /// Nico
    /// </summary>
    public class LevelLauncher : MonoBehaviour
    {
        public IEnumerator StartLevel(string _levelName)
        {
            AsyncOperation _newScene;
            switch (_levelName)
            {
                case "Hub":
                    _newScene = SceneManager.LoadSceneAsync("Hub");
                    yield return new WaitUntil(() => _newScene.isDone);
                    GameManager.Instance.gameState.Set(GameState.Hub);
                    break;
                case "Run":
                    _newScene = SceneManager.LoadSceneAsync("Nico");
                    yield return new WaitUntil(() => _newScene.isDone);
                    GameManager.Instance.gameState.Set(GameState.Run);
                    break;
                default:
                    break;
            }
        }
    }
}