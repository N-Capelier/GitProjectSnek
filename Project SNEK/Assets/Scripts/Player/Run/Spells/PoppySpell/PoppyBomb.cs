using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wall;
using AudioManagement;

namespace Player.Spells
{
    /// <summary>
    /// Coco
    /// </summary>
    public class PoppyBomb : MonoBehaviour
    {
        [SerializeField] float absorbSpeed,timeBeforeIgnition, timeBeforeStart;
        bool hasStarted = false;
        bool ignited = false;
        Rigidbody rb;
        public CapsuleCollider capCollider;
        public BoxCollider boxCollider;
        List<GameObject> grabbedObjects = new List<GameObject>();
        public GameObject explosionBox;
        public Animator animator;
        public ParticleSystem absorbFx, explosionFx, deathFx;

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
                    StartCoroutine(Absorb(true));
                }
        }

        private void OnTriggerStay(Collider other)
        {
            if (hasStarted == true && ignited == false)
                if (!grabbedObjects.Contains(other.gameObject))
                {
                    grabbedObjects.Add(other.gameObject);
                    if (other.gameObject.GetComponent<EntityGrabber>())
                        StartCoroutine(other.gameObject.GetComponent<EntityGrabber>().MoveTowardBomb(transform.position, absorbSpeed, deathFx));
                }
        }

        IEnumerator StartBehaviour()
        {
            yield return new WaitForSeconds(timeBeforeStart + 0.245f);
            if(hasStarted == false)      
                StartCoroutine(Absorb(false));
        }

        IEnumerator Absorb(bool hitWall)
        {
            Source sourceTemp;

            rb.velocity = Vector3.zero;
            transform.position = new Vector3(
            Mathf.RoundToInt(transform.position.x),
            transform.position.y,
            Mathf.RoundToInt(transform.position.z));
            if (hitWall == true)
                transform.Rotate(new Vector3(0, 180, 0));
            hasStarted = true;
            boxCollider.enabled = false;
            yield return new WaitForSeconds(0.3f);
            absorbFx.Play();
            animator.Play("Anim_Kettle_Absorb");
            sourceTemp = AudioManager.Instance.PlayThisSoundEffect("MarmiteAspire", true);
            capCollider.enabled = true;
            yield return new WaitForSeconds(timeBeforeIgnition);
            sourceTemp.audioSource.Stop();
            if(ignited == false)
            StartCoroutine(Ignite());
        }

        public IEnumerator Ignite()
        {
            ignited = true;
            capCollider.enabled = false;
            Destroy(absorbFx.gameObject);
            animator.Play("Anim_Object_KettleExplosion");
            AudioManager.Instance.PlaySoundEffect("MarmiteEnd");
            yield return new WaitForSeconds(0.75f);
            explosionFx.Play();
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
        }
    }
}

