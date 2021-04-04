﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
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
        //Transform targetPos;

        Clock bombClock;

        //int patternOrder = 0;
        int patternCount = 0;
        [SerializeField] float timeToBomb;
        public float speed = 3;
        bool bombOver = false;
        //bool canBeHit = false;
        bool canDoPattern = true;

        [Space]
        public EnemyAttackPattern pattern;
        [Space]
        public EnemyAttackPattern labyrinth;

        [SerializeField] Vector3 targetVec;

        // Update is called once per frame
        void Update()
        {
            
            if (canDoPattern)
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
            Vector3 pos1 = new Vector3(targetFeedback.transform.position.x -2, targetFeedback.transform.position.y, targetFeedback.transform.position.z);
            Vector3 pos2 = new Vector3(targetFeedback.transform.position.x + 2, targetFeedback.transform.position.y, targetFeedback.transform.position.z);
            targetFeedback.transform.position = new Vector3(Random.Range(pos1.x, pos2.x), targetFeedback.transform.position.y, targetFeedback.transform.position.z);

            while (bombOver == false)
            {                
                targetFeedback.transform.position = Vector3.Lerp(pos1, pos2, (Mathf.Sin(Time.time * speed) +1) /2);                
                yield return new WaitForEndOfFrame();
            }
            targetVec = new Vector3(targetFeedback.transform.position.x, targetFeedback.transform.position.y, targetFeedback.transform.position.z);
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
            for (int x = 0; x < pattern.row.Length; x++)
            {
                for (int y = 0; y < pattern.row[x].column.Length; y++)
                {
                    if (pattern.row[x].column[y] == true)
                    {
                        Instantiate(targetMarker, (new Vector3(targetVec.x + y, targetVec.y, targetVec.z - x)), Quaternion.identity);
                    }
                }
            }
            patternCount++;            
        }

        void TargetCellLabyrinth()
        {
            for (int x = 0; x < labyrinth.row.Length; x++)
            {
                for (int y = 0; y < labyrinth.row[x].column.Length; y++)
                {
                    if (labyrinth.row[x].column[y] == false)
                    {
                        Instantiate(targetMarkerLong, (new Vector3((patternPos.transform.position.x + y), (patternPos.transform.position.y), (patternPos.transform.position.z - x))), Quaternion.identity);
                    }
                }
            }
            patternCount++;
        }

        IEnumerator SpawnMouchou()
        {
            yield return new WaitForSeconds(1f);
            Instantiate(mouchou, new Vector3(transform.position.x + Random.Range(-3, 3), transform.position.y, transform.position.z -1), Quaternion.identity);
            yield return new WaitForSeconds(5f);
            Instantiate(mouchou, new Vector3(transform.position.x + Random.Range(-3, 3), transform.position.y, transform.position.z -1), Quaternion.identity);
            yield return new WaitForSeconds(4);
            patternCount = 0;
            canDoPattern = true;
        }

        void EndTimeBomb()
        {
            bombOver = true;
        }
    }
}