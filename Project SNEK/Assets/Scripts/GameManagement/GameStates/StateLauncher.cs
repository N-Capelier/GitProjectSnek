using UnityEngine;

namespace GameManagement
{
    /// <summary>
    /// Nico
    /// </summary>
    public class StateLauncher : MonoBehaviour
    {
        [SerializeField] GameState gameState;

        [SerializeField] bool setAlphaDown;

        private void Start()
        {
            GameManager.Instance.gameState.Set(gameState);

            if(setAlphaDown)
            {
                GameManager.Instance.gameState.SetAlphaDown();
            }
        }

    }
}