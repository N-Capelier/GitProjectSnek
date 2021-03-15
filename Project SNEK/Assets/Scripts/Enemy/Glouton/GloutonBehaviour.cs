﻿using System.Collections;
using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// Arthur
    /// </summary>
    public class GloutonBehaviour : MonoBehaviour
    {
        EnemyStats stats;
        public Animator anim;

        public int targetNumber;
        public GameObject targetMarker;

        Vector3 targetPos;

        public EnemyAttackPattern attackPattern;

        // Start is called before the first frame update
        void Start()
        {
            stats = GetComponentInParent<EnemyStats>();
            stats.attackClock.ClockEnded += OnShouldAttack;
        }

        void OnShouldAttack()
        {
            anim.SetBool("isAttacking", true);
            StartCoroutine(LaunchProjectile());
        }

        IEnumerator LaunchProjectile()
        {
            yield return new WaitForSeconds(1);
            TargetCell();
            yield return new WaitForSeconds(2);
            anim.SetBool("isAttacking", false);

        }



        void TargetCell()
        {
            for (int i = 0; i < attackPattern.attackPattern.GetLength(0); i++)
            {
                for (int x = 0; x < attackPattern.attackPattern.GetLength(1); x++)
                {
                    if(attackPattern.attackPattern[i,x] == true)
                    {
                        Instantiate(targetMarker, targetPos, Quaternion.identity);
                    }
                }
            }

            /*for (int i = 0; i < targetNumber; i++)
            {
                targetPos = new Vector3(transform.position.x - Random.Range(-3, 3), 0, transform.position.z - Random.Range(1, 5));
                Instantiate(targetMarker, targetPos, Quaternion.identity);
            }*/
        }

        private void OnDestroy()
        {
            stats.attackClock.ClockEnded -= OnShouldAttack;
        }

    }

}