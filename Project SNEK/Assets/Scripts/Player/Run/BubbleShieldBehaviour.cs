using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;

namespace Player.Technique
{
    /// <summary>
    /// Coco
    /// </summary>
    public class BubbleShieldBehaviour : MonoBehaviour
    {
        [SerializeField]float coolDownBtwHit, damage;
        public int count;
        bool canHit = true;
        public BubbleShieldTechnique shieldTechnique;
        public ParticleSystem shieldFx;

        private void Start()
        {
            shieldFx.Play();
        }
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if (canHit == true)
                {
                    canHit = false;
                    other.GetComponent<EnemyStats>().TakeDamage(damage); // Rajouter le multiplicateur
                    StartCoroutine(countUp());
                }

            }
        }

        IEnumerator countUp()
        {
            count++;
            if (count >= 2)
            {
                StartCoroutine(DestroyShield());
                yield break;
            }
            yield return new WaitForSeconds(coolDownBtwHit);
            canHit = true;

        }

        public IEnumerator DestroyShield()
        {
            shieldFx.Stop();
            yield return new WaitForSeconds(0.2f);
            Destroy(gameObject);
        }
    }
}


