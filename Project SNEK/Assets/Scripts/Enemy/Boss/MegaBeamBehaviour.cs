using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using Player.Spells;
using Boss;

namespace Enemy
{
    public class MegaBeamBehaviour : MonoBehaviour
    {
        public int deathIndex;
        bool destroyedByShield = false;
        bool isShieldOver = false;
        bool hasHitShield = false;
        BoxCollider col;

        [SerializeField] GameObject objectRenderer;

        private void Awake()
        {
            col = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("PlayerController") && hasHitShield == false)
            {
                PlayerManager.Instance.currentController.Death(deathIndex);
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Shield") && hasHitShield == false)
            {
                col.enabled = false;
                hasHitShield = true;
            }
        }

        private void Update()
        {
            if (hasHitShield && !PlayerManager.Instance.currentController.runController.playerRunSpell.GetComponent<ThistleSpell>().shield.activeSelf)
            {
                Debug.Log("shield over");
                BossParanoia.Instance.isMegaBeamOver = true;
            }
        }


        IEnumerator DestroyedByTime()
        {
            yield return new WaitForSeconds(BossParanoia.Instance.timeOfBeam);
            if (gameObject != null && destroyedByShield == false)
            {
                gameObject.GetComponent<BoxCollider>().enabled = false;
                objectRenderer.SetActive(false);
                Destroy(gameObject);
            }
        }

        public IEnumerator Destroyed()
        {
            destroyedByShield = true;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            objectRenderer.SetActive(false);
            yield return null;
            Destroy(gameObject);
        }
    }
}
