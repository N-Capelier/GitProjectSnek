using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameManagement;
using Player.Controller;
using AudioManagement;


namespace Player.Attack
{
    /// <summary>
    /// Nico
    /// </summary>
    public class PlayerRunAttack : MonoBehaviour
    {
        [Header("Base Attack")]
        [SerializeField] [Range(0f, 100f)] float attackCooldown = 1f;
        [Range(0, 100)] public float attackDamages = 10; // A REFERENCER
        [HideInInspector] public float rangeBonus = 1f;// A REFERENCER
        [HideInInspector] public float rangeBonusOffSet = 1f;// A REFERENCER
        [HideInInspector] public bool canAttack = true;
        [HideInInspector] public bool isAttacking = false;
        [SerializeField] [Range(0f, 1f)] float moveSpeedDuringAttack = 0.2f;
        Clock cooldownTimer;
        public GameObject attackCollision;
        [SerializeField] GameObject attackFx;
        [SerializeField] float fxOffSet = 0.15f;
        [Space(20)]
        [Header("Beam Attack")]
        [SerializeField] [Range(0f, 100f)] float beamCooldown = 1f;
        [SerializeField] float beamSpeed;
        public GameObject beamPrefab;
        GameObject beam;
        bool beamIsUp = true;

        public GameObject swordObject;

        private void Start()
        {
            PlayerManager.Instance.currentController.playerRunAttack = this;
            canAttack = true;
            cooldownTimer = new Clock();
            InputHandler.InputReceived += HandleInput;
            cooldownTimer.ClockEnded += OnCooldownEnded;
            PlayerController.PlayerDead += OnDeath;
            PlayerRunController.PlayerChangedDirection += OnChangeDirection;

            if(SceneManager.GetActiveScene().name == "TutorialMap")
            {
                canAttack = false;
                swordObject.SetActive(false);
                cooldownTimer.ClockEnded -= OnCooldownEnded;
            }
        }

        private void Update()
        {
            if (SceneManager.GetActiveScene().name == "TutorialMap" && canAttack == true)
            {
                canAttack = false;
                swordObject.SetActive(false);
                cooldownTimer.ClockEnded -= OnCooldownEnded;
            }
        }

        private void OnDestroy()
        {
            cooldownTimer.ClockEnded -= OnCooldownEnded;
            InputHandler.InputReceived -= HandleInput;
            PlayerController.PlayerDead -= OnDeath;
            PlayerRunController.PlayerChangedDirection -= OnChangeDirection;
        }

        Coroutine attackCoroutine;

        void HandleInput(InputType inputType)
        {
            if (PlayerManager.Instance.currentController.isInCutscene)
            {
                return;
            }
            if (PlayerManager.Instance.currentController.isDead)
                return;
            if (inputType == InputType.Tap && canAttack)
                attackCoroutine = StartCoroutine(Attack());
        }

        GameObject slashFx, attack;

        private IEnumerator Attack()
        {
            //PlayerManager.Instance.currentController.canMove = false;
            PlayerManager.Instance.currentController.attackMoveSpeedModifier = moveSpeedDuringAttack;
            //PlayerManager.Instance.currentController.playerRunSpirits.UpdateSpiritsVelocity();
            PlayerManager.Instance.currentController.rb.velocity = PlayerManager.Instance.currentController.rb.velocity * PlayerManager.Instance.currentController.attackMoveSpeedModifier;
            canAttack = false;
            isAttacking = true;
            //attack

            //Faire un switch à l'instantiation
            PlayerManager.Instance.currentController.objectRenderer.GetComponent<Animator>().Play("Anim_PlayerRun_attack");
            slashFx = Instantiate(attackFx, gameObject.transform.GetChild(0).gameObject.transform.position, Quaternion.identity);
            slashFx.gameObject.transform.localScale = new Vector3(rangeBonus + 0.2f, 1, rangeBonus + 0.2f);
            AudioManager.Instance.PlaySoundEffect("PlayerAttack01");
            switch (PlayerManager.Instance.currentController.currentDirection)
            {
                case PlayerDirection.Up:
                    slashFx.transform.Rotate(new Vector3(90, 0, 0));
                    slashFx.transform.position += new Vector3(0, 0, -fxOffSet);
                    break;
                case PlayerDirection.Down:
                    slashFx.transform.Rotate(new Vector3(90, 0, 180));
                    slashFx.transform.position += new Vector3(0, 0, fxOffSet);
                    break;
                case PlayerDirection.Left:
                    slashFx.transform.Rotate(new Vector3(90, 0, 90));
                    slashFx.transform.position += new Vector3(fxOffSet, 0, 0);
                    break;
                case PlayerDirection.Right:
                    slashFx.transform.Rotate(new Vector3(90, 0, 270));
                    slashFx.transform.position += new Vector3(-fxOffSet, 0, 0);
                    break;
            }
            yield return new WaitForSeconds(0.1f);
            attack = Instantiate(attackCollision, transform.position, Quaternion.identity);

            switch (PlayerManager.Instance.currentController.currentDirection)
            {
                // Ajouter un * par rapport à la range
                case Controller.PlayerDirection.Up:
                    attack.transform.localScale = new Vector3(2.9f * rangeBonus, 1, 2 * rangeBonus);
                    attack.transform.position = transform.position + new Vector3(0, 0, 0.5f * rangeBonus * rangeBonusOffSet);
                    //attack.GetComponent<BoxCollider>().size = new Vector3(3 * rangeBonus, 1, 2 * rangeBonus);
                    //attack.GetComponent<BoxCollider>().center = new Vector3(0, 0, 0.5f);
                    break;
                case Controller.PlayerDirection.Down:
                    attack.transform.localScale = new Vector3(2.9f * rangeBonus, 1, 2 * rangeBonus);
                    attack.transform.position = transform.position + new Vector3(0, 0, -0.5f * rangeBonus * rangeBonusOffSet);
                    //attack.GetComponent<BoxCollider>().size = new Vector3(3 * rangeBonus, 1, 2 * rangeBonus);
                    //attack.GetComponent<BoxCollider>().center = new Vector3(0, 0, -0.5f);
                    break;
                case Controller.PlayerDirection.Left:
                    attack.transform.localScale = new Vector3(2 * rangeBonus, 1, 2.9f * rangeBonus);
                    attack.transform.position = transform.position + new Vector3(-0.5f * rangeBonus * rangeBonusOffSet, 0, 0);
                    //attack.GetComponent<BoxCollider>().size = new Vector3(2 * rangeBonus, 1, 3 * rangeBonus);
                    //attack.GetComponent<BoxCollider>().center = new Vector3(-0.5f, 0, 0);
                    break;
                case Controller.PlayerDirection.Right:
                    attack.transform.localScale = new Vector3(2 * rangeBonus, 1, 2.9f * rangeBonus);
                    attack.transform.position = transform.position + new Vector3(0.5f * rangeBonus * rangeBonusOffSet, 0, 0);
                    //attack.GetComponent<BoxCollider>().size = new Vector3(2 * rangeBonus, 1, 3 * rangeBonus);
                    //attack.GetComponent<BoxCollider>().center = new Vector3(0.5f, 0, 0);
                    break;
            }
            if(PlayerManager.Instance.currentController.playerRunSpirits.GetActiveSpirits() >= 5 && beamIsUp == true)
            {
                StartCoroutine(BeamAttack());
            }
            yield return new WaitForSeconds(0.3f);
            yield return new WaitForFixedUpdate();
            Destroy(attack);
            yield return new WaitForSeconds(attackCooldown * 0.4f);
            isAttacking = false;
            //PlayerManager.Instance.currentController.canMove = true;
            PlayerManager.Instance.currentController.attackMoveSpeedModifier = 1f;
            //PlayerManager.Instance.currentController.playerRunSpirits.UpdateSpiritsVelocity();
            PlayerManager.Instance.currentController.rb.velocity = PlayerManager.Instance.currentController.rb.velocity * PlayerManager.Instance.currentController.attackMoveSpeedModifier;
            Destroy(slashFx);

            yield return new WaitForSeconds(attackCooldown * 0.6f);
            canAttack = true;
        }

        IEnumerator BeamAttack()
        {
            beamIsUp = false;
            beam = Instantiate(beamPrefab, transform.position, Quaternion.identity);
            AudioManager.Instance.PlaySoundEffect("SwordBeam");
            switch (PlayerManager.Instance.currentController.currentDirection)
            {
                case Controller.PlayerDirection.Up:
                    beam.GetComponent<Rigidbody>().velocity = Vector3.forward * beamSpeed;
                    beam.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                    break;
                case Controller.PlayerDirection.Down:
                    beam.GetComponent<Rigidbody>().velocity = Vector3.back * beamSpeed;
                    beam.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                    break;
                case Controller.PlayerDirection.Right:
                    beam.GetComponent<Rigidbody>().velocity = Vector3.right * beamSpeed;
                    beam.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                    break;
                case Controller.PlayerDirection.Left:
                    beam.GetComponent<Rigidbody>().velocity = Vector3.left * beamSpeed;
                    beam.transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
                    break;
            }
            yield return new WaitForSeconds(beamCooldown);
            beamIsUp = true;
        }
        void OnCooldownEnded()
        {
            canAttack = true;
        }

        void OnChangeDirection(PlayerDirection _dir)
        {
            if (attackCoroutine == null)
                return;

            StopCoroutine(attackCoroutine);

            if (slashFx != null)
                Destroy(slashFx);

            if (attack != null)
                Destroy(attack);

            PlayerManager.Instance.currentController.attackMoveSpeedModifier = 1f;
            //PlayerManager.Instance.currentController.playerRunSpirits.UpdateSpiritsVelocity();
            PlayerManager.Instance.currentController.rb.velocity = PlayerManager.Instance.currentController.rb.velocity * PlayerManager.Instance.currentController.attackMoveSpeedModifier;

            canAttack = true;
        }

        void OnDeath()
        {
            canAttack = true;
            PlayerManager.Instance.currentController.attackMoveSpeedModifier = 1f;
            //PlayerManager.Instance.currentController.playerRunSpirits.UpdateSpiritsVelocity();
        }
    }
}