using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Enemy
{
    public class BulletBehaviour : MonoBehaviour
    {
        public int deathIndex;
        public float bulletLifetime;
        public bool isDestroyableByWall;
        bool destroyedByShield = false;

        [SerializeField] GameObject objectRenderer;
        [SerializeField] GameObject deathFx;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("PlayerController") && !PlayerManager.Instance.currentController.isInCutscene)
            {
                PlayerManager.Instance.currentController.Death(deathIndex);
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Shield"))
            {
                StartCoroutine(Destroyed());
            }

            if(isDestroyableByWall)
            {
                if(other.gameObject.layer == LayerMask.NameToLayer("Wall"))
                {
                    StartCoroutine(Destroyed());
                }
            }
        }

        private void Start()
        {
            StartCoroutine(DestroyedByTime());
        }

        IEnumerator DestroyedByTime()
        {
            yield return new WaitForSeconds(bulletLifetime);
            if(gameObject != null && destroyedByShield == false)
            {
                gameObject.GetComponent<BoxCollider>().enabled = false;
                objectRenderer.SetActive(false);

                DeathFx();
                Destroy(gameObject);
            }
        }

        public IEnumerator Destroyed()
        {
            destroyedByShield = true;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            objectRenderer.SetActive(false);
            yield return null;
            DeathFx();
            Destroy(gameObject);
        }

        void DeathFx()
        {
            Instantiate(deathFx, transform.position, Quaternion.identity);
        }
    }
}