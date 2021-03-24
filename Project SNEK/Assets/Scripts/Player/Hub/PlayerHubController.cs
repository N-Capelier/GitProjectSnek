using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Rendering.Hub;
using Hub.Interaction;

namespace Player.Controller
{
    /// <summary>
    /// Nico
    /// </summary>
    public class PlayerHubController : PlayerController
    {

        public NavMeshAgent agent;
        public Transform target;
        public GameObject renderer;

        [HideInInspector] public byte actions = 0;

        public override void Awake()
        {
            target = HubCamTargetController.Instance.transform;
            InteractionManager.Instance.playerController = this;
        }

        private void FixedUpdate()
        {
            if(actions == 0)
            {
                agent.destination = target.position;
                animator.SetFloat("Distance", Vector3.Distance(agent.destination, renderer.transform.position));
            }
        }

    }
}