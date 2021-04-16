using UnityEngine;

namespace Map
{
    /// <summary>
    /// Nico
    /// </summary>
    public class BackToHubBehaviour : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Player"))
                GameManagement.GameManager.Instance.gameState.Set(GameManagement.GameState.Hub, "Hub");
        }
    }
}