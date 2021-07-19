using System.Collections;
using UnityEngine;
using Player;
using AudioManagement;

namespace Wall 
{
    /// <summary>
    /// Corentin
    /// </summary>
    public class WallBehaviour : MonoBehaviour
    {
        [SerializeField] public bool isDestroyable;
        [SerializeField] GameObject objectRenderer;
        [SerializeField] ParticleSystem fx;
        [SerializeField] string soundName;

        public int deathIndex;
        protected virtual void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.layer == LayerMask.NameToLayer("PlayerController"))
            {
                PlayerManager.Instance.currentController.Death(deathIndex);
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Attack") && isDestroyable == true)
            {
                StartCoroutine(GetDestroyed());
            }
        }

        public IEnumerator GetDestroyed()
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            objectRenderer.SetActive(false);
            if(fx != null)
            {
                fx.Play();
                AudioManager.Instance.PlaySoundEffect(soundName);
                yield return new WaitForSeconds(fx.main.duration);
            }
            Destroy(gameObject);
        }
    }
}