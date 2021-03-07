using UnityEngine;

namespace GameManagement
{
    public class StateLauncher : MonoBehaviour
    {
        [SerializeField] GameState gameState;

        private void Start()
        {
            GameManager.Instance.gameState.Set(gameState);
        }

    }
}