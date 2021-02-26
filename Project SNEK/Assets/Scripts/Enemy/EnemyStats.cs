using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Controller;

namespace Enemy
{
    public class EnemyStats : MonoBehaviour
    {
        [HideInInspector] public Clock attackClock;
        [HideInInspector] public Clock movementClock;

        [SerializeField] [Range(0, 10)] int attackCooldown = 5;
        [SerializeField] [Range(0, 10)] int movementCooldown = 2;

        [Space]
        [SerializeField] [Range(0, 10)] int maxHp = 1;
        [HideInInspector] public int currentHp;

        [Space]
        [HideInInspector] public Rigidbody rb = null;
        [Range(0, 5)] public int moveSpeed = 1;

        [Space]
        [SerializeField] [Range(0, 5)] int spiritLoot = 1;

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

        void onTimerAttackEnd()
        {
            attackClock.SetTime(attackCooldown);
        }

        void onTimerMovementEnd()
        {
            movementClock.SetTime(movementCooldown);
        }

        private void OnDestroy()
        {
            //Instantie Esprit( x SpiritLoot)
            attackClock.ClockEnded -= onTimerAttackEnd;
            movementClock.ClockEnded -= onTimerMovementEnd;
        }
    }    
}

