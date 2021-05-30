using UnityEngine;
using AudioManagement;

namespace GameManagement.GameStates
{
    /// <summary>
    /// Nico
    /// </summary>
    public class MainMenuGameState : StateMachineBehaviour
    {
        public static Source mainMenuMusic;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(mainMenuMusic == null)
            {
                mainMenuMusic = AudioManager.Instance.PlayThisSoundEffect("MainMenuMusic", true);
            }

        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (mainMenuMusic != null)
            {
                mainMenuMusic.audioSource.Stop();
                mainMenuMusic = null;
            }
        }
    }
}