using System.Collections;
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
        float baseMoveSpeed;
        float baseAngularSpeed;
        float baseAcceleration;

        float distanceFromDestination;

        public NavMeshAgent agent;
        [HideInInspector] public Transform target;

        [HideInInspector] public byte actions = 0;

        IEnumerator Start()
        {
            baseMoveSpeed = agent.speed;
            baseAngularSpeed = agent.angularSpeed;
            baseAcceleration = agent.acceleration;

            actions++;
            yield return new WaitForSeconds(0.5f);
            yield return new WaitUntil(() => HubCamTargetController.Instance != null);
            target = HubCamTargetController.Instance.transform;  ////////////////////////////////////////////////////////////////////////
            InteractionManager.Instance.playerController = this;
            actions--;
        }

        private void FixedUpdate()
        {
            distanceFromDestination = Vector3.Distance(agent.destination, objectRenderer.transform.position);

            switch (distanceFromDestination)
            {
                case float value when (value <= 12):
                    agent.speed = baseMoveSpeed;
                    agent.angularSpeed = baseAngularSpeed;
                    agent.acceleration = baseAcceleration;
                    break;
                case float value when (value < 14f):
                    agent.speed = baseMoveSpeed;
                    agent.angularSpeed = baseAngularSpeed;
                    agent.acceleration = baseAcceleration * 60f;
                    break;
                case float value when (value >= 14f):
                    agent.speed = baseMoveSpeed * 6f;
                    agent.angularSpeed = baseAngularSpeed * 6f;
                    agent.acceleration = baseAcceleration * 60f;
                    break;
                default:
                    throw new System.Exception("WTF bro you've created a negative distance, you are a beast!");
            }

            if (actions == 0)
            {
                if(agent != null && target != null)
                {
                    agent.destination = target.position;
                    animator.SetFloat("Distance", distanceFromDestination);
                }
            }
            else
            {
                animator.SetFloat("Distance", 0);
            }
        }
    }
}