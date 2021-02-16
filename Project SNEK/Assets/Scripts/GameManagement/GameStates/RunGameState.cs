using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Player.Controller;
using Map;

namespace GameManagement.GameStates
{
    /// <summary>
    /// Nico
    /// </summary>
    public class RunGameState : StateMachineBehaviour
    {
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            GameObject _newController = Instantiate(PlayerManager.Instance.runPlayer, PlayerManager.Instance.transform);
            PlayerManager.Instance.currentController = _newController.GetComponent<PlayerController>();
            PlayerManager.Instance.currentController.transform.position = MapGrid.Instance.GetWorldPos(3, 1);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}
    }
}