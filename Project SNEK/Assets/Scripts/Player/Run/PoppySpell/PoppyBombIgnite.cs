using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Spells
{
    public class PoppyBombIgnite : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("Attack"))
                if(gameObject.GetComponentInParent<PoppyBomb>() != null)
                    StartCoroutine(gameObject.GetComponentInParent<PoppyBomb>().Ignite());
        }
    }
}

