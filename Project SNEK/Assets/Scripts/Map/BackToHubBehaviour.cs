using UnityEngine;
using GameManagement;

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
            if (other.CompareTag("Player"))
            {
                //GameManagement.GameManager.Instance.gameState.Set(GameManagement.GameState.Cinematic);
                switch (index)
                {
                    case 0:
                        GameManager.Instance.gameState.Set(GameState.Hub, "Hub");

                        break;
                    case 1:
                        GameManager.Instance.gameState.Set(GameState.Cinematic, "Level1_1End");
                        break;
                    case 2:
                        GameManager.Instance.gameState.Set(GameState.Cinematic, "Level1_2End");
                        break;
                    case 3:
                        GameManager.Instance.gameState.Set(GameState.Cinematic, "Level2_1End");
                        break;
                    case 4:
                        GameManager.Instance.gameState.Set(GameState.Cinematic, "Level2_2End");
                        break;
                }
            }
        }
    }
}