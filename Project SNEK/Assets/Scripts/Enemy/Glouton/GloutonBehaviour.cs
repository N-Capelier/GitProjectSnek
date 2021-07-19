using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioManagement;

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
        public GameObject bomb;
        GameObject projectile;

        Vector3 targetPos;

        public bool random = false;
        public bool doublePattern;

        [SerializeField] GameObject patternPos;

        [Space]
        public EnemyAttackPattern pattern;
        public EnemyAttackPattern pattern2;
        int patternRotation = 0;

        List<GameObject> incomingBombs;

        // Start is called before the first frame update
        void Start()
        {
            stats = GetComponentInParent<EnemyStats>();
            stats.attackClock.ClockEnded += OnShouldAttack;
        }

        private void FixedUpdate()
        {
            if (projectile != null)
            {
                projectile.transform.position = new Vector3(projectile.transform.position.x, projectile.transform.position.y + 0.5f, projectile.transform.position.z);
            }
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
            yield return new WaitForSeconds(1.8f);
            AudioManager.Instance.PlayThisSoundEffect("GloutonSpit", transform);
            projectile = Instantiate(bomb, new Vector3(transform.position.x, 1.5f, transform.position.z), Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
            anim.SetBool("isAttacking", false);
        }


        GameObject marker;

        void TargetCell()
        {
            incomingBombs = new List<GameObject>();

            if (random == true)
            {
                for (int i = 0; i < targetNumber; i++)
                {
                    targetPos = new Vector3(transform.position.x - Random.Range(-3, 3), 0, transform.position.z - Random.Range(1, 5));
                    marker = Instantiate(targetMarker, targetPos, Quaternion.identity);
                    incomingBombs.Add(marker);
                }
            }
            else if (doublePattern == false)
            {
                for (int x = 0; x < pattern.row.Length; x++)
                {
                    for (int y = 0; y < pattern.row[x].column.Length; y++)
                    {
                        if (pattern.row[x].column[y] == true)
                        {
                            marker = Instantiate(targetMarker, (new Vector3((patternPos.transform.position.x + y), (patternPos.transform.position.y), (patternPos.transform.position.z - x))), Quaternion.identity);
                            incomingBombs.Add(marker);
                        }
                    }
                }
            }
            else if (doublePattern == true)
            {
                if (patternRotation == 0)
                {
                    for (int x = 0; x < pattern.row.Length; x++)
                    {
                        for (int y = 0; y < pattern.row[x].column.Length; y++)
                        {
                            if (pattern.row[x].column[y] == true)
                            {
                                marker = Instantiate(targetMarker, (new Vector3((patternPos.transform.position.x + y), (patternPos.transform.position.y), (patternPos.transform.position.z - x))), Quaternion.identity);
                                incomingBombs.Add(marker);
                            }
                        }
                    }
                    patternRotation++;
                }
                else if (patternRotation == 1)
                {
                    for (int x = 0; x < pattern2.row.Length; x++)
                    {
                        for (int y = 0; y < pattern2.row[x].column.Length; y++)
                        {
                            if (pattern2.row[x].column[y] == true)
                            {
                                marker = Instantiate(targetMarker, (new Vector3((patternPos.transform.position.x + y), (patternPos.transform.position.y), (patternPos.transform.position.z - x))), Quaternion.identity);
                                incomingBombs.Add(marker);
                            }
                        }
                    }
                    patternRotation--;
                }
            }
        }

        private void OnDestroy()
        {
            stats.attackClock.ClockEnded -= OnShouldAttack;
            if(incomingBombs != null)
            {
                for (int i = 0; i < incomingBombs.Count; i++)
                {
                    if(incomingBombs[i] != null)
                    {
                        Destroy(incomingBombs[i]);
                    }
                }
            }
        }
    }
}