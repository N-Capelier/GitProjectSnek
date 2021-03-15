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

       public GameObject vomito;


        // Start is called before the first frame update
        void Start()
        {
            stats = GetComponentInParent<EnemyStats>();

            stats.attackClock.ClockEnded += OnShouldAttack;
        }

        void OnShouldAttack()
        {
            mBm.OnShouldMove();
            mBm.canMove = false;
            StartCoroutine(SpitBehaviour());
        }

        IEnumerator SpitBehaviour()
        {
            Instantiate(vomito, mBm.currentNode, Quaternion.identity);
            yield return new WaitUntil(() => mBm.isMoving == true);
            mBm.canMove = true;
        }

        private void OnDestroy()
        {
            stats.attackClock.ClockEnded -= OnShouldAttack;
        }
    }

}