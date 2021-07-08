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
        public UIHandler uiHandler = null;

        [HideInInspector] public bool playedBossCinematic = false;

        private void Awake()
        {
            CreateSingleton(true);
            Application.targetFrameRate = 30;
        }
    }
}