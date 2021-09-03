using UnityEngine;
using Enemy;
using AudioManagement;
using Boss;

namespace Player.Attack
{
    /// <summary>
    /// Coco
    /// </summary>
    public class PlayerAttackCollision : MonoBehaviour
    {
        public void OnTriggerEnter(Collider enemy)
        {
            if (enemy.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if(enemy != null)
                {
                    enemy.GetComponent<EnemyStats>().TakeDamage(PlayerManager.Instance.currentController.playerRunAttack.attackDamages);
                    AudioManager.Instance.PlaySoundEffect("PlayerSwordImpact");
                }
            }

            if (enemy.gameObject.layer == LayerMask.NameToLayer("Boss"))
            {
                enemy.GetComponent<TestAnorexia>().TakeDamage(PlayerManager.Instance.currentController.playerRunAttack.attackDamages);
                AudioManager.Instance.PlaySoundEffect("PlayerSwordImpact");
            }

            if (enemy.gameObject.layer == LayerMask.NameToLayer("BossParanoia"))
            {
                enemy.GetComponent<BossParanoia>().TakeDamage(PlayerManager.Instance.currentController.playerRunAttack.attackDamages);
                AudioManager.Instance.PlaySoundEffect("PlayerSwordImpact");
            }

            if (enemy.gameObject.layer == LayerMask.NameToLayer("BossDepression"))
            {
                enemy.GetComponent<BossDepression>().TakeDamage(PlayerManager.Instance.currentController.playerRunAttack.attackDamages);
                AudioManager.Instance.PlaySoundEffect("PlayerSwordImpact");
            }
        }
    }
}

