using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Wall 
{
    public class WallBehaviour : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("PlayerController"))
            {
                PlayerManager.Instance.currentController.Death();
            }
        }
    }
}

