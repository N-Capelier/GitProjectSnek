using System.Collections;
using UnityEngine;
using Player;

namespace Hub.Interaction
{
    public abstract class Interactor : MonoBehaviour
    {
        [SerializeField] Transform cameraTargetTransform;
        [SerializeField] Transform playerTargetTransform;
        [SerializeField] protected Animator animator;

        [SerializeField] [Range(0f, 360f)] float orientation = 0f;
        //[SerializeField] float rotationSpeed = 0.1f;

        public virtual IEnumerator BeginInteraction()
        {
            if (InteractionManager.Instance.camTarget.actions != 0 || InteractionManager.Instance.isInteracting)
                yield break;

            InteractionManager.Instance.isInteracting = true;

            InteractionManager.Instance.camTarget.actions++;
            InteractionManager.Instance.camTarget.transform.position = cameraTargetTransform.position;

            InteractionManager.Instance.playerController.actions++;
            InteractionManager.Instance.playerController.agent.destination = playerTargetTransform.position;

            while (Vector3.Distance(playerTargetTransform.position, InteractionManager.Instance.playerController.agent.transform.position) > 1f)
            {
                yield return null;
            }

            float _rotationSpeed = 420f;
            Quaternion _targetQuat = Quaternion.Euler(0, orientation, 0);
            Debug.Log($"player: {PlayerManager.Instance.currentController.objectRenderer.gameObject.transform.rotation.eulerAngles} || target: {_targetQuat.eulerAngles}");
            //while (Mathf.Abs(_targetQuat.eulerAngles.y - PlayerManager.Instance.currentController.objectRenderer.gameObject.transform.rotation.eulerAngles.y) > 2)
            //{
            //    if(Vector3.SignedAngle(PlayerManager.Instance.currentController.objectRenderer.gameObject.transform.rotation.eulerAngles, _targetQuat.eulerAngles, Vector3.up) > 0)
            //    {
            //        print("1");
            //        PlayerManager.Instance.currentController.objectRenderer.gameObject.transform.RotateAround(PlayerManager.Instance.currentController.objectRenderer.gameObject.transform.position, Vector3.up, _rotationSpeed * Time.deltaTime);
            //    }
            //    else
            //    {
            //        print("2");
            //        PlayerManager.Instance.currentController.objectRenderer.gameObject.transform.RotateAround(PlayerManager.Instance.currentController.objectRenderer.gameObject.transform.position, Vector3.up, -_rotationSpeed * Time.deltaTime);
            //    }
                
            //    yield return new WaitForEndOfFrame();
            //}

            //while (Mathf.Abs(InteractionManager.Instance.playerController.agent.transform.rotation.y - orientation) > 0)
            //{
            //    InteractionManager.Instance.playerController.agent.transform.rotation = Quaternion.Lerp(InteractionManager.Instance.playerController.agent.transform.rotation, Quaternion.Euler(0, orientation, 0), rotationSpeed * Time.time);
            //    yield return new WaitForFixedUpdate();
            //}
            //InteractionManager.Instance.playerController.agent.transform.rotation = Quaternion.Euler(0, orientation, 0);
            //yield return null;

            //float timeElapsed = Time.time;
            //while (Mathf.Abs(InteractionManager.Instance.playerController.agent.transform.rotation.y - orientation) > 0.01f)
            //{
            //    InteractionManager.Instance.playerController.agent.transform.eulerAngles =
            //        Vector3.Lerp(
            //            InteractionManager.Instance.playerController.agent.transform.rotation.eulerAngles,
            //            new Vector3(0, orientation, 0),
            //            timeElapsed);
            //    timeElapsed += Time.deltaTime;
            //    yield return new WaitForEndOfFrame();
            //}

            Interact();
        }

        protected abstract void Interact();
    }
}