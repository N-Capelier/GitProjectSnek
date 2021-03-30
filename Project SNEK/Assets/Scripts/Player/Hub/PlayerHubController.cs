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
        float baseMoveSpeed;
        float mach20MoveSpeed;

        float distanceFromDestination;

        public NavMeshAgent agent;
        [HideInInspector] public Transform target;

        [HideInInspector] public byte actions = 0;

        void Start()
        {
            baseMoveSpeed = agent.speed;
            mach20MoveSpeed = baseMoveSpeed * 50f;
            StartCoroutine(GetElements());
        }

        private void FixedUpdate()
        {
            distanceFromDestination = Vector3.Distance(agent.destination, objectRenderer.transform.position);
            Debug.Log("distance: " + distanceFromDestination);

            //if (distanceFromDestination > 12f)
            //{
            //    agent.speed = mach20MoveSpeed;
            //}
            //else
            //{
            //    agent.speed = baseMoveSpeed;
            //}

            if(actions == 0)
            {
                agent.destination = target.position;
                animator.SetFloat("Distance", distanceFromDestination);
            }
        }
        //private void OnBecameVisible()
        //{
        //    agent.speed = baseMoveSpeed;
        //}

        //private void OnBecameInvisible()
        //{
        //    agent.speed = mach20MoveSpeed;
        //}

        IEnumerator GetElements()
        {
            actions++;
            yield return new WaitForSeconds(0.1f);
            target = HubCamTargetController.Instance.transform;
            InteractionManager.Instance.playerController = this;
            actions--;
        }
    }
}