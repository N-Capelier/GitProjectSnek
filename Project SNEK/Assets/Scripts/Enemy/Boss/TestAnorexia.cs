using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using Player.Spells;
using Map;

namespace Boss
{
    public class TestAnorexia : MonoBehaviour
    {
        public GameObject mouchou;

        public GameObject targetMarker;
        public GameObject targetMarkerLong;
        public GameObject patternPos;
        public GameObject targetFeedback;
        public GameObject cam;
        public GameObject shield;
        public GameObject shieldPos;
        [SerializeField] float camDistance;

        Clock bombClock;
        Rigidbody rb;

        //int patternOrder = 0;
        int patternCount = 0;
        [SerializeField] float timeToBomb;
        public float speed = 3;
        float moveSpeed = 2.5f;
        bool bombOver = false;
        [SerializeField] bool canBeHit = false;
        bool canDoPattern = true;

        [Space]
        public EnemyAttackPattern pattern;
        [Space]
        public EnemyAttackPattern labyrinth;
        List<GameObject> incomingBombs;

        [SerializeField] Vector3 targetVec;


        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            //UpdateMovement();
            
            if(shieldPos.transform.childCount <= 0 && canBeHit == false)
            {
                canBeHit = true;
                StopAllCoroutines();
                StartCoroutine(Stun());
            }

            if (canDoPattern && canBeHit == false)
            {
                switch (patternCount)
                {
                    case 0:
                        canDoPattern = false;
                        StartCoroutine(PatternBomb());
                        return;
                    case 1:
                        canDoPattern = false;
                        StartCoroutine(PatternLabyrinth());
                        return;
                    case 2:
                        canDoPattern = false;
                        StartCoroutine(SpawnMouchou());
                        return;
                }
            }            
        }

        
        IEnumerator PatternBomb()
        {

            //canBeHit = false;
            targetFeedback.SetActive(true);
            targetFeedback.transform.localPosition = new Vector3(0, 0.3f, -1.5f); 
            timeToBomb = Random.Range(4, 8);
            bombClock = new Clock(timeToBomb);
            bombClock.ClockEnded += EndTimeBomb;
            Vector3 pos1 = new Vector3(targetFeedback.transform.localPosition.x - 2, targetFeedback.transform.localPosition.y, gameObject.transform.localPosition.z -13);
            Vector3 pos2 = new Vector3(targetFeedback.transform.localPosition.x + 2, targetFeedback.transform.localPosition.y, gameObject.transform.localPosition.z -13);
            targetFeedback.transform.localPosition = new Vector3(Random.Range(pos1.x, pos2.x), targetFeedback.transform.localPosition.y, gameObject.transform.localPosition.z -13);

            while (bombOver == false)
            {                
                targetFeedback.transform.localPosition = Vector3.Lerp(pos1, pos2, (Mathf.Sin(Time.time * speed) +1) /2);                
                yield return new WaitForEndOfFrame();
            }
            targetVec = new Vector3(targetFeedback.transform.position.x, targetFeedback.transform.position.y, gameObject.transform.position.z);
            yield return new WaitForSeconds(1f);
            TargetCell();            
            targetFeedback.SetActive(false);            
            yield return new WaitForSeconds(10);
            bombOver = false;
            canDoPattern = true;
        }

        IEnumerator PatternLabyrinth()
        {
            TargetCellLabyrinth();
            yield return new WaitForSeconds(10);
            canDoPattern = true;
        }

        void TargetCell()
        {
            incomingBombs = new List<GameObject>();

            for (int x = 0; x < pattern.row.Length; x++)
            {
                for (int y = 0; y < pattern.row[x].column.Length; y++)
                {
                    if (pattern.row[x].column[y] == true)
                    {
                        Instantiate(targetMarker, (new Vector3(targetVec.x + y, targetVec.y, targetVec.z - x)), Quaternion.identity, gameObject.transform);
                        incomingBombs.Add(targetMarker);
                    }
                }
            }
            patternCount++;            
        }

        void TargetCellLabyrinth()
        {
            incomingBombs = new List<GameObject>();

            for (int x = 0; x < labyrinth.row.Length; x++)
            {
                for (int y = 0; y < labyrinth.row[x].column.Length; y++)
                {
                    if (labyrinth.row[x].column[y] == false)
                    {
                        Instantiate(targetMarkerLong, (new Vector3((patternPos.transform.position.x + y), (patternPos.transform.position.y), (patternPos.transform.position.z - x))), Quaternion.identity, gameObject.transform);
                        incomingBombs.Add(targetMarkerLong);
                    }
                }
            }
            patternCount++;
        }

        IEnumerator SpawnMouchou()
        {
            yield return new WaitForSeconds(1f);
            Instantiate(mouchou, new Vector3(transform.position.x + Random.Range(-3, 3), transform.position.y, transform.position.z -1), Quaternion.identity);
            yield return new WaitForSeconds(2f);
            Instantiate(mouchou, new Vector3(transform.position.x + Random.Range(-3, 3), transform.position.y, transform.position.z -1), Quaternion.identity);
            yield return new WaitForSeconds(2f);
            Instantiate(mouchou, new Vector3(transform.position.x + Random.Range(-3, 3), transform.position.y, transform.position.z - 1), Quaternion.identity);
            yield return new WaitForSeconds(2);
            patternCount = 0;
            canDoPattern = true;
        }

        IEnumerator Stun()
        {            
            yield return new WaitForSeconds(5);
            Instantiate(shield, shieldPos.transform.position, Quaternion.identity, shieldPos.transform);
            canBeHit = false;
        }

        void EndTimeBomb()
        {
            bombOver = true;
        }

        void UpdateMovement()
        {
            if (canBeHit == false)
            {
                if ((gameObject.transform.position.z - cam.transform.position.z) < camDistance)
                {
                    rb.velocity = new Vector3(0, 0, moveSpeed);
                }
                else
                {
                    rb.velocity = new Vector3(0, 0, 0);
                }
            }
            else
                rb.velocity = new Vector3(0, 0, 0);
        }

        void StopPatterns()
        {
            StopAllCoroutines();            
            targetFeedback.gameObject.SetActive(false);            

            for (int i = 0; i < incomingBombs.Count; i++)
            {
                Destroy(incomingBombs[i]);
            }
        }
    }
}
