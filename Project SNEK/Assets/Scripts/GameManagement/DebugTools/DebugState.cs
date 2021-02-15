using UnityEngine;

namespace GameManagement.DebugTools
{
    /// <summary>
    /// Nico
    /// </summary>
    public class DebugState : MonoBehaviour
    {

        [SerializeField] GameState gameState;
        [SerializeField] bool forceState = false;

        private void Update()
        {
            if (forceState)
            {
                GameManager.Instance.gameState.Set(gameState);
                forceState = false;
            }
        }

    }
}