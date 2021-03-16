using System.Collections;
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

        public bool random = false;

        [SerializeField] GameObject patternPos;

        [Space]
        public EnemyAttackPattern pattern;

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
            if(random == true)
            {
                for (int i = 0; i < targetNumber; i++)
                {
                    targetPos = new Vector3(transform.position.x - Random.Range(-3, 3), 0, transform.position.z - Random.Range(1, 5));
                    Instantiate(targetMarker, targetPos, Quaternion.identity);
                }
            }
            else
            {
                for (int x = 0; x < pattern.row.Length; x++)
                {
                    for (int y = 0; y < pattern.row[x].column.Length; y++)
                    {
                        //print(attackPattern.attackPattern[i, x]);
                        //print(pattern.row[x].column[y]);

                        if (pattern.row[x].column[y] == true)
                        {
                            Instantiate(targetMarker, (new Vector3((patternPos.transform.position.x + y), (patternPos.transform.position.y), (patternPos.transform.position.z - x))), Quaternion.identity);
                        }
                    }
                }
            }            
        }

        private void OnDestroy()
        {
            stats.attackClock.ClockEnded -= OnShouldAttack;
        }

    }

}