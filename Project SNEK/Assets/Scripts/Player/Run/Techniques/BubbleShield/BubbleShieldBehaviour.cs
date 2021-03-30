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
        public ParticleSystem shieldFx, shieldDeathFx;

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
            gameObject.GetComponent<Collider>().enabled = false;
            Instantiate(shieldDeathFx, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.3f);
            Destroy(gameObject);
        }
    }
}


