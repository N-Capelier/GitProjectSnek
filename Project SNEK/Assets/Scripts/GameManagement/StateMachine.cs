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
        public Dictionary<GameState, string> gameStates = new Dictionary<GameState, string>();
        public GameState ActiveState { get; private set; } = GameState.Cinematic;

        private void Awake()
        {
            animator = GetComponent<Animator>();

            Set(GameState.Cinematic);

            gameStates.Add(GameState.Cinematic, "Cinematic");
            gameStates.Add(GameState.Dialog, "Dialog");
            gameStates.Add(GameState.Hub, "Hub");
            gameStates.Add(GameState.MainMenu, "MainMenu");
            gameStates.Add(GameState.PauseMenu, "PauseMenu");
            gameStates.Add(GameState.Run, "Run");
        }

        public void Set(GameState newState)
        {
            if (ActiveState != newState)
            {
                animator.Play(gameStates[newState]);
                ActiveState = newState;
            }
        }
    }
}