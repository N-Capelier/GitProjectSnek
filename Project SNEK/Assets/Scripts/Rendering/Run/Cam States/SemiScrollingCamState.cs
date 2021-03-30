using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Rendering.Run
{
    /// <summary>
    /// Nico
    /// </summary>
    public class SemiScrollingCamState : StateMachineBehaviour
    {
        GameObject deathZone;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            RunCamController.Instance.transform.position = new Vector3(0, 0, 0);
            deathZone = Instantiate(RunCamController.Instance.deathZone, new Vector3(5f, 0.25f, -2f), Quaternion.identity, RunCamController.Instance.vcam.transform);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(PlayerManager.Instance.currentController.transform.position.z > RunCamController.Instance.vcam.transform.position.z + 10f)
            {
                RunCamController.Instance.rb.velocity = new Vector3(0, 0, 2.5f);
            }
            else
            {
                RunCamController.Instance.rb.velocity = new Vector3(0, 0, RunCamController.Instance.scrollSpeed * 2.5f);
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (deathZone != null)
            {
                Destroy(deathZone);
            }
            else
            {
                throw new System.Exception("An error occured when tried to leave SemiScrollingCam state: deathZone not instantiated");
            }
        }
    }

}