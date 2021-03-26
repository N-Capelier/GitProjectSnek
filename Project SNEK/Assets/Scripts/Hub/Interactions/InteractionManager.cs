using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rendering.Hub;
using Player.Controller;
using Player;

namespace Hub.Interaction
{
    public class InteractionManager : Singleton<InteractionManager>
    {
        public HubCamTargetController camTarget;
        public PlayerHubController playerController;

        private void Awake()
        {
            CreateSingleton();
        }

        public virtual void EndInteraction()
        {
            camTarget.transform.position = playerController.agent.transform.position;
            camTarget.actions--;
            playerController.actions--;
        }
    }
}