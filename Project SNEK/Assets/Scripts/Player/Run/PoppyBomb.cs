using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wall;

namespace Player.Spells
{
    /// <summary>
    /// Coco
    /// </summary>
    public class PoppyBomb : MonoBehaviour
    {
        [SerializeField] float absorbRadius;
        bool hasStarted = false;
        Rigidbody rb;
        public CapsuleCollider capCollider;
        public BoxCollider boxCollider;
        void Start()
        {
            rb = gameObject.GetComponent<Rigidbody>();
            StartCoroutine(StartBehaviour());
            capCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (hasStarted == false)
                if (other.gameObject.layer == LayerMask.NameToLayer("Wall") || other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    StartCoroutine(Absorb());
                }

        }

        private void OnTriggerStay(Collider other)
        {
                if (hasStarted == true)
            {
                if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    other.gameObject.SetActive(false);
                }
                else if (other.gameObject.GetComponent<WallBehaviour>() != null)
                {
                    if(other.gameObject.GetComponent<WallBehaviour>().isDestroyable == true)
                    other.gameObject.SetActive(false);
                }
                else if (other.gameObject.GetComponent<VioletBehaviour>() != null)
                {
                    other.gameObject.SetActive(false);
                }
            }

        }

        IEnumerator StartBehaviour()
        {
            yield return new WaitForSeconds(0.4f);
            if(hasStarted == false)
            StartCoroutine(Absorb());
        }

        IEnumerator Absorb()
        {
            hasStarted = true;
            rb.velocity = Vector3.zero;
            boxCollider.enabled = false;
            transform.position = new Vector3(
            Mathf.RoundToInt(transform.position.x),
            transform.position.y,
            Mathf.RoundToInt(transform.position.z)
            );
            //Start absorption after anim
            yield return new WaitForSeconds(0.2f);
            capCollider.enabled = true;
            yield return null;
        }
    }
}

