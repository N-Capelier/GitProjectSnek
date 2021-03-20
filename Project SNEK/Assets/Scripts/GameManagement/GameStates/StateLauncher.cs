using UnityEngine;

namespace GameManagement
{
    /// <summary>
    /// Nico
    /// </summary>
    public class StateLauncher : MonoBehaviour
    {
        [SerializeField] GameState gameState;

        private void Start()
        {
            GameManager.Instance.gameState.Set(gameState);
        }

    }
}