using UnityEngine;
using Player;

namespace Rendering.Run
{
    /// <summary>
    /// Nico
    /// </summary>
    public class FreeScrollingCamState : StateMachineBehaviour
    {
        GameObject deathZone;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("free scrolling");
            RunCamController.Instance.transform.position = new Vector3(0, 0, 0);
            RunCamController.Instance.rb.velocity = new Vector3(0, 0, RunCamController.Instance.scrollSpeed * 2.5f);

            deathZone = Instantiate(RunCamController.Instance.deathZone, new Vector3(5f, 0.25f, -4f), Quaternion.identity, RunCamController.Instance.vcam.transform);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{

        //}

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            RunCamController.Instance.rb.velocity = Vector3.zero;

            if(deathZone != null)
            {
                Destroy(deathZone);
            }
            //else
            //{
            //    throw new System.Exception("An error occured when tried to leave FreeScrollingCam state: deathZone not instantiated");
            //}
        }
    }
}