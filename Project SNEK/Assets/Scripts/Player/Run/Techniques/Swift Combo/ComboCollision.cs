using Enemy;
using UnityEngine;

namespace Player.Attack
{   
    public class ComboCollision : MonoBehaviour
    {
        public float damageModifier;

        public void OnTriggerEnter(Collider enemy)
        {
            if (enemy.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                enemy.GetComponent<EnemyStats>().TakeDamage(PlayerManager.Instance.currentController.playerRunAttack.attackDamages * damageModifier);
            }
        }
    }
}

