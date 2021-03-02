using UnityEngine;

namespace GameManagement.DebugTools
{
    /// <summary>
    /// Nico
    /// </summary>
    public class DebugState : MonoBehaviour
    {

        [SerializeField] GameState gameState;
        [SerializeField] string sceneName;

        [Space]
        [SerializeField] bool forceState = false;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (forceState)
            {
                GameManager.Instance.gameState.Set(gameState, sceneName);
                forceState = false;
            }
        }

    }
}