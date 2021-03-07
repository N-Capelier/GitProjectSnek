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

        [SerializeField] GameState startingState;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            Set(startingState);
        }


        public void Set(GameState newState, string levelName = "")
        {
            StartCoroutine(LoadAndWaitScene(newState, levelName));
        }

        IEnumerator LoadAndWaitScene(GameState newState, string levelName)
        {
            if (ActiveState == newState)
                yield return null;

            if (levelName != "")
            {
                SceneManager.LoadScene(levelName);
                /*_newScene.allowSceneActivation = false;
                while(_newScene.progress < 0.95f)
                {
                    yield return null;
                }
                _newScene.allowSceneActivation = true;*/
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