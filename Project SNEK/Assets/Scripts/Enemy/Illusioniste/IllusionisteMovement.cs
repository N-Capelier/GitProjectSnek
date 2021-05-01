using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class IllusionisteMovement : MonoBehaviour
    {
        EnemyStats stats;
        IllusionisteBehaviour behaviour;
        public EnemyAttackPattern[] patterns;
        public GameObject patternPos;
        public GameObject cloneParent;
        /*[HideInInspector]*/ public int patternCount = 0;

        void Start()
        {
            stats = GetComponent<EnemyStats>();
            behaviour = GetComponent<IllusionisteBehaviour>();
            stats.movementClock.ClockEnded += OnShouldMove;
            StartCoroutine(DelayStart());
        }

        IEnumerator DelayStart()
        {
            yield return new WaitForSeconds(1.5f);
            stats.movementClock.SetTime(stats.movementCooldown);
        }

        void OnShouldMove()
        {
            /*if (patternCount > 2)
            {
                patternCount++;
            }
            else
                patternCount = 0;*/

            StartCoroutine(SpawnRoutine());
        }


        int remainClones;
        IEnumerator SpawnRoutine()
        {
            if(behaviour.isKillable == false)
            {
                remainClones = cloneParent.gameObject.transform.childCount;
                for (int i = 0; i < behaviour.clonesList.Length; i++)
                {
                    if (behaviour.clonesList[i] != null)
                    {
                        Destroy(behaviour.clonesList[i]);
                    }
                }
                behaviour.clonesList = new GameObject[remainClones];

                yield return new WaitForSeconds(0);
                InstantiateClones();
                if (behaviour.clonesList.Length <= 1 && behaviour.isKillable == false)
                {
                    behaviour.isKillable = true;                    
                    StartCoroutine(behaviour.IsRegenerating());
                }
            }            
        }

        [SerializeField] int index = 0;
        GameObject clone;

        public void InstantiateClones()
        {
            index = 0;
            switch (patternCount)
            {
                case 0:
                    for (int x = 0; x < patterns[0].row.Length; x++)
                    {
                        for (int y = 0; y < patterns[0].row[x].column.Length; y++)
                        {
                            if (patterns[0].row[x].column[y] == true)
                            {
                                clone = Instantiate(behaviour.clone, (new Vector3((patternPos.transform.position.x + y), (patternPos.transform.position.y), (patternPos.transform.position.z - x))),Quaternion.identity, cloneParent.transform);
                                behaviour.clonesList[index] = clone;
                                index++;
                                if (index == behaviour.clonesList.Length)
                                {
                                    
                                    break;
                                }
                            }
                        }
                        if (index == behaviour.clonesList.Length)
                        {

                            break;
                        }
                    }
                    patternCount++;
                    break;
                case 1:
                    for (int x = 0; x < patterns[1].row.Length; x++)
                    {
                        for (int y = 0; y < patterns[1].row[x].column.Length; y++)
                        {
                            if (patterns[1].row[x].column[y] == true)
                            {
                                clone = Instantiate(behaviour.clone, (new Vector3((patternPos.transform.position.x + y), (patternPos.transform.position.y), (patternPos.transform.position.z - x))), Quaternion.identity, cloneParent.transform);
                                behaviour.clonesList[index] = clone;
                                index++;
                                if (index == behaviour.clonesList.Length)
                                {
                                    break;
                                }
                            }
                        }
                        if (index == behaviour.clonesList.Length)
                        {

                            break;
                        }
                    }
                    patternCount = 0;
                    break;

            }
        }
    }
}

