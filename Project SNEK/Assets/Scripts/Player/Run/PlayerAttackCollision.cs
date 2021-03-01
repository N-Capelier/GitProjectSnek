using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Attack
{
    /// <summary>
    /// Coco
    /// </summary>
    public class PlayerAttackCollision : MonoBehaviour
    {
        private void OnTriggerEnter(Collider enemy)
        {
            if (enemy.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                enemy.GetComponent<TestStat>().TakeDamage(PlayerManager.Instance.currentController.playerRunAttack.attackDamages);
            }
        }
    }
}

