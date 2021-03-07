using UnityEngine;
using Player.Controller;

namespace Player
{
    /// <summary>
    /// Nico
    /// </summary>
    public class PlayerManager : Singleton<PlayerManager>
    {
        [Header("References")]
        public GameObject hubPlayer = null;
        public GameObject runPlayer = null;

        [HideInInspector] public PlayerController currentController = null;

        private void Awake()
        {
            CreateSingleton(true);
        }
    }
}