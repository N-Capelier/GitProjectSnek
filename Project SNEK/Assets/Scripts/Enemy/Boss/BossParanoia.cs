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
        bool canDoPattern = false;
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
        [InspectorName("Pattern Target")]
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
        [InspectorName("Pattern Spawn")]
        public GameObject mouchouPrefab;        
        public GameObject shrubPrefab;
        public GameObject spawnFx;
        public GameObject spellPickup;
        GameObject shrub;
        List<GameObject> shrubs;
        public EnemyAttackPattern pattern;
        bool isSpawning = false;
        int mouchouNumber;

        [Space]
        [InspectorName("Pattern MegaBeam")]
        bool isMegaBeam = false;
        public GameObject megaBeamObject;
        public GameObject hand1;
        public GameObject hand2;
        public float timeOfBeam;
        

        [Space]
        [InspectorName("UI")] 
        [SerializeField] GameObject hpUi;
        [SerializeField] TextMeshProUGUI hpText;

        [Space]
        [InspectorName("Render")]
        [SerializeField] SkinnedMeshRenderer bodyRenderer;
        [SerializeField] SkinnedMeshRenderer handsRenderer;
        Material defaultMatBody;
        Material defaultMatHands;
        [SerializeField] Material hitMaterial;

        private void Awake()
        {
            CreateSingleton();
        }

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            currentHp = maxHp;
            StartCoroutine(DelayedStart());
        }

        IEnumerator DelayedStart()
        {
            yield return new WaitForSeconds(3);

            canDoPattern = true;
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
                        StartCoroutine(PatternMouchou());
                        return;
                    case 3:
                        canDoPattern = false;
                        StartCoroutine(MegaBeam());
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
            for (int j = 0; j < incomingBombs.Count; j++)
            {
                incomingBombs[j].GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            }

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

        int blastIndex = 0;
        IEnumerator PatternTarget()
        {
            blastIndex++;
            yield return new WaitForSeconds(4);

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
            blastObject.transform.LookAt(targetObject.transform.position);
            Destroy(targetObject);

            ResetPatternTarget();

        }

        IEnumerator FastPatternTarget()
        {
            blastIndex++;
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
            blastObject.transform.LookAt(targetObject.transform.position);
            Destroy(targetObject);

            ResetPatternTarget();

        }

        private void ResetPatternTarget()
        {
            switch (currentHp)
            {
                case 30:
                    patternCount++;
                    canDoPattern = true;
                    blastIndex = 0;
                    break;
                case 20:
                    if (blastIndex >= 3)
                    {
                        patternCount++;
                        canDoPattern = true;
                        blastIndex = 0;
                    }
                    else
                        StartCoroutine(FastPatternTarget());
                    break;
                case 10:
                    if (blastIndex >= 5)
                    {
                        patternCount++;
                        canDoPattern = true;
                        blastIndex = 0;
                    }
                    else
                        StartCoroutine(FastPatternTarget());
                    break;
            }
        }

        int mouchouClone;
        IEnumerator PatternMouchou()
        {
            switch (currentHp)
            {
                case 30:
                    mouchouNumber = 2;
                    break;
                case 20:
                    mouchouNumber = 4;
                    break;
                case 10:
                    mouchouNumber = 6;
                    break;
            }

            yield return new WaitForSeconds(2);

            animator.SetInteger("animPatternCount", 3);
            animator.SetBool("animIsAttacking", true);

            yield return new WaitUntil(() => isSpawning);

            animator.SetBool("animIsAttacking", false);

            shrubs = new List<GameObject>();

            for (int x = 0; x < pattern.row.Length; x++)
            {
                for (int y = 0; y < pattern.row[x].column.Length; y++)
                {
                    if (pattern.row[x].column[y] == true)
                    {
                        shrub = Instantiate(shrubPrefab, (new Vector3((patternPos.transform.position.x + y), (patternPos.transform.position.y), (patternPos.transform.position.z - x + 5))), Quaternion.identity);
                        Instantiate(spawnFx, (new Vector3((patternPos.transform.position.x + y), (patternPos.transform.position.y), (patternPos.transform.position.z - x + 5))), Quaternion.identity);
                        shrubs.Add(shrub);
                    }
                }
            }

            for (int j = 0; j < mouchouNumber; j++)
            {
                mouchouClone = Random.Range(0, shrubs.Count);
                for (int i = 0; i < shrubs.Count; i++)
                {
                    if (i == mouchouClone)
                    {
                        Instantiate(mouchouPrefab, shrubs[i].transform.position, Quaternion.identity);
                        Destroy(shrubs[i]);
                        shrubs.Remove(shrubs[i]);
                    }
                }
            } 
            
            Instantiate(spellPickup, (new Vector3((patternPos.transform.position.x + 4), (patternPos.transform.position.y), (patternPos.transform.position.z + 8))), Quaternion.identity);

            yield return new WaitForSeconds(7);

            ResetPatternSpawn();

        }

        private void ResetPatternSpawn()
        {
            patternCount++;
            isSpawning = false;
            canDoPattern = true;
        }

        public void AnimSpawn()
        {
            isSpawning = true;
        }

        GameObject beam1;
        GameObject beam2;
        GameObject beam3;

        IEnumerator MegaBeam()
        {
            animator.SetInteger("animPatternCount", 4);
            animator.SetBool("animIsAttacking", true);

            yield return new WaitUntil(() => isMegaBeam);

            beam1 = Instantiate(megaBeamObject, blastPos);
            beam2 = Instantiate(megaBeamObject, hand1.transform);
            beam3 = Instantiate(megaBeamObject, hand2.transform);

            beam1.transform.LookAt(new Vector3(PlayerManager.Instance.currentController.transform.position.x, PlayerManager.Instance.currentController.transform.position.y, PlayerManager.Instance.currentController.transform.position.z - 4));
            beam2.transform.LookAt(new Vector3(PlayerManager.Instance.currentController.transform.position.x, PlayerManager.Instance.currentController.transform.position.y, PlayerManager.Instance.currentController.transform.position.z - 4));
            beam3.transform.LookAt(new Vector3(PlayerManager.Instance.currentController.transform.position.x, PlayerManager.Instance.currentController.transform.position.y, PlayerManager.Instance.currentController.transform.position.z - 4));

            yield return new WaitForSeconds(timeOfBeam);

            animator.SetBool("animIsAttacking", false);
            Destroy(beam1);
            Destroy(beam2);
            Destroy(beam3);
            canBeHit = true;
            camDistance = 7;

        }

        public void AnimMegaBeam()
        {
            isMegaBeam = true;
        }

        void StunReset()
        {
            animator.SetBool("animIsHit", true);
            animator.SetInteger("animPatternCount", 0);
            animator.SetBool("animIsAttacking", false);            
            camDistance = 13;
            moveSpeed = moveSpeed * 3;
            canBeHit = false;
            StartCoroutine(ResetSpell());
        }

        IEnumerator ResetSpell()
        {
            yield return new WaitForSeconds(1.7f);
            moveSpeed = moveSpeed / 3;
            //PlayerManager.Instance.currentController.runController.SetSpell(None);
            patternCount = 0;
            canDoPattern = true;
            canBeHit = false;
            isMegaBeam = false;
            animator.SetBool("animIsHit", false);
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

        public void TakeDamage(float damage)
        {
            currentHp -= damage;
            AudioManager.Instance.PlayThisSoundEffect("BossHit");
            //SaveManager.Instance.state.bossAnorexiaHp--;
            SaveManager.Instance.Save();

            if (currentHp > 0)
            {
                StartCoroutine(HitFeedback());
                StunReset();
                StartCoroutine(DisplayHp());
            }
            else if (currentHp <= 0)
            {
                StopAllCoroutines();
                CutsceneManager.Instance.StopMusic();
                /*director.playableAsset = endCinematic;
                bodyRenderer.enabled = false;
                handsRenderer.enabled = false;
                generator.bossIsDead = true;
                generator.GenerateEndTerrains();
                endGraphs.SetActive(true);
                cam.SetActive(false);
                SaveManager.Instance.state.isDemoFinished = true;
                SaveManager.Instance.Save();
                PlayerManager.Instance.currentController.gameObject.SetActive(false);
                director.Play();*/
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
}
