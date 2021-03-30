using System.Collections;
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
        Transform targetPos;

        int patternOrder = 0;
        int patternCount = 0;
        bool canBeHit = false;
        bool canDoPattern = true;

        [Space]
        public EnemyAttackPattern pattern;
        [Space]
        public EnemyAttackPattern labyrinth;

        

        // Start is called before the first frame update
        void Start()
        {
            
        }

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
            TargetCell();
            yield return new WaitForSeconds(10);
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
                        Instantiate(targetMarker, (new Vector3((patternPos.transform.position.x + y), (patternPos.transform.position.y), (patternPos.transform.position.z - x))), Quaternion.identity);
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

    }
}
