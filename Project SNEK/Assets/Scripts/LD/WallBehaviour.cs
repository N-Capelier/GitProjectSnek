using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Wall 
{
    public class WallBehaviour : MonoBehaviour
    {
        [SerializeField] bool isDestroyable;
        [SerializeField] GameObject objectRenderer;
        [SerializeField] ParticleSystem fx;
        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.layer == LayerMask.NameToLayer("PlayerController"))
            {
                PlayerManager.Instance.currentController.Death();
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
                yield return new WaitForSeconds(fx.main.duration);
            }
            Destroy(gameObject);
        }
    }
}

