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
        public int lifeTime;
        /*[HideInInspector]*/ public int patternCount = 0;

        void Start()
        {
            stats = GetComponent<EnemyStats>();
            behaviour = GetComponent<IllusionisteBehaviour>();
            //stats.movementClock.ClockEnded += OnShouldMove;
            StartCoroutine(SpawnRoutine());
        }



        int remainClones;
        IEnumerator SpawnRoutine()
        {
            yield return new WaitForSeconds(lifeTime - 1.11f);

            for (int i = 0; i < behaviour.clonesList.Count; i++)
            {
                behaviour.clonesList[i].GetComponentInChildren<Animator>().SetBool("despawned", true);
            }

            yield return new WaitForSeconds(1.11f);

            if (behaviour.isKillable == false)
            {
                remainClones = cloneParent.gameObject.transform.childCount;
                for (int i = 0; i < behaviour.clonesList.Count; i++)
                {
                    if (behaviour.clonesList[i] != null)
                    {
                        Destroy(behaviour.clonesList[i]);
                    }
                }
                behaviour.clonesList = new List<GameObject>();

                yield return new WaitForSeconds(0);
                InstantiateClones();
                if (behaviour.clonesList.Count <= 1 && behaviour.isKillable == false)
                {
                    behaviour.isKillable = true;                    
                }
            }
            StartCoroutine(SpawnRoutine());
        }

        [SerializeField] int index = 0;
        GameObject clone;

        public void InstantiateClones()
        {
            StartCoroutine(behaviour.OnshouldAttack());

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
                                behaviour.clonesList.Add(clone);
                                index++;
                                if (index == behaviour.cloneNumber)
                                {
                                    
                                    break;
                                }
                            }
                        }
                        if (index == behaviour.cloneNumber)
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
                                behaviour.clonesList.Add(clone);
                                index++;
                                if (index == behaviour.cloneNumber)
                                {
                                    break;
                                }
                            }
                        }
                        if (index == behaviour.cloneNumber)
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

