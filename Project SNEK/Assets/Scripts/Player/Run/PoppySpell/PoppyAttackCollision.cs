using UnityEngine;
using Enemy;


namespace Player.Spells
{
    public class PoppyAttackCollision : MonoBehaviour
    {
        public float explosionDamage;

        public void OnTriggerEnter(Collider enemy)
        {
            if (enemy.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                enemy.GetComponent<EnemyStats>().TakeDamage(explosionDamage);
        }
    }
}

