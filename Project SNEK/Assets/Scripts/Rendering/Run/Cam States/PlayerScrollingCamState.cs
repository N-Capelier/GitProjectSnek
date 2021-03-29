﻿using Player;
using UnityEngine;

namespace Rendering.Run
{
    /// <summary>
    /// Nico
    /// </summary>
    public class PlayerScrollingCamState : StateMachineBehaviour
    {
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            RunCamController.Instance.vcam.Follow = PlayerManager.Instance.currentController.gameObject.transform;
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    if (Input.GetKeyDown(KeyCode.Tab)) { RunCamController.Instance.Set(CamState.FreeScrolling); }
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            RunCamController.Instance.vcam.Follow = null;
        }
    }
}