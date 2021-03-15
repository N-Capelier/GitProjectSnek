using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Spells
{
    /// <summary>
    /// Coco
    /// </summary>
    public class PoppyBomb : MonoBehaviour
    {
        bool hasStarted = false;
        Rigidbody rb;
        void Start()
        {
            rb = gameObject.GetComponent<Rigidbody>();
            StartCoroutine(StartBehaviour());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Wall") || other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                if(hasStarted == false)
                StartCoroutine(Absorb());
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
            transform.position = new Vector3(
            Mathf.RoundToInt(transform.position.x),
            transform.position.y,
            Mathf.RoundToInt(transform.position.z)
            );
            yield return null;
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}

