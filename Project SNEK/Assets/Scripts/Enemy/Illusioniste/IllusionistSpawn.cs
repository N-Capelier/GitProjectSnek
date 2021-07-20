using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class IllusionistSpawn : MonoBehaviour
    {
        public IllusionisteMovement movement;
        public IllusionisteBehaviour behaviour;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("PlayerController") && behaviour.isSpawned == false)
            {
                print("Hello");
                behaviour.isSpawned = true;
                movement.InstantiateClones();
                StartCoroutine(movement.SpawnRoutine());                
            }
        }
    }
}
