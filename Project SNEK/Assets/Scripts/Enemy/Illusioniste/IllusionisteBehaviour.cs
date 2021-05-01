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
        /*[HideInInspector]*/ public bool isKillable = false;

        private void Start()
        {
            clonesList = new GameObject[cloneNumber];
            stats = GetComponent<EnemyStats>();
            movement = GetComponent<IllusionisteMovement>();
            stats.attackClock.ClockEnded += OnshouldAttack;
            movement.InstantiateClones();
        }

        private void Update()
        {
            if(isKillable == true)
            {
                StartCoroutine(IsRegenerating());
            }
        }

        int index = 0;

        void OnshouldAttack()
        {
            if(isKillable == false)
            {
                index = Random.Range(0, clonesList.Length);
                StartCoroutine(clonesList[index].GetComponent<IllusionisteClone>().Fire());
            }
        }

        public IEnumerator IsRegenerating()
        {
            clonesList[0].GetComponentInChildren<MeshRenderer>().material = clonesList[0].GetComponent<IllusionisteClone>().killableMat;
            yield return new WaitForSeconds(5);
            clonesList[0].GetComponentInChildren<MeshRenderer>().material = clonesList[0].GetComponent<IllusionisteClone>().defaultMat;
            isKillable = false;
        }

        public void Death()
        {
            stats.Death();
        }

    }
}
