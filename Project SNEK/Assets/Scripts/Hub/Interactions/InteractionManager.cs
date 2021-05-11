using Rendering.Hub;
using Player.Controller;

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
            if (!isInteracting)
                return;
            camTarget.transform.position = playerController.agent.transform.position;
            camTarget.actions--;
            playerController.actions--;
            isInteracting = false;
        }
    }
}