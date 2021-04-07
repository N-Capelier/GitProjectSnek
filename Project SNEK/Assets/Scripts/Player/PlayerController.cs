using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Player.Attack;
using Player.Spirits;
using Rendering.Run;
using AudioManagement;

namespace Player.Controller
{
    public enum PlayerDirection
    {
        Up,
        Right,
        Down,
        Left
    }

    /// <summary>
    /// Nico
    /// </summary>
    public abstract class PlayerController : MonoBehaviour
    {
        [HideInInspector] public PlayerRunAttack playerRunAttack;
        public SpiritManager playerRunSpirits;

        public delegate void PlayerDeath();
        public static event PlayerDeath PlayerDead;

        public Vector3 startingNode = new Vector3(5, 0, 0);
        [HideInInspector] public PlayerDirection currentDirection;
        [HideInInspector] public PlayerDirection nextDirection;

        [Space]
        [HideInInspector] public Rigidbody rb = null;
        [Range(0, 400)] public float moveSpeed = 50;
        [HideInInspector] public float attackMoveSpeedModifier = 1f;
        [HideInInspector] public float spellMoveSpeedModifier = 1f;

        [Space]
        [SerializeField] [Range(0, 10)] int baseHP = 4;
        int currentHP;
        [HideInInspector] public bool isDead = false;
        public Transform checkPoint;
        public Vector3 respawnNode;

        public GameObject objectRenderer;
        public GameObject deathFx;
        public Animator animator;

        [SerializeField] GameObject hpUi;
        [SerializeField] TextMeshProUGUI hpText;

        public virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Init(int _bonusHP, int _bonusRange, int _bonusPower)
        {
            currentHP = baseHP + _bonusHP;
            playerRunAttack.rangeBonus += _bonusRange;
            playerRunAttack.rangeBonusOffSet += _bonusRange * 0.5f;
            playerRunAttack.attackDamages += _bonusPower;
        }

        public void Death()
        {
            currentHP--;
            PlayerDead?.Invoke();
            isDead = true;
            if(currentHP <= 0)
            {
                StartCoroutine(DeathCoroutine());
            }
            else
            {
                StartCoroutine(RespawnCoroutine());
            }
        }

        public void SnapPosition()
        {
            transform.position = new Vector3(
                Mathf.RoundToInt(transform.position.x),
                transform.position.y,
                Mathf.RoundToInt(transform.position.z)
                );
        }

        IEnumerator RespawnCoroutine()
        {
            //play death anim
            Instantiate(deathFx, transform.position, Quaternion.identity);
            objectRenderer.GetComponent<Animator>().Play("Anim_PlayerRun_death");
            AudioManager.Instance.PlaySoundEffect("PlayerHit");
            yield return new WaitForSeconds(1.5f);
            PlayerManager.Instance.gameObject.SetActive(false);            
            /*AsyncOperation _loadingScene = */
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            //yield return new WaitUntil(() => _loadingScene.isDone);
            PlayerManager.Instance.gameObject.SetActive(true);
            PlayerManager.Instance.currentController.playerRunSpirits.ResetSpiritsPositions();
            for (int i = 0; i < PlayerManager.Instance.currentController.playerRunSpirits.spiritChain.Count; i++)
            {
                PlayerManager.Instance.currentController.playerRunSpirits.spiritChain[i].objectRenderer.SetActive(false);
            }
            transform.position = checkPoint.position;
            RunCamController.Instance.Set(CamState.PlayerScrolling, true);
            isDead = false;
            StartCoroutine(DisplayHp());
            //play respawn anim

        }

        IEnumerator DeathCoroutine()
        {
            //play defeat anim
            Instantiate(deathFx, transform.position, Quaternion.identity);
            objectRenderer.GetComponent<Animator>().Play("Anim_PlayerRun_death");
            AudioManager.Instance.PlaySoundEffect("PlayerHit");
            yield return new WaitForSeconds(1f);
            GameManagement.GameManager.Instance.gameState.Set(GameManagement.GameState.Hub, "Hub");
            isDead = false;
        }

        IEnumerator DisplayHp()
        {
            hpUi.transform.LeanScale(Vector3.one, 0.3f);
            hpText.text = currentHP.ToString();
            yield return new WaitForSeconds(2f);
            hpUi.transform.LeanScale(Vector3.zero, 0.1f);

        }
    }
}