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
        [SerializeField] [Range(0, 100)] int attackDamages = 10;

        bool canAttack = true;
        Clock cooldownTimer;
        public GameObject attackCollision;

        private void Start()
        {
            cooldownTimer = new Clock();
            InputHandler.InputReceived += HandleInput;
            cooldownTimer.ClockEnded += OnCooldownEnded;
        }

        void HandleInput(InputType inputType)
        {
            if (inputType == InputType.Tap && canAttack)
                StartCoroutine(Attack());
        }

        private IEnumerator Attack()
        {
            canAttack = false;
            PlayerManager.Instance.currentController.canMove = false;
            cooldownTimer.SetTime(attackCooldown);

            //attack
            GameObject attack = Instantiate(attackCollision, transform.position, Quaternion.identity);

            switch (PlayerManager.Instance.currentController.currentDirection)
            {
                // Ajouter un * par rapport à la range
                case Controller.PlayerDirection.Up:
                    attack.GetComponent<BoxCollider>().size = new Vector3(3, 1, 2);
                    attack.GetComponent<BoxCollider>().center = new Vector3(0, 0, 0.5f);
                    break;
                case Controller.PlayerDirection.Down:
                    attack.GetComponent<BoxCollider>().size = new Vector3(3, 1, 2);
                    attack.GetComponent<BoxCollider>().center = new Vector3(0, 0, -0.5f);
                    break;
                case Controller.PlayerDirection.Left:
                    attack.GetComponent<BoxCollider>().size = new Vector3(2, 1, 3);
                    attack.GetComponent<BoxCollider>().center = new Vector3(-0.5f, 0, 0);
                    break;
                case Controller.PlayerDirection.Right:
                    attack.GetComponent<BoxCollider>().size = new Vector3(2, 1, 3);
                    attack.GetComponent<BoxCollider>().center = new Vector3(0.5f, 0, 0);
                    break;
            }
            yield return new WaitForSeconds(0.3f);
            PlayerManager.Instance.currentController.canMove = true;
            Destroy(attack);

        }

        void SetAttackDir()
        {

        }

        void OnCooldownEnded()
        {
            canAttack = true;
        }
    }
}