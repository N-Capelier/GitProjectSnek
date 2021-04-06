using UnityEngine;
using Player;
using Player.Controller;
using AudioManagement;

namespace GameManagement.GameStates
{
    /// <summary>
    /// Nico
    /// </summary>
    public class HubGameState : StateMachineBehaviour
    {
        public static Source hubMusic;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            GameObject _newController = Instantiate(PlayerManager.Instance.hubPlayer, PlayerManager.Instance.transform);
            PlayerManager.Instance.currentController = _newController.GetComponent<PlayerController>();

            if (hubMusic == null)
            {
                hubMusic = AudioManager.Instance.PlayThisSoundEffect("HubMusic");
                hubMusic.audioSource.loop = true;
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
            Destroy(PlayerManager.Instance.currentController.gameObject);

            if (hubMusic != null)
            {
                hubMusic.audioSource.Stop();
                hubMusic = null;
            }
        }
    }
}