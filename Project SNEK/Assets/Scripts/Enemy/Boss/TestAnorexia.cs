using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using Player;
using UnityEngine.Timeline;
using UnityEngine.Playables;
using Cinematic;
using AudioManagement;
using Saving;

namespace Boss
{
    public class TestAnorexia : Singleton<TestAnorexia>
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
        [SerializeField] bool canDoPattern = true;
        bool isTaunt = false;

        [Space]
        [SerializeField] SkinnedMeshRenderer bodyRenderer;
        [SerializeField] SkinnedMeshRenderer handsRenderer;
        Material defaultMatBody;
        Material defaultMatHands;
        [SerializeField] Material hitMaterial;

        [Space]
        public PlayableDirector director;
        public TimelineAsset endCinematic;
        public GameObject endGraphs;
        public TerrainGenerator generator;

        [Space]
        public EnemyAttackPattern pattern;
        [Space]
        public EnemyAttackPattern[] labyrinth;
        List<GameObject> incomingBombs;

        [SerializeField] Vector3 targetVec;

        public TimelineAsset introCinematic;
        WaitForSeconds markerDelay = new WaitForSeconds(.1f);

        private void Awake()
        {
            CreateSingleton();
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            incomingBombs = new List<GameObject>();
            currentHp = maxHp;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.J))
            {
                Debug.Log(Rendering.Run.RunCamController.Instance.ActiveState);
            }
        }

        void FixedUpdate()
        {
            UpdateMovement();
            
            if(shieldPos.transform.childCount == 0 && canBeHit == false)
            {
                canBeHit = true;
                StopPatterns();
                StartCoroutine(Stun());
            }

            if (canDoPattern && !canBeHit)
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
            
            if(PlayerManager.Instance.currentController != null)
            {
                if (PlayerManager.Instance.currentController.playerRunSpirits.GetActiveSpirits() >= 3 && isTaunt == false)
                {                    
                    animator.SetBool("animIsAttacking", false);
                    StopAllCoroutines();
                    canDoPattern = false;
                    isTaunt = true;
                    StartCoroutine(ComeClose());
                }
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
            targetVec = new Vector3(PlayerManager.Instance.currentController.transform.position.x, targetFeedback.transform.position.y, gameObject.transform.position.z);
            targetVec = SnapPosition(targetVec);
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(TargetCell());            
            yield return new WaitForSeconds(0.2f);
            targetFeedback.SetActive(false);
            yield return new WaitForSeconds(2f);
            animator.SetBool("animIsAttacking", false);
            AudioManager.Instance.PlayThisSoundEffect("BossGrunt");
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
            AudioManager.Instance.PlayThisSoundEffect("StartPoisonRain");
            yield return new WaitForSeconds(10f);            
            canDoPattern = true;
        }

        GameObject marker;

        public Vector3 SnapPosition(Vector3 vector)
        {
            return new Vector3(
                Mathf.RoundToInt(vector.x),
                vector.y,
                Mathf.RoundToInt(vector.z)
                );
        }

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
                        yield return markerDelay;
                    }
                }
            }
            patternCount++;            
        }

        int i;
        void TargetCellLabyrinth()
        {
            incomingBombs = new List<GameObject>();

            switch (currentHp)
            {
                case 30:
                    i = 0;
                    break;
                case 20:
                    i = 1;
                    break;
                case 10:
                    i = 2;
                    break;
            }

            for (int x = 0; x < labyrinth[i].row.Length; x++)
            {
                for (int y = 0; y < labyrinth[i].row[x].column.Length; y++)
                {
                    if (labyrinth[i].row[x].column[y] == false)
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
            AudioManager.Instance.PlayThisSoundEffect("SummonMouchou");
            Instantiate(mouchou, new Vector3(transform.position.x + Random.Range(-3, 3), transform.position.y, transform.position.z -1), Quaternion.identity);
            animator.SetBool("animIsAttacking", false);
            yield return new WaitForSeconds(2.5f);
            AudioManager.Instance.PlayThisSoundEffect("SummonMouchou");
            Instantiate(mouchou, new Vector3(transform.position.x + Random.Range(-3, 3), transform.position.y, transform.position.z -1), Quaternion.identity);
            yield return new WaitForSeconds(2.5f);
            AudioManager.Instance.PlayThisSoundEffect("SummonMouchou");
            Instantiate(mouchou, new Vector3(transform.position.x + Random.Range(-3, 3), transform.position.y, transform.position.z - 1), Quaternion.identity);            
            yield return new WaitForSeconds(2);
            
            patternCount = 0;
            canDoPattern = true;
        }

        IEnumerator ComeClose()
        {
            animator.Play(Animator.StringToHash("BossAno_TauntIn"));
            animator.SetBool("animIsTaunt", true);
            camDistance = 9;
            yield return new WaitForSeconds(7f);
            animator.SetBool("animIsTaunt", false);
            camDistance = 13;
            moveSpeed = moveSpeed * 3;
            yield return new WaitForSeconds(0.5f);
            playerSpirits = PlayerManager.Instance.currentController.playerRunSpirits.GetActiveSpirits();
            PlayerManager.Instance.currentController.playerRunSpirits.ConsumeSpirits(playerSpirits);
            moveSpeed = moveSpeed / 3;
            yield return new WaitForSeconds(4f);
            patternCount = 0;
            canDoPattern = true;
            isTaunt = false;
        }

        IEnumerator Stun()
        {
            camDistance = 7;
            animator.Play(Animator.StringToHash("BossAno_StunIn"));
            animator.SetBool("animIsStuned", true);
            animator.SetBool("animIsTaunt", false);
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

        int playerSpirits;
        IEnumerator InstantiateShield()
        {
            yield return new WaitForSeconds(1.7f);
            shieldo.SetActive(true);            
            moveSpeed = moveSpeed / 3;
            yield return new WaitForSeconds(1f);
            playerSpirits = PlayerManager.Instance.currentController.playerRunSpirits.GetActiveSpirits();
            PlayerManager.Instance.currentController.playerRunSpirits.ConsumeSpirits(playerSpirits);
            yield return new WaitForSeconds(3f);
            canDoPattern = true;
            isTaunt = false;
        }



        void EndTimeBomb()
        {
            bombOver = true;
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

        public void StartPatterns()
        {
            StopAllCoroutines();
            targetFeedback.SetActive(false);
            patternCount = 0;
            canDoPattern = true;
        }

        public void TakeDamage(float damage)
        {
            currentHp -= damage;
            print(currentHp);
            AudioManager.Instance.PlayThisSoundEffect("BossHit");

            if (currentHp > 0)
            {
                StartCoroutine(HitFeedback());
                StunReset();
                //Instantiate(hitFx, transform.position, Quaternion.identity);
            }
            else if (currentHp <= 0)
            {
                StopAllCoroutines();
                CutsceneManager.Instance.StopMusic();
                director.playableAsset = endCinematic;
                bodyRenderer.enabled = false;
                handsRenderer.enabled = false;
                generator.bossIsDead = true;
                generator.GenerateStartTerrain();
                endGraphs.SetActive(true);
                cam.SetActive(false);
                SaveManager.Instance.state.isDemoFinished = true;
                SaveManager.Instance.Save();
                PlayerManager.Instance.currentController.gameObject.SetActive(false);
                director.Play();
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
