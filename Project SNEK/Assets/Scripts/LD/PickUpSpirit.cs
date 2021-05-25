using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Spirits;
using Player;
using AudioManagement;

public class PickUpSpirit : MonoBehaviour
{
    public GameObject getSpiritParticle;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("PlayerController"))
        {
            PlayerManager.Instance.currentController.playerRunSpirits.AddSpirit();
            AudioManager.Instance.PlaySoundEffect("ObjectSpiritCollect");
            Instantiate(getSpiritParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
