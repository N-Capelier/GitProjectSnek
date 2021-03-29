using UnityEngine;
using Player;

namespace Rendering.Run
{
    /// <summary>
    /// Nico
    /// </summary>
    public class FreeScrollingCamState : StateMachineBehaviour
    {
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            RunCamController.Instance.transform.position = new Vector3(4, 9, -5);
            RunCamController.Instance.rb.velocity = new Vector3(0, 0, RunCamController.Instance.scrollSpeed * 2.5f);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    if(Input.GetKeyDown(KeyCode.Tab)) { RunCamController.Instance.Set(CamState.PlayerScrolling); }
        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            RunCamController.Instance.rb.velocity = Vector3.zero;
        }
    }
}