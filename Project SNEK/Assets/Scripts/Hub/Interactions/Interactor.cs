using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hub.Interaction
{
    public abstract class Interactor : MonoBehaviour
    {
        [SerializeField] Transform cameraTargetTransform;
        [SerializeField] Transform playerTargetTransform;

        [SerializeField] [Range(0f, 360f)] float orientation = 0f;

        public IEnumerator BeginInteraction()
        {
            InteractionManager.Instance.camTarget.actions++;
            InteractionManager.Instance.camTarget.transform.position = cameraTargetTransform.position;

            InteractionManager.Instance.playerController.actions++;
            InteractionManager.Instance.playerController.agent.destination = playerTargetTransform.position;

            while (Vector3.Distance(playerTargetTransform.position, InteractionManager.Instance.playerController.agent.transform.position) > 1f)
            {
                yield return null;
            }

            InteractionManager.Instance.playerController.agent.transform.Rotate(new Vector3(0, orientation, 0));

            Interact();
        }

        protected abstract void Interact();

    }
}