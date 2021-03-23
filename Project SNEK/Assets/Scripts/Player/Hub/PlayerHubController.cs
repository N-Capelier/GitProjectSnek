using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Rendering.Hub;

namespace Player.Controller
{
    /// <summary>
    /// Nico
    /// </summary>
    public class PlayerHubController : PlayerController
    {

        [SerializeField] NavMeshAgent agent;
        [SerializeField] Transform target;

        public override void Awake()
        {
            target = HubCamTargetController.Instance.transform;
        }

        private void FixedUpdate()
        {
            agent.destination = target.position;
        }

    }
}