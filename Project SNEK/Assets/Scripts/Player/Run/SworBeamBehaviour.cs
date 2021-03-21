using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using Wall;

namespace Player.Technique
{
    /// <summary>
    /// Coco
    /// </summary>
    public class SworBeamBehaviour : MonoBehaviour
    {
        [SerializeField] float damage;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                other.GetComponent<EnemyStats>().TakeDamage(damage);
            else if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                StartCoroutine(DestroyBeam());
            }
        }
        public IEnumerator DestroyBeam()
        {
            yield return new WaitForSeconds(0);
            Destroy(gameObject);
        }
    }
}

