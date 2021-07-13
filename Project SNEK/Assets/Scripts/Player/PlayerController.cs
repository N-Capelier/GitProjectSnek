using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Player.Attack;
using Player.Spirits;
using Rendering.Run;
using AudioManagement;
using CoinUI;
using Player.Spells;

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
        [HideInInspector] public PlayerSpell playerRunSpell;
        public SpiritManager playerRunSpirits;

        public delegate void PlayerDeath();
        public static event PlayerDeath PlayerDead;

        public Vector3 startingNode = new Vector3(5, 0, 0);
        [HideInInspector] public PlayerDirection currentDirection;
        [HideInInspector] public PlayerDirection nextDirection;

        [Space]
        [HideInInspector] public Rigidbody rb = null;
        [HideInInspector] public bool isInCutscene = false;
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
        public SkinnedMeshRenderer faceRenderer;
        public GameObject deathFx;
        public Animator animator;

        [SerializeField] Material[] faces;

        [SerializeField] GameObject hpUi;
        [SerializeField] TextMeshProUGUI hpText;
        [SerializeField] GameObject spiritChainPrefab;

        public Animator coinAnimator;
        public CoinCountUI coinUI;

        public virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public IEnumerator Init(int _bonusHP, float _bonusRange, int _bonusPower)
        {
            yield return new WaitForEndOfFrame();
            currentHP = baseHP + _bonusHP;
            playerRunAttack.rangeBonus = _bonusRange;
            playerRunAttack.rangeBonusOffSet = _bonusRange * 0.5f;
            //playerRunAttack.attackDamages += _bonusPower;
        }

        public void Death(int deathIndex, int _spiritCount = 0)
        {
            if (isDead)
                return;
            if (SceneManager.GetActiveScene().name != "TutorialMap" && !isInCutscene)
            {
                currentHP--;
            }
            PlayerDead?.Invoke();
            isDead = true;
            playerRunAttack.canAttack = true;           ////// ?????????????? J'ai vraiment écrit ça moi ? Nico
            if (currentHP <= 0)
            {
                StartCoroutine(DeathCoroutine(deathIndex));
            }
            else
            {
                StartCoroutine(RespawnCoroutine(deathIndex, _spiritCount));
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

        IEnumerator RespawnCoroutine(int deathAnimIndex, int _spiritCount = 0)
        {
            //death fx
            Instantiate(deathFx, transform.position, Quaternion.identity);

            //death face
            faceRenderer.material = faces[1];

            //death anim
            if (deathAnimIndex == 0)
                objectRenderer.GetComponent<Animator>().Play("Anim_PlayerRun_death");
            else if (deathAnimIndex == 1)
                objectRenderer.GetComponent<Animator>().Play("Anim_PlayerRun_deathPoison");

            //death sound
            AudioManager.Instance.PlaySoundEffect("PlayerHit");

            //Wait until death anim ends
            yield return new WaitForSeconds(1.5f);

            //deactivate player during scene reload
            PlayerManager.Instance.gameObject.SetActive(false);      
            
            // Reload scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            
            //reactivate player
            PlayerManager.Instance.gameObject.SetActive(true);

            //Align spirits
            PlayerManager.Instance.currentController.playerRunSpirits.ResetSpiritsPositions();

            //deactivate all spirit renderers
            for (int i = 0; i < playerRunSpirits.spiritChain.Count; i++)
            {
                playerRunSpirits.spiritChain[i].objectRenderer.SetActive(false);
            }

            if(_spiritCount > 0)
            {
                for (int i = 0; i < _spiritCount; i++)
                {
                    playerRunSpirits.AddSpirit();
                }
            }

            //Teleports player to checkpoint
            transform.position = checkPoint.position;

            //alive face
            faceRenderer.material = faces[0];

            //Camera reset
            RunCamController.Instance.Set(CamState.PlayerScrolling, true);

            //Leave death "state"
            isDead = false;

            //Display hp
            StartCoroutine(DisplayHp());
        }

        IEnumerator DeathCoroutine(int deathAnimIndex)
        {
            //play defeat anim
            faceRenderer.material = faces[1];
            Instantiate(deathFx, transform.position, Quaternion.identity);
            if(deathAnimIndex == 0)
            objectRenderer.GetComponent<Animator>().Play("Anim_PlayerRun_death");
            else if(deathAnimIndex == 1)
            objectRenderer.GetComponent<Animator>().Play("Anim_PlayerRun_deathPoison");
            AudioManager.Instance.PlaySoundEffect("PlayerHit");
            yield return new WaitForSeconds(1f);
            faceRenderer.material = faces[0];
            //GameManagement.GameManager.Instance.gameState.Set(GameManagement.GameState.Hub, "Hub");
            GameOverMenu.Instance.menuCanvas.SetActive(true);
            isDead = false;
        }

        bool bossLevel = false;
        public void RespawnAfterCutscene(int _spiritCount)
        {
            if (SceneManager.GetActiveScene().name != "Boss Anorexia")
            {
                PlayerManager.Instance.gameObject.SetActive(false);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                PlayerManager.Instance.gameObject.SetActive(true);
            }

            playerRunSpirits.ResetSpiritsPositions();
            if (_spiritCount > 0)
            {
                for (int i = 0; i < _spiritCount; i++)
                {
                    playerRunSpirits.AddSpirit();
                }
            }

            transform.position = checkPoint.position;

            RunCamController.Instance.Set(CamState.PlayerScrolling, true);

            //Destroy(playerRunSpirits.gameObject);
            //GameObject _newSpiritChain = Instantiate(spiritChainPrefab, transform);
            //playerRunSpirits = _newSpiritChain.GetComponent<SpiritManager>();

            isInCutscene = false;
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