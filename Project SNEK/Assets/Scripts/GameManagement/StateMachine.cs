using System.Collections.Generic;
using UnityEngine;
using GameManagement.GameStates;
using UnityEngine.SceneManagement;
using System.Collections;

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

        public void Set(GameState newState, string levelName = "")
        {
            if (ActiveState == newState)
                return;

            if(levelName != "")
            {
                SceneManager.LoadScene(levelName);
                Set(newState);
            }
            else
            {
                animator.Play(newState.ToString());
                ActiveState = newState;
            }
        }
    }
}