using UnityEngine;

namespace GameManagement
{
    /// <summary>
    /// Nico
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        [Header("References")]
        public StateMachine gameState = null;
        public InputHandler inputHandler = null;

        private void Awake()
        {
            CreateSingleton(true);
        }
    }
}