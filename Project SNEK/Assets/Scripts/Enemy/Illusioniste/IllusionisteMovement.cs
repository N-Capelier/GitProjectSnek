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
        public GameObject spawnParticle;
        public GameObject despawnParticle;
        public int lifeTime;
        /*[HideInInspector]*/ public int patternCount = 0;

        void Start()
        {
            stats = GetComponent<EnemyStats>();
            behaviour = GetComponent<IllusionisteBehaviour>();
            //stats.movementClock.ClockEnded += OnShouldMove;
            //StartCoroutine(SpawnRoutine());
        }



        int remainClones;
        public IEnumerator SpawnRoutine()
        {         
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

                yield return new WaitForSeconds(0.2f);

                InstantiateClones();

                yield return new WaitForSeconds(0.2f);

                for (int i = 0; i < behaviour.clonesList.Count; i++)
                {
                    if (behaviour.clonesList[i] != null)
                    {
                        Instantiate(spawnParticle, behaviour.clonesList[i].transform.position, Quaternion.identity);
                        yield return new WaitForSeconds(0.15f);
                    }
                }

                yield return new WaitForSeconds(0.5f);

                for (int i = 0; i < behaviour.clonesList.Count; i++)
                {
                    if(behaviour.clonesList[i] != null)
                    {
                        behaviour.clonesList[i].SetActive(true);
                        yield return new WaitForSeconds(0.15f);
                    }
                }

                yield return new WaitForSeconds(0.5f);

                StartCoroutine(behaviour.OnshouldAttack());

                /*if (behaviour.clonesList.Count <= 1 && behaviour.isKillable == false)
                {
                    behaviour.isKillable = true;                    
                }*/
            }

            yield return new WaitForSeconds(lifeTime - 1.3f);

            for (int i = 0; i < behaviour.clonesList.Count; i++)
            {
                behaviour.clonesList[i].GetComponentInChildren<Animator>().SetBool("despawned", true);                
            }

            yield return new WaitForSeconds(1f);

            for (int i = 0; i < behaviour.clonesList.Count; i++)
            {
                Instantiate(despawnParticle, behaviour.clonesList[i].transform.position, Quaternion.identity);
            }           

            yield return new WaitForSeconds(0.4f);

            StartCoroutine(SpawnRoutine());
        }

        [SerializeField] int index = 0;
        GameObject clone;

        public void InstantiateClones()
        {
            behaviour.clonesList = new List<GameObject>();

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
                                clone.SetActive(false);
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
                                clone.SetActive(false);
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

