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
        Animator animator;

        [Space]
        [InspectorName("Stats")]
        [SerializeField] float maxHp;
        float currentHp;
        [SerializeField] Transform patternPos;

        int patternCount = 0;

        [Space]
        [InspectorName("Pattern Wave")]
        [SerializeField] float bulletSpeed;
        public EnemyAttackPattern[] waves;
        List<GameObject> incomingBombs;
        GameObject bullet;
        Vector3 bulletDir;
        

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
            animator = GetComponent<Animator>();

            StartCoroutine(PatternWave());
        }

        private void OnEnable()
        {
            StartCoroutine(DisplayHp());
        }

        private void FixedUpdate()
        {
            UpdateMovement();
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

        int wavesIndex;
        IEnumerator PatternWave()
        {
            animator.SetInteger("animPatternCount", 1);
            animator.SetBool("animIsAttacking", true);

            bulletDir = new Vector3(0, 0, -1);
            incomingBombs = new List<GameObject>();

            switch (currentHp)
            {
                case 30:
                    wavesIndex = 0;
                    break;
                case 20:
                    wavesIndex = 1;
                    break;
                case 10:
                    wavesIndex = 2;
                    break;
            }

            for (int x = 0; x < waves[wavesIndex].row.Length; x++)
            {
                for (int y = 0; y < waves[wavesIndex].row[x].column.Length; y++)
                {
                    if (waves[wavesIndex].row[x].column[y] == false)
                    {
                        bullet = Instantiate(projectile, (new Vector3((patternPos.transform.position.x + y), (patternPos.transform.position.y), (patternPos.transform.position.z - x))), Quaternion.identity, gameObject.transform);
                        incomingBombs.Add(bullet);
                        bullet.GetComponent<Rigidbody>().AddForce(bulletDir * bulletSpeed, ForceMode.Force);
                        yield return new WaitForSeconds(0.3f);
                    }
                }
            }
            patternCount++;

            yield return new WaitForSeconds(1f);
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
