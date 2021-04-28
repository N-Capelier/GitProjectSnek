using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class IllusionisteBehaviour : MonoBehaviour
    {
        EnemyStats stats;
        IllusionisteMovement movement;
        public GameObject clone;
        public int cloneNumber;
        public GameObject[] clonesList;

        private void Start()
        {
            clonesList = new GameObject[cloneNumber];
            stats = GetComponent<EnemyStats>();
            movement = GetComponent<IllusionisteMovement>();
            stats.attackClock.ClockEnded += OnshouldAttack;
            movement.InstantiateClones();
        }


        void OnshouldAttack()
        {
            Debug.Log("Illu_ATK");
        }

        void InstantiateClones()
        {
            
        }

    }
}
