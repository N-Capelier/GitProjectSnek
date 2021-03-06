using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using Wall;

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
                enemy.GetComponent<EnemyStats>().TakeDamage(PlayerManager.Instance.currentController.playerRunAttack.attackDamages);
            }
            /*else if(enemy.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                if(enemy.GetComponent<WallBehaviour>().isDestroyable == true)
                {
                    enemy.GetComponent<WallBehaviour>().GetDestroyed();
                }
            }*/
        }
    }
}

