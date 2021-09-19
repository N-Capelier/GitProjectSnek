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
    public class BossDepression : Singleton<BossDepression>
    {
        public GameObject cam;
        float camDistance = 13;
        float moveSpeed = 2.5f;

        Rigidbody rb;
        [SerializeField] Animator animator;        

        [Space]
        [Header("Stats")]
        [SerializeField] float maxHp;
        float currentHp;
        [SerializeField] Transform patternPos;
        Vector3 bulletDir;

        int patternCount = 0;
        bool canDoPattern = false;
        bool canBeHit = false;

        [Space]
        [Header("Pattern Mille Masques")]
        public EnemyAttackPattern[] milleMasqueStartPositions;
        public EnemyAttackPattern[] heartsPhase1;
        public EnemyAttackPattern[] heartsPhase2;
        public EnemyAttackPattern[] heartsPhase3;
        public GameObject heartPrefab;
        public GameObject straightMilleMasques;
        public GameObject snakeMilleMasques;

        [Space]
        [Header("Pattern Telekinesis")]
        public EnemyAttackPattern objectsPositions;
        public GameObject[] objectsPrefab;
        private List<GameObject> projectiles = new List<GameObject>();
        private int projectilesCount;
        public float projectileSpeed;
        public float projectileLifetime;
        public GameObject destroyFeedback;
        public GameObject appearFeedback;

        [Space]
        [Header("Pattern Monochrom Objects")]
        public GameObject[] monochromObjects;
        private EnemyAttackPattern pattern;
        public EnemyAttackPattern[] monochromObjectsPatterns;
        public GameObject spellPickup;
        private Coroutine waitForSpellRoutine;
        private Coroutine resetStateRoutine;
        private Coroutine stunExit;
        public EnemyAttackPattern panicPattern;


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
        public PlayableDirector director;
        public TimelineAsset introCinematic;
        public TimelineAsset endCinematic;
        public TerrainGenerator generator;

        private void Awake()
        {
            CreateSingleton();
        }

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            StartCoroutine(DelayedStart());
            //currentHp = SaveManager.Instance.state.bossParanoiaHp * 10;
            currentHp = maxHp;
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

            if (canDoPattern && !canBeHit)
            {
                switch (patternCount)
                {
                    case 0:
                        canDoPattern = false;
                        animator.SetInteger("patternIndex", 1);
                        animator.SetBool("animIsAttacking", true);
                        //StartCoroutine(PatternMilleMasques());
                        return;
                    case 1:
                        canDoPattern = false;
                        animator.SetInteger("patternIndex", 2);
                        animator.SetBool("animIsAttacking", true);
                        //StartCoroutine(PatternTelekinesis());
                        return;
                    case 2:
                        canDoPattern = false;
                        animator.SetInteger("patternIndex", 3);
                        animator.SetBool("animIsAttacking", true);
                        //StartCoroutine(PatternMonochromObjects());
                        return;
                }
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

        IEnumerator PatternMilleMasques()
        {
            int index = 0;
            GameObject prefab = null;
            EnemyAttackPattern[] currentHeartList = null;
            switch (currentHp)
            {
                case 30:
                    //Spawn Mille masque phase 1
                    index = 0;
                    prefab = straightMilleMasques;
                    currentHeartList = heartsPhase1;

                    break;
                case 20:
                    //Spawn Mille masque phase 2
                    index = 1;
                    prefab = snakeMilleMasques;
                    currentHeartList = heartsPhase2;
                    break;
                case 10:
                    //Spawn Mille masque phase 3
                    index = 2;
                    prefab = snakeMilleMasques;
                    currentHeartList = heartsPhase3;
                    break;
            }

            int count = 0;
            for (int x = 0; x < milleMasqueStartPositions[index].row.Length; x++)
            {
                for (int y = 0; y < milleMasqueStartPositions[index].row[x].column.Length; y++)
                {
                    if (milleMasqueStartPositions[index].row[x].column[y] == true)
                    {
                        GameObject milleMasque = Instantiate(prefab, new Vector3(patternPos.transform.position.x + y, 0 , patternPos.transform.position.z - x), prefab.transform.rotation);
                        for (int i = 0; i < currentHeartList[count].row.Length; i++)
                        {
                            for (int j = 0; j < currentHeartList[count].row[i].column.Length; j++)
                            {
                                if (currentHeartList[count].row[i].column[j] == true)
                                {
                                    MilleMasquesBehaviour behaviour = milleMasque.GetComponentInChildren<MilleMasquesBehaviour>();
                                    GameObject heart = Instantiate(heartPrefab, new Vector3((int)patternPos.transform.position.x + j, (int)patternPos.transform.position.y, (int)patternPos.transform.position.z - i), prefab.transform.rotation);
                                    MilleMasquesHeart heartComponent = heart.GetComponent<MilleMasquesHeart>();
                                    heartComponent.behaviour = behaviour;
                                    behaviour.hearts.Add(heartComponent);
                                    behaviour.canDie = true;
                                    print("Spawn Heart");
                                }
                            }
                        }

                        count++;
                    }
                }
            }

            yield return new WaitForEndOfFrame();
            animator.SetBool("animIsAttacking", false);

            yield return new WaitForSeconds(10);
            patternCount++;
            canDoPattern = true;
        }

        IEnumerator PatternTelekinesis()
        {
            switch (currentHp)
            {
                case 30:
                    //Throw one projectile
                    projectilesCount = 1;
                    break;
                case 20:
                    //Throw two projectile
                    projectilesCount = 2;
                    break;
                case 10:
                    //Throw four projectile
                    projectilesCount = 4;
                    break;
            }
            projectiles.Clear();
            for (int x = 0; x < objectsPositions.row.Length; x++)
            {
                for (int y = 0; y < objectsPositions.row[x].column.Length; y++)
                {
                    if (objectsPositions.row[x].column[y] == true)
                    {
                        GameObject prefab = objectsPrefab[Random.Range(0, objectsPrefab.Length)];
                        GameObject temp = Instantiate(prefab, new Vector3((int)patternPos.transform.position.x + y, (int)patternPos.transform.position.y , (int)patternPos.transform.position.z - x + 5), prefab.transform.rotation);
                        Instantiate(appearFeedback, temp.transform.position, Quaternion.identity);
                        projectiles.Add(temp);
                    }
                }
            }

            for (int i = 0; i < projectilesCount; i++)
            {
                //ThrowCoroutine
                StartCoroutine(ThrowProjectile(projectiles[Random.Range(0, projectiles.Count-1)], projectileLifetime));
            }

            animator.SetBool("animIsAttacking", false);

            yield return new WaitForSeconds(10);
            patternCount++;
            canDoPattern = true;
        }

        IEnumerator PatternMonochromObjects()
        {
            switch (currentHp)
            {
                case 30:
                    //Get patter phase 1
                    pattern = monochromObjectsPatterns[0];

                    break;
                case 20:

                    //Get pattern phase 2
                    pattern = monochromObjectsPatterns[1];


                    break;
                case 10:
                    //Get pattern phase 3
                    pattern = monochromObjectsPatterns[2];

                    break;
            }

            yield return null;


            for (int x = 0; x < pattern.row.Length; x++)
            {
                for (int y = 0; y < pattern.row[x].column.Length; y++)
                {
                    if (pattern.row[x].column[y] == true)
                    {
                        //spawn random monochrom object
                        GameObject prefab = monochromObjects[Random.Range(0, monochromObjects.Length)];
                        Instantiate(prefab, new Vector3((int)patternPos.transform.position.x + y, (int)patternPos.transform.position.y, (int)patternPos.transform.position.z - x + 5), prefab.transform.rotation);
                    }
                }
            }

            GameObject pickup = Instantiate(spellPickup, (new Vector3((patternPos.transform.position.x + 4), (patternPos.transform.position.y), (patternPos.transform.position.z + 8))), Quaternion.identity);

            animator.SetBool("animIsAttacking", false);
            waitForSpellRoutine = StartCoroutine(WaitForPickup(pickup));
            resetStateRoutine = StartCoroutine(ResetState(pickup));
        }

        private IEnumerator ThrowProjectile(GameObject projectile, float lifetime)
        {
            yield return new WaitForSeconds(1f);
            Coroutine rotate = StartCoroutine(RotateProjectile(projectile, lifetime+1));
            Vector3 lerpTarget = projectile.transform.position + new Vector3(0,0.8f,0);
            float lerp = 0;
            while (lerp < 1)
            {
                projectile.transform.position = Vector3.Lerp(projectile.transform.position, lerpTarget, lerp);
                lerp += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            float timer = 0;
            Vector3 direction = (PlayerManager.Instance.currentController.objectRenderer.transform.position + new Vector3(0, 0, 3)) - projectile.transform.position;

            while(timer < lifetime)
            {
                projectile.transform.position += direction * Time.deltaTime * projectileSpeed;
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            //Instantiate fx + destroy projectile
            Instantiate(destroyFeedback, projectile.transform.position, Quaternion.identity);
            StopCoroutine(rotate);
            Destroy(projectile);
        }

        private IEnumerator RotateProjectile(GameObject projectile, float lifetime)
        {
            Vector3 randRotation = new Vector3(Random.Range(0f ,1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            float timer = 0;
            while (timer < lifetime)
            {
                projectile.transform.rotation *= Quaternion.Euler(randRotation.x * timer, randRotation.y * timer, randRotation.z * timer);
                timer += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator WaitForPickup(GameObject pickup)
        {
            yield return new WaitUntil(()=> pickup == null);
            StopCoroutine(resetStateRoutine);
            StartCoroutine(PanicPattern());
        }

        private IEnumerator ResetState(GameObject pickup)
        {
            yield return new WaitForSeconds(15);
            StopCoroutine(waitForSpellRoutine);
            Instantiate(appearFeedback, pickup.transform.position, Quaternion.identity);
            Destroy(pickup);
            patternCount = 0;
            canDoPattern = true;
            canBeHit = false;
        }

        private IEnumerator PanicPattern()
        {
            for (int x = 0; x < panicPattern.row.Length; x++)
            {
                for (int y = 0; y < panicPattern.row[x].column.Length; y++)
                {
                    if (panicPattern.row[x].column[y] == true)
                    {
                        //spawn random monochrom object
                        GameObject prefab = monochromObjects[Random.Range(0, monochromObjects.Length)];
                        Instantiate(prefab, new Vector3((int)patternPos.transform.position.x + y, (int)patternPos.transform.position.y, (int)patternPos.transform.position.z - x), prefab.transform.rotation);
                    }
                }
            }

            yield return new WaitUntil(() => PlayerManager.Instance.currentController.isDead || PlayerManager.Instance.currentController.playerRunSpell.buttonImage.fillAmount < 1);

            if (PlayerManager.Instance.currentController.isDead)
            {
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(0.3f);
                animator.SetBool("animIsHit", false);
                animator.SetBool("animIsStunned", true);
                canBeHit = true;
                camDistance = 7;
            }

            stunExit = StartCoroutine(StunExit());
        }

        IEnumerator StunExit()
        {
            yield return new WaitForSeconds(10);

            StunReset();

        }

        void StunReset()
        {
            if (stunExit != null)
            {
                StopCoroutine(stunExit);
            }
            animator.SetBool("animIsStunned", false);

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
            PlayerManager.Instance.currentController.runController.SetSpell(Player.Controller.Spell.None);
            patternCount = 0;
            canDoPattern = true;
            canBeHit = false;
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
            SaveManager.Instance.state.bossParanoiaHp--;
            SaveManager.Instance.Save();

            if (currentHp > 0)
            {
                animator.SetBool("animIsHit", true);
                StartCoroutine(HitFeedback());
                StunReset();
                StartCoroutine(DisplayHp());
            }
            else if (currentHp <= 0)
            {
                StopAllCoroutines();
                CutsceneManager.Instance.StopMusic();
                director.playableAsset = endCinematic;
                bodyRenderer.enabled = false;
                handsRenderer.enabled = false;
                generator.bossIsDead = true;
                generator.GenerateEndTerrains();
                cam.SetActive(false);
                //SaveManager.Instance.state.isDemoFinished = true;
                SaveManager.Instance.Save();
                PlayerManager.Instance.currentController.gameObject.SetActive(false);
                director.Play();
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
