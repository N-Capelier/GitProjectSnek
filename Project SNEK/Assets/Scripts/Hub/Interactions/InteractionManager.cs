using Rendering.Hub;
using Player.Controller;
using System.Collections;
using UnityEngine;

namespace Hub.Interaction
{
    public class InteractionManager : Singleton<InteractionManager>
    {
        public HubCamTargetController camTarget;
        public PlayerHubController playerController;
        public bool isInteracting;

        private void Awake()
        {
            CreateSingleton();
        }

        public virtual void EndInteraction()
        {
            StartCoroutine(EndInteractionCoroutine());
        }

        IEnumerator EndInteractionCoroutine()
        {
            yield return new WaitForSeconds(0.1f);
            if (!isInteracting)
                yield break;
            camTarget.transform.position = playerController.agent.transform.position;
            camTarget.actions--;
            playerController.actions--;
            yield return new WaitForSeconds(1f);
            isInteracting = false;
        }
    }
}