using System.Collections;
using UnityEngine;
using AudioManagement;

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

        GameObject lastPoison;
        IEnumerator SpitBehaviour()
        {
            lastPoison = Instantiate(vomito, mBm.currentNode, Quaternion.identity);
            AudioManager.Instance.PlayThisSoundEffect("MouchouVomito04", transform);
            yield return new WaitUntil(() => mBm.isMoving == true);
            mBm.canMove = true;
        }

        private void OnDestroy()
        {
            Destroy(lastPoison);
            AudioManager.Instance.PlayThisSoundEffect("MouchouDeath", transform);
            stats.attackClock.ClockEnded -= OnShouldAttack;
        }
    }

}