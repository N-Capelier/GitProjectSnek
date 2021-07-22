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

        bool destroyedByShield = false;

        [SerializeField] GameObject objectRenderer;
        [SerializeField] ParticleSystem fx;
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("PlayerController"))
            {
                PlayerManager.Instance.currentController.Death(deathIndex);
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Wall") || other.gameObject.layer == LayerMask.NameToLayer("Shield"))
            {
                StartCoroutine(Destroyed());
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
                if (fx != null)
                {
                    fx.Play();
                    //AudioManager.Instance.PlaySoundEffect(soundName);
                    yield return new WaitForSeconds(fx.main.duration);
                }
                Destroy(gameObject);
            }
        }

        public IEnumerator Destroyed()
        {
            destroyedByShield = true;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            objectRenderer.SetActive(false);
            if (fx != null)
            {
                fx.Play();
                //AudioManager.Instance.PlaySoundEffect(soundName);
                yield return new WaitForSeconds(fx.main.duration);
            }
            Destroy(gameObject);
        }
    }
}

