﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using Player;
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
        Animator animator;

        [SerializeField] float maxHp;
        float currentHp;

        //int patternOrder = 0;
        int patternCount = 0;
        float timeToBomb;
        public float feedbackPosSpeed;
        float moveSpeed = 2.5f;
        bool bombOver = false;
        [SerializeField] bool canBeHit = false;
        bool canDoPattern = true;

        [Space]
        [SerializeField] SkinnedMeshRenderer bodyRenderer;
        [SerializeField] SkinnedMeshRenderer handsRenderer;
        Material defaultMatBody;
        Material defaultMatHands;
        [SerializeField] Material hitMaterial;

        [Space]
        public EnemyAttackPattern pattern;
        [Space]
        public EnemyAttackPattern labyrinth;
        List<GameObject> incomingBombs;

        [SerializeField] Vector3 targetVec;


        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            incomingBombs = new List<GameObject>();
            currentHp = maxHp;            
        }

        // Update is called once per frame
        void Update()
        {
            UpdateMovement();
            
            if(shieldPos.transform.childCount == 0 && canBeHit == false)
            {
                canBeHit = true;
                StopPatterns();
                StartCoroutine(Stun());
            }

            if (canDoPattern && canBeHit == false)
            {
                switch (patternCount)
                {
                    case 0:
                        canDoPattern = false;
                        StartCoroutine(PatternBomb());
                        //StartCoroutine(SpawnMouchou());
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
            
            if(PlayerManager.Instance.currentController.playerRunSpirits.GetActiveSpirits() == 3)
            {
                StopAllCoroutines();
                canDoPattern = false;
                StartCoroutine(ComeClose());
            }

        }

        
        IEnumerator PatternBomb()
        {
            animator.SetInteger("animPatternCount", 1);
            animator.SetBool("animIsAttacking", true);
            targetFeedback.SetActive(true);
            targetFeedback.transform.localPosition = new Vector3(0, 0.3f, -1.5f); 
            timeToBomb = Random.Range(2, 4);
            bombOver = false;
            bombClock = new Clock(timeToBomb);
            bombClock.ClockEnded += EndTimeBomb;
            Vector3 pos1 = new Vector3(targetFeedback.transform.localPosition.x - 2, targetFeedback.transform.localPosition.y, gameObject.transform.localPosition.z -13);
            Vector3 pos2 = new Vector3(targetFeedback.transform.localPosition.x + 2, targetFeedback.transform.localPosition.y, gameObject.transform.localPosition.z -13);
            targetFeedback.transform.localPosition = new Vector3(Random.Range(pos1.x, pos2.x), targetFeedback.transform.localPosition.y, gameObject.transform.localPosition.z -13);

            while (bombOver == false)
            {                
                targetFeedback.transform.localPosition = Vector3.Lerp(pos1, pos2, (Mathf.Sin(Time.time * feedbackPosSpeed) +1) /2);                
                yield return new WaitForEndOfFrame();
            }            
            targetVec = new Vector3(targetFeedback.transform.position.x, targetFeedback.transform.position.y, gameObject.transform.position.z);
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(TargetCell());
            yield return new WaitForSeconds(0.2f);
            targetFeedback.SetActive(false);
            yield return new WaitForSeconds(2f);
            animator.SetBool("animIsAttacking", false);                      
            yield return new WaitForSeconds(5);
            bombOver = false;
            canDoPattern = true;
        }

        IEnumerator PatternLabyrinth()
        {
            animator.SetInteger("animPatternCount", 2);
            animator.SetBool("animIsAttacking", true);
            yield return new WaitForSeconds(0.1f);
            animator.SetBool("animIsAttacking", false);
            yield return new WaitForSeconds(2f);
            TargetCellLabyrinth();
            yield return new WaitForSeconds(10f);            
            canDoPattern = true;
        }

        GameObject marker;

        IEnumerator TargetCell()
        {
            incomingBombs = new List<GameObject>();
            
            for (int x = 0; x < pattern.row.Length; x++)
            {
                for (int y = 0; y < pattern.row[x].column.Length; y++)
                {
                    if (pattern.row[x].column[y] == true)
                    {
                        marker = Instantiate(targetMarker, (new Vector3(targetVec.x + y, targetVec.y, targetVec.z - x -1)), Quaternion.identity, gameObject.transform);
                        incomingBombs.Add(marker);
                        yield return new WaitForSeconds(0.1f);
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
                        marker = Instantiate(targetMarkerLong, (new Vector3((patternPos.transform.position.x + y), (patternPos.transform.position.y), (patternPos.transform.position.z - x))), Quaternion.identity, gameObject.transform);
                        incomingBombs.Add(marker);
                    }
                }
            }
            patternCount++;
        }

        IEnumerator SpawnMouchou()
        {
            animator.SetBool("animIsAttacking", true);
            animator.SetInteger("animPatternCount", 3);
            yield return new WaitForSeconds(3f);
            Instantiate(mouchou, new Vector3(transform.position.x + Random.Range(-3, 3), transform.position.y, transform.position.z -1), Quaternion.identity);
            animator.SetBool("animIsAttacking", false);
            yield return new WaitForSeconds(2.5f);            
            Instantiate(mouchou, new Vector3(transform.position.x + Random.Range(-3, 3), transform.position.y, transform.position.z -1), Quaternion.identity);
            yield return new WaitForSeconds(2.5f);
            Instantiate(mouchou, new Vector3(transform.position.x + Random.Range(-3, 3), transform.position.y, transform.position.z - 1), Quaternion.identity);            
            yield return new WaitForSeconds(2);
            
            patternCount = 0;
            canDoPattern = true;
        }

        IEnumerator ComeClose()
        {
            camDistance = 10;
            yield return new WaitForSeconds(7f);
            patternCount = 0;
            canDoPattern = true;
        }

        IEnumerator Stun()
        {
            animator.Play("BossAno_StunIn");
            animator.SetBool("animIsStuned", true);
            yield return new WaitForSeconds(7);
            if(shieldPos.transform.childCount == 0)
            {
                StunReset();
            }            
        }

        GameObject shieldo;
        void StunReset()
        {
            animator.SetBool("animIsStuned", false);
            animator.SetInteger("animPatternCount", 0);
            animator.SetBool("animIsAttacking", false);
            shieldo = Instantiate(shield, shieldPos.transform.position, Quaternion.identity, shieldPos.transform);
            shieldo.SetActive(false);
            camDistance = 13;
            moveSpeed = moveSpeed * 3;
            canBeHit = false;
            StartCoroutine(InstantiateShield());            
        }

        IEnumerator InstantiateShield()
        {
            yield return new WaitForSeconds(1.7f);
            shieldo.SetActive(true);            
            moveSpeed = moveSpeed / 3;
            yield return new WaitForSeconds(3);
            canDoPattern = true;
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
                    return;
                }
                else
                {
                    rb.velocity = new Vector3(0, 0, 0);
                    return;
                }
            }
            else
                rb.velocity = new Vector3(0, 0, 0);
        }

        void StopPatterns()
        {
            StopAllCoroutines();            
            targetFeedback.SetActive(false);
            patternCount = 0;

            if(incomingBombs.Count > 0)
            {
                for (int i = 0; i < incomingBombs.Count; i++)
                {
                    Destroy(incomingBombs[i]);
                }
            }           
        }

        public void TakeDamage(float damage)
        {
            currentHp -= damage;
            print(currentHp);

            if (currentHp > 0)
            {
                StartCoroutine(HitFeedback());
                StunReset();
                //Instantiate(hitFx, transform.position, Quaternion.identity);
            }
            else if (currentHp <= 0)
            {
                animator.Play("BossAno_StunDeath");
            }
        }

        IEnumerator HitFeedback()
        {
            defaultMatBody = bodyRenderer.material;
            defaultMatHands = handsRenderer.material;

            bodyRenderer.material = hitMaterial;
            handsRenderer.material = hitMaterial;

            yield return new WaitForSeconds(0.1f);

            bodyRenderer.material = defaultMatBody;
            handsRenderer.material = defaultMatHands;
        }
    }
}
