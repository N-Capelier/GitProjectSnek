using System.Collections.Generic;
using UnityEngine;
using GameManagement.GameStates;

namespace GameManagement
{
    public enum GameState
    {
        MainMenu,
        PauseMenu,
        Cinematic,
        Dialog,
        Hub,
        Run
    }

    /// <summary>
    /// Nico
    /// </summary>
    public class StateMachine : MonoBehaviour
    {
        Animator animator;
        public GameState ActiveState { get; private set; } = GameState.MainMenu;

        [SerializeField] GameState startingState = GameState.MainMenu;

        private void Awake()
        {
            animator = GetComponent<Animator>();

            Set(startingState);
        }

        public void Set(GameState newState)
        {
            if (ActiveState != newState)
            {
                animator.Play(newState.ToString());
                ActiveState = newState;
            }
        }
    }
}