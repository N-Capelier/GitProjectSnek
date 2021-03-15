using System.Collections;
using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// Arthur
    /// </summary>
    public class MouchouAnorexieBehaviour : MonoBehaviour
    {
        public MouchouBaseMovement mBm;
        EnemyStats stats;

        public bool isVomito;
        public bool isEater;

        public GameObject vomito;
        public GameObject jaw;

        // Start is called before the first frame update
        void Start()
        {
            stats = GetComponentInParent<EnemyStats>();

            stats.attackClock.ClockEnded += OnShouldAttack;
        }

        void OnShouldAttack()
        {
            mBm.canMove = false;

            if (isVomito == true && isEater == false)
            {
                StartCoroutine(SpitBehaviour());
            }
            else if (isVomito == false && isEater == true)
            {
                StartCoroutine(EatBehaviour());
            }
            else if (isVomito == true && isEater == true)
            {
                //random
            }
        }

        IEnumerator SpitBehaviour()
        {
            mBm.GetNextNode();
            mBm.UpdateMovement();
            Instantiate(vomito, mBm.currentNode, Quaternion.identity);
            yield return new WaitUntil(() => mBm.isMoving == true);
            mBm.canMove = true;
        }

        IEnumerator EatBehaviour()
        {
            mBm.GetNextNode();
            mBm.UpdateMovement();
            Instantiate(jaw, mBm.currentNode, Quaternion.identity);
            Instantiate(jaw, mBm.nextNode, Quaternion.identity);
            yield return new WaitUntil(() => mBm.isMoving == true);
            mBm.canMove = true;
        }

        private void OnDestroy()
        {
            stats.attackClock.ClockEnded -= OnShouldAttack;
        }
    }

}