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
        [SerializeField] [Range(0f, 1f)] float comboCooldown = 0.05f; 
        [SerializeField] [Range(0, 100)] public float attackDamages = 10; // A REFERENCER

        [HideInInspector] int combo = 3; // A REFERENCER
        [HideInInspector] int comboMeter = 1;
        [HideInInspector] float rangeBonus = 1.40f;// A REFERENCER
        [HideInInspector] float rangeBonusOffSet = 1.2f;
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
        void HandleInput(InputType inputType)
        {
            if (inputType == InputType.Tap && canAttack && comboMeter > 0)
                StartCoroutine(Attack());
        }
        private IEnumerator Attack()
        {

            PlayerManager.Instance.currentController.canMove = false;
            if(combo > 0)
            cooldownTimer.SetTime(attackCooldown);
            else if(combo <= 0)
            {
                canAttack = false;
                cooldownTimer.SetTime(comboCooldown);
            }
            //attack
            GameObject attack = Instantiate(attackCollision, transform.position, Quaternion.identity);
            //Play Attack animation

            if (comboMeter > 0)
            {
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

                comboMeter -= 1;
                Debug.Log("Combo equal " + comboMeter);
  
            }
            yield return new WaitForSeconds(0.05f);
            Destroy(attack);
            yield return new WaitForSeconds(0.4f);
            comboMeter = combo;
            PlayerManager.Instance.currentController.canMove = true;
        }
        void OnCooldownEnded()
        {
            canAttack = true;
        }
    }
}