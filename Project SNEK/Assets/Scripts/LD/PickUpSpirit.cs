using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Spirits;
using Player;

public class PickUpSpirit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("PlayerController"))
        {
            PlayerManager.Instance.currentController.playerRunSpirits.AddSpirit();
            //Instantiate pickup particle
            Destroy(gameObject);
        }
    }
}
