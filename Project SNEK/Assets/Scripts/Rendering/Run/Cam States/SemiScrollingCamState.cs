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

        float zVelocity = 0f;
        float xVelocity = 0f;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            RunCamController.Instance.transform.position = new Vector3(0f, 0, 0);
            deathZone = Instantiate(RunCamController.Instance.deathZone, new Vector3(5f, 0.25f, -2f), Quaternion.identity, RunCamController.Instance.vcam.transform);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(PlayerManager.Instance.currentController.transform.position.z - 5f > RunCamController.Instance.vcam.transform.position.z)
            {
                zVelocity = PlayerManager.Instance.currentController.rb.velocity.z;
            }
            else
            {
                zVelocity = 0f;
            }

            //xVelocity = Mathf.Lerp(
            //    RunCamController.Instance.transform.position.x,
            //    PlayerManager.Instance.currentController.transform.position.x,
            //    .5f);

            xVelocity = (PlayerManager.Instance.currentController.rb.transform.position.x - RunCamController.Instance.rb.transform.position.x) * 3.5f;

            //RunCamController.Instance.rb.velocity = new Vector3(
            //    xVelocity,
            //    RunCamController.Instance.rb.velocity.y,
            //    zVelocity);

            RunCamController.Instance.rb.velocity = new Vector3(
                xVelocity,
                RunCamController.Instance.rb.velocity.y,
                zVelocity);
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