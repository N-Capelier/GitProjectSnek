using UnityEngine;
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

        [SerializeField] public TransitionScreenManager sceneTransition;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            Set(startingState);
        }

        public void Set(GameState newState, string levelName = "", bool makeTransition = true)
        {
            StartCoroutine(LoadAndWaitForScene(newState, levelName, makeTransition));
        }

        IEnumerator LoadAndWaitForScene(GameState newState, string levelName, bool makeTransition)
        {
            if (ActiveState == newState)
                yield return null;

            if (levelName != "")
            {
                if (makeTransition && sceneTransition != null)
                {
                    StartCoroutine(sceneTransition.AlphaUp(.05f, levelName));
                }
                Set(newState);
            }
            else
            {
                animator.Play(Animator.StringToHash(newState.ToString()));
                ActiveState = newState;
            }
        }

        public void SetAlphaDown()
        {
            StartCoroutine(sceneTransition.AlphaDown(.025f));
        }
    }
}