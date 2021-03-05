using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagement;
using System;

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
            GameObject attack = Instantiate(attackCollision, transform.position, Quaternion.identity);
            //Play Attack animation
                switch (PlayerManager.Instance.currentController.currentDirection)
                {
                    // Ajouter un * par rapport à la range
                    case Controller.PlayerDirection.Up:
                        attack.transform.localScale = new Vector3(3 * rangeBonus, 1, 2 * rangeBonus);
                        attack.transform.position = transform.position + new Vector3(0, 0, 0.5f * rangeBonus * rangeBonusOffSet);
                        //attack.GetComponent<BoxCollider>().size = new Vector3(3 * rangeBonus, 1, 2 * rangeBonus);
                        //attack.GetComponent<BoxCollider>().center = new Vector3(0, 0, 0.5f);
                        break;
                    case Controller.PlayerDirection.Down:
                        attack.transform.localScale = new Vector3(3 * rangeBonus, 1, 2 * rangeBonus);
                        attack.transform.position = transform.position + new Vector3(0, 0, -0.5f * rangeBonus * rangeBonusOffSet);
                        //attack.GetComponent<BoxCollider>().size = new Vector3(3 * rangeBonus, 1, 2 * rangeBonus);
                        //attack.GetComponent<BoxCollider>().center = new Vector3(0, 0, -0.5f);
                        break;
                    case Controller.PlayerDirection.Left:
                        attack.transform.localScale = new Vector3(2 * rangeBonus, 1, 3 * rangeBonus);
                        attack.transform.position = transform.position + new Vector3(-0.5f * rangeBonus * rangeBonusOffSet, 0, 0);
                        //attack.GetComponent<BoxCollider>().size = new Vector3(2 * rangeBonus, 1, 3 * rangeBonus);
                        //attack.GetComponent<BoxCollider>().center = new Vector3(-0.5f, 0, 0);
                        break;
                    case Controller.PlayerDirection.Right:
                        attack.transform.localScale = new Vector3(2 * rangeBonus, 1, 3 * rangeBonus);
                        attack.transform.position = transform.position + new Vector3(0.5f * rangeBonus * rangeBonusOffSet, 0, 0);
                        //attack.GetComponent<BoxCollider>().size = new Vector3(2 * rangeBonus, 1, 3 * rangeBonus);
                        //attack.GetComponent<BoxCollider>().center = new Vector3(0.5f, 0, 0);
                        break;
                }
            yield return new WaitForSeconds(0.05f);
            Destroy(attack);
            yield return new WaitForSeconds(attackCooldown * 0.4f);
            PlayerManager.Instance.currentController.canMove = true;
            yield return new WaitForSeconds(attackCooldown * 0.6f);
            canAttack = true;
        }
        void OnCooldownEnded()
        {
            canAttack = true;
        }
    }
}