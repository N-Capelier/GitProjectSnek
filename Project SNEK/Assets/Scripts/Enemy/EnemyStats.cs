using UnityEngine;
using Player;
using AudioManagement;
using System.Collections;

namespace Enemy
{
    /// <summary>
    /// Arthur
    /// </summary>
    public class EnemyStats : MonoBehaviour
    {
        [HideInInspector] public Clock attackClock;
        [HideInInspector] public Clock movementClock;

        [SerializeField] [Range(0f, 15f)] float attackCooldown = 5f;
        [SerializeField] [Range(0f, 10f)] float movementCooldown = 2f;

        [Space]
        [SerializeField] float maxHp = 1;
        [HideInInspector] public float currentHp;
        [SerializeField] GameObject deathFx, hitFx;
        [SerializeField] MeshRenderer enemyRenderer;
        [SerializeField] SkinnedMeshRenderer skinnedRenderer;
        Material defaultMat;
        [SerializeField] Material hitMaterial;

        [Space]
        [HideInInspector] public Rigidbody rb = null;
        [Range(0, 5)] public int moveSpeed = 1;

        //[Space]
        //[SerializeField] [Range(0, 5)] int spiritLoot = 1;

        private void Awake()
        {
            currentHp = maxHp;
            rb = GetComponent<Rigidbody>();

            attackClock = new Clock(attackCooldown);
            movementClock = new Clock(movementCooldown);
        }

        void Start()
        {
            attackClock.ClockEnded += onTimerAttackEnd;
            movementClock.ClockEnded += onTimerMovementEnd;
        }

        #region Clock Methods


        void onTimerAttackEnd()
        {
            attackClock.SetTime(attackCooldown);
        }

        void onTimerMovementEnd()
        {
            movementClock.SetTime(movementCooldown);
        }

        #endregion

        public void TakeDamage(float damage)
        {
            currentHp -= damage;

            if (currentHp > 0)
            {
                StartCoroutine(HitFeedback());
                Instantiate(hitFx, transform.position, Quaternion.identity);
            }
            else
                Death();
        }

        IEnumerator HitFeedback()
        {
            if (enemyRenderer != null)
            {
                defaultMat = enemyRenderer.material;
                enemyRenderer.material = hitMaterial;
            }
            else if (skinnedRenderer != null)
            {
                defaultMat = skinnedRenderer.material;
                skinnedRenderer.material = hitMaterial;
            }
            yield return new WaitForSeconds(0.1f);
            if (enemyRenderer != null)
                enemyRenderer.material = defaultMat;
            else if (skinnedRenderer != null)
                skinnedRenderer.material = defaultMat;
        }

        public void Death()
        {
            PlayerManager.Instance.currentController.playerRunSpirits.AddSpirit();
            Instantiate(deathFx, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            //Instantie Esprit( x SpiritLoot)
            attackClock.ClockEnded -= onTimerAttackEnd;
            movementClock.ClockEnded -= onTimerMovementEnd;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("PlayerController"))
            {
                PlayerManager.Instance.currentController.Death();
            }
        }

    }
}

