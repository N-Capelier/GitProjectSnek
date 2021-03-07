using System.Collections;
using UnityEngine;
using GameManagement;

namespace Player.Attack
{
    /// <summary>
    /// Nico
    /// </summary>
    public class PlayerRunAttack : MonoBehaviour
    {
        [SerializeField] [Range(0f, 100f)] float attackCooldown = 1f;
        [SerializeField] [Range(0, 100)] public float attackDamages = 10; // A REFERENCER
        [HideInInspector] float rangeBonus = 1f;// A REFERENCER
        [HideInInspector] float rangeBonusOffSet = 1f;// A REFERENCER
        bool canAttack = true;
        Clock cooldownTimer;
        public GameObject attackCollision;
        [SerializeField] GameObject attackFx;
        [SerializeField] float fxOffSet = 0.15f;

        private void Start()
        {
            PlayerManager.Instance.currentController.playerRunAttack = this;
            cooldownTimer = new Clock();
            InputHandler.InputReceived += HandleInput;
            cooldownTimer.ClockEnded += OnCooldownEnded;
        }

        private void OnDestroy()
        {
            cooldownTimer.ClockEnded -= OnCooldownEnded;
            InputHandler.InputReceived -= HandleInput;
        }

        void HandleInput(InputType inputType)
        {
            if (inputType == InputType.Tap && canAttack)
                StartCoroutine(Attack());
        }
        private IEnumerator Attack()
        {

            PlayerManager.Instance.currentController.canMove = false;
            canAttack = false;
            //attack

            //Faire un switch à l'instantiation
            PlayerManager.Instance.currentController.objectRenderer.GetComponent<Animator>().Play("Anim_PlayerRun_attack");
            yield return new WaitForSeconds(0.04f);
            GameObject slashFx = Instantiate(attackFx, gameObject.transform.GetChild(0).gameObject.transform.position, Quaternion.identity);
            slashFx.gameObject.transform.localScale = new Vector3(rangeBonus + 0.2f, 1, rangeBonus + 0.2f);
            switch (PlayerManager.Instance.currentController.currentDirection)
            {
                case Controller.PlayerDirection.Up:
                    slashFx.transform.Rotate(new Vector3(90, 0, 0));
                    slashFx.transform.position += new Vector3(0,0,-fxOffSet);
                    break;  
                case Controller.PlayerDirection.Down:
                    slashFx.transform.Rotate(new Vector3(90, 0, 180));
                    slashFx.transform.position += new Vector3(0,0,fxOffSet) ;
                    break;       
                case Controller.PlayerDirection.Left:
                    slashFx.transform.Rotate(new Vector3(90, 0, 90));
                    slashFx.transform.position += new Vector3(fxOffSet,0,0);
                    break;  
                case Controller.PlayerDirection.Right:
                    slashFx.transform.Rotate(new Vector3(90, 0, 270));
                    slashFx.transform.position += new Vector3(-fxOffSet,0,0);
                    break;
            }
            yield return new WaitForSeconds(0.1f);
            GameObject attack = Instantiate(attackCollision, transform.position, Quaternion.identity);
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
            yield return new WaitForSeconds(0.05f);
            Destroy(attack);
            yield return new WaitForSeconds(attackCooldown * 0.4f);
            PlayerManager.Instance.currentController.canMove = true;
            Destroy(slashFx);
            yield return new WaitForSeconds(attackCooldown * 0.6f);
            canAttack = true;
        }
        void OnCooldownEnded()
        {
            canAttack = true;
        }
    }
}