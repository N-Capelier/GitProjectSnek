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
        [SerializeField] float damage, beamLifeTime;
        [SerializeField] GameObject swordRenderer;
        [SerializeField] GameObject particleFx, particleFxPrefab, explosionFx;
        bool hasExploded;

        private void Start()
        {
            particleFx = Instantiate(particleFxPrefab, transform.position, Quaternion.identity);
            StartCoroutine(BeamLifetime());
        }

        private void FixedUpdate()
        {
            if(particleFx != null && hasExploded == false)
            {
                particleFx.transform.position = transform.position;
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                other.GetComponent<EnemyStats>().TakeDamage(damage);
            else if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                StartCoroutine(DestroyBeam());
            }
        }
        IEnumerator BeamLifetime()
        {
            yield return new WaitForSeconds(beamLifeTime - 0.416f);
            if (gameObject != null)
            {
                gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                if(hasExploded == false)
                StartCoroutine(DestroyBeam());
            }
        }

        public IEnumerator DestroyBeam()
        {
            hasExploded = true;
            swordRenderer.transform.GetComponentInChildren<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Collider>().enabled = false;
            Instantiate(explosionFx, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
            if(gameObject != null)
            Destroy(gameObject);
        }
    }
}

