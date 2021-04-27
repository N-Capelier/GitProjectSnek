using UnityEngine;
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

        [SerializeField] TransitionScreenManager sceneTransition;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            Set(startingState);
        }


        public void Set(GameState newState, string levelName = "", bool makeTransition = true)
        {
            StartCoroutine(LoadAndWaitScene(newState, levelName, makeTransition));
        }

        //public void SetAlphaUp()
        //{
        //    StartCoroutine(sceneTransition.AlphaUp(.2f));
        //}

        public void SetAlphaDown()
        {
            StartCoroutine(sceneTransition.AlphaDown(.05f));
        }

        IEnumerator LoadAndWaitScene(GameState newState, string levelName, bool makeTransition)
        {
            if (ActiveState == newState)
                yield return null;

            if (levelName != "")
            {
                if (makeTransition && sceneTransition != null)
                {
                    StartCoroutine(sceneTransition.AlphaUp(.05f, levelName));
                }
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