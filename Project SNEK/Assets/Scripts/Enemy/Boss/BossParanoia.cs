using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagement;
using Enemy;
using Player;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Cinematic;
using AudioManagement;
using Saving;
using TMPro;

namespace Boss
{
    public class BossParanoia : Singleton<BossParanoia>
    {
        [SerializeField] GameObject projectile;
        public GameObject cam;
        float camDistance = 13;
        float moveSpeed = 2.5f;

        Rigidbody rb;
        [SerializeField] Animator animator;        

        [Space]
        [InspectorName("Stats")]
        [SerializeField] float maxHp;
        float currentHp;
        [SerializeField] Transform patternPos;
        Vector3 bulletDir;

        int patternCount = 0;
        bool canDoPattern = true;
        bool canBeHit = false;

        [Space]
        [InspectorName("Pattern Wave")]
        [SerializeField] float bulletSpeed;
        public EnemyAttackPattern[] waves;
        List<GameObject> incomingBombs;
        GameObject bullet;
        int wavesIndex;
        int waveCount;
        bool canSpawnWave = false;
        bool isWaveEnded = false;
        bool isShootingWave = false;
        bool isPatternWaveEnded = false;

        [Space]
        [InspectorName("Pattern Wave")]
        public GameObject followParticle;
        GameObject followObject;
        public GameObject targetParticle;
        GameObject targetObject;
        public GameObject blastParticle;
        GameObject blastObject;
        public float timeToLock;
        public float timeToBlast;
        public Transform blastPos;


        [Space]
        [InspectorName("UI")] 
        [SerializeField] GameObject hpUi;
        [SerializeField] TextMeshProUGUI hpText;

        private void Awake()
        {
            CreateSingleton();
        }

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            currentHp = maxHp;            
        }

        private void OnEnable()
        {
            StartCoroutine(DisplayHp());
        }

        private void FixedUpdate()
        {
            UpdateMovement();
            UpdateBulletsMovement();

            if (canDoPattern && !canBeHit)
            {
                switch (patternCount)
                {
                    case 0:
                        canDoPattern = false;
                        StartCoroutine(PatternWave());
                        return;
                    case 1:
                        canDoPattern = false;
                        StartCoroutine(PatternTarget());
                        return;
                    case 2:
                        canDoPattern = false;
                        //StartCoroutine(SpawnMouchou());
                        return;
                }
            }

            if(followObject != null)
            {
                followObject.transform.position = PlayerManager.Instance.currentController.transform.position;
            }
        }

        void UpdateMovement()
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

        public void UpdateBulletsMovement()
        {
            if (canSpawnWave == true && isShootingWave == false)
            {
                for (int i = 0; i < incomingBombs.Count; i++)
                {
                    incomingBombs[i].GetComponent<Rigidbody>().velocity = rb.velocity;
                }
            }
        }


        IEnumerator PatternWave()
        {
            bulletDir = new Vector3(0, 0, -1);
            incomingBombs = new List<GameObject>();

            switch (currentHp)
            {
                case 30:
                    wavesIndex = 0;
                    animator.SetBool("rightSide", true);
                    break;
                case 20:
                    if (waveCount == 0)
                    {
                        wavesIndex = 1;
                        animator.SetBool("rightSide", false);
                    }
                    else
                        wavesIndex = 2;
                    animator.SetBool("rightSide", true);
                    break;
                case 10:
                    if (waveCount == 0)
                    {
                        wavesIndex = 0;
                        animator.SetBool("rightSide", true);
                    }
                    else if(waveCount == 1)
                    {
                        wavesIndex = 1;
                        animator.SetBool("rightSide", false);
                    }
                    else if (waveCount == 2)
                    {
                        wavesIndex = 2;
                        animator.SetBool("rightSide", true);
                    }
                    break;
            }

            yield return new WaitForEndOfFrame();
            animator.SetInteger("animPatternCount", 1);
            animator.SetBool("animIsAttacking", true);

            yield return new WaitUntil(() => canSpawnWave);

            StartCoroutine(SpawnWave());

            yield return new WaitUntil(() => isShootingWave);

            StartCoroutine(ShootWave());

            yield return new WaitUntil(() => isPatternWaveEnded);

            ResetPatternWave();                  
        }

        private IEnumerator SpawnWave()
        {
            for (int x = 0; x < waves[wavesIndex].row.Length; x++)
            {
                for (int y = 0; y < waves[wavesIndex].row[x].column.Length; y++)
                {
                    if (waves[wavesIndex].row[x].column[y] == true)
                    {
                        bullet = Instantiate(projectile, (new Vector3((patternPos.transform.position.x + y), (patternPos.transform.position.y), (patternPos.transform.position.z - x - 2))), Quaternion.identity, patternPos);
                        incomingBombs.Add(bullet);
                        yield return new WaitForSeconds(0.07f);
                    }
                }
            }
            waveCount++;
        }

        public IEnumerator ShootWave()
        {
            /*for (int j = 0; j < incomingBombs.Count; j++)
            {
                incomingBombs[j].GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            }*/

            switch (wavesIndex)
            {
                case 0:
                    for (int i = incomingBombs.Count - 1; i >= 0; i--)
                    {
                        incomingBombs[i].GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                        incomingBombs[i].GetComponent<Rigidbody>().AddForce(bulletDir.normalized * bulletSpeed, ForceMode.Force);
                        yield return new WaitForSeconds(0.25f);
                    }
                    break;
                case 1:
                    for (int i = 0; i < incomingBombs.Count; i++)
                    {
                        incomingBombs[i].GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                        incomingBombs[i].GetComponent<Rigidbody>().AddForce(bulletDir.normalized * bulletSpeed, ForceMode.Force);
                        yield return new WaitForSeconds(0.25f);
                    }
                    break;
                case 2:
                    for (int i = incomingBombs.Count - 1; i >= 0; i--)
                    {
                        incomingBombs[i].GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                        incomingBombs[i].GetComponent<Rigidbody>().AddForce(bulletDir.normalized * bulletSpeed, ForceMode.Force);
                        yield return new WaitForSeconds(0.25f);
                    }
                    break;
            }

            animator.SetBool("animIsAttacking", false);

            isPatternWaveEnded = true;

        }

        public void CanSpawnWave()
        {
            canSpawnWave = true;
        }

        public void AnimShootWave()
        {
            isShootingWave = true;
        }

        private void ResetPatternWave()
        {
            switch (currentHp)
            {
                case 30:
                   if(waveCount == 1)
                    {
                        patternCount++;
                        canDoPattern = true;
                        isShootingWave = false;
                        isPatternWaveEnded = false;
                        isWaveEnded = false;
                        canSpawnWave = false;
                        waveCount = 0;
                    }
                    break;
                case 20:
                    if (waveCount == 2)
                    {
                        patternCount++;
                        canDoPattern = true;
                        isShootingWave = false;
                        isPatternWaveEnded = false;
                        isWaveEnded = false;
                        canSpawnWave = false;
                        waveCount = 0;
                    }
                    else
                    {
                        isShootingWave = false;
                        isPatternWaveEnded = false;
                        isWaveEnded = false;
                        canSpawnWave = false;
                        StartCoroutine(PatternWave());
                    }                        
                    break;
                case 10:
                    if (waveCount == 3)
                    {
                        patternCount++;
                        canDoPattern = true;
                        isShootingWave = false;
                        isPatternWaveEnded = false;
                        isWaveEnded = false;
                        canSpawnWave = false;
                        waveCount = 0;
                    }
                    else
                    {
                        isShootingWave = false;
                        isPatternWaveEnded = false;
                        isWaveEnded = false;
                        canSpawnWave = false;
                        StartCoroutine(PatternWave());
                    }
                    break;
            }
        }


        IEnumerator PatternTarget()
        {
            yield return new WaitForSeconds(2);

            animator.SetInteger("animPatternCount", 2);
            animator.SetBool("animIsAttacking", true);

            yield return new WaitForSeconds(0.5f);

            followObject = Instantiate(followParticle, PlayerManager.Instance.currentController.transform.position, Quaternion.identity);

            yield return new WaitForSeconds(timeToLock);

            targetObject = Instantiate(targetParticle, followObject.transform.position, Quaternion.identity);
            Destroy(followObject);

            yield return new WaitForSeconds(timeToBlast);

            animator.SetBool("animIsAttacking", false);

            yield return new WaitForSeconds(1.5f);

            bulletDir = (PlayerManager.Instance.currentController.transform.position - transform.position);
            blastObject = Instantiate(blastParticle, blastPos.position, Quaternion.identity);
            //blastObject.transform.Rotate(bulletDir.normalized);
            Destroy(targetObject);

        }


        IEnumerator DisplayHp()
        {
            yield return new WaitForSeconds(0.5f);
            hpUi.transform.LeanScale(Vector3.one, 0.4f);
            switch (currentHp)
            {
                case 30:
                    hpText.text = 3.ToString();
                    break;
                case 20:
                    hpText.text = 2.ToString();
                    break;
                case 10:
                    hpText.text = 1.ToString();
                    break;
            }
            yield return new WaitForSeconds(3f);
            hpUi.transform.LeanScale(Vector3.zero, 0.2f);

        }

    }
}
