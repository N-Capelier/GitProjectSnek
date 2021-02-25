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

        private void Start()
        {
            cooldownTimer = new Clock();
            InputHandler.InputReceived += HandleInput;
            cooldownTimer.ClockEnded += OnCooldownEnded;
        }

        void HandleInput(InputType inputType)
        {
            if (inputType == InputType.Tap && canAttack)
                Attack();
        }

        private void Attack()
        {
            canAttack = false;
            cooldownTimer.SetTime(attackCooldown);

            //attack
            print("Attack!");
        }

        void OnCooldownEnded()
        {
            canAttack = true;
        }
    }
}