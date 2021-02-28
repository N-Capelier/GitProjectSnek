using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider enemy)
    {
        if (enemy.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            enemy.GetComponent<TestStat>().TakeDamage(1);
        }
    }
}
