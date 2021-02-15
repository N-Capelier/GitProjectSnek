using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Controller;
using GameManagement;

namespace Player
{
    /// <summary>
    /// Nico
    /// </summary>
    public class PlayerManager : MonoBehaviour
    {
        [Header("References")]
        public GameObject hubPlayer = null;
        public GameObject runPlayer = null;

        [HideInInspector] public PlayerController currentController = null;

        public void InitController(GameState _state)
        {
            GameObject _stateObject = null;

            switch(_state)
            {
                case GameState.Hub:
                    _stateObject = hubPlayer;
                    break;
                case GameState.Run:
                    _stateObject = runPlayer;
                    break;
                default:
                    break;
            }

            if(_stateObject != currentController)
            {
                GameObject _newController = Instantiate(_stateObject, transform);
                currentController = _newController.GetComponentInChildren<PlayerController>();
            }
        }
    }
}