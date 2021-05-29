using UnityEngine;
using UnityEngine.SceneManagement;

namespace Map
{
    /// <summary>
    /// Nico
    /// </summary>
    public class BackToHubBehaviour : MonoBehaviour
    {
        [SerializeField] int index;
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
            {
                //GameManagement.GameManager.Instance.gameState.Set(GameManagement.GameState.Cinematic);
                switch (index)
                {
                    case 0:
                        SceneManager.LoadScene("Level1_1End");
                        break;
                    case 1:
                        SceneManager.LoadScene("Level1_2End");
                        break;
                }
            }
        }
    }
}