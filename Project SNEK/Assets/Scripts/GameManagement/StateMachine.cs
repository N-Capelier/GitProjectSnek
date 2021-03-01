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
            /*if(newState == GameState.Run)
            {
                if(levelName == null || levelName == "")
                {
                    throw new System.Exception("Run Game State must have an associated level name");
                }


            }
            else */if(ActiveState != newState)
            {
                animator.Play(newState.ToString());
                ActiveState = newState;
            }
        }

        IEnumerator LoadRun(string levelName)
        {
            AsyncOperation _newScene;
            _newScene = SceneManager.LoadSceneAsync("Nico");
            yield return new WaitUntil(() => _newScene.isDone);
        }
    }
}