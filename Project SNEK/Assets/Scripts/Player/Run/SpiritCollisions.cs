using UnityEngine;
using Player;

namespace Player.Spirits
{
    public class SpiritCollisions : MonoBehaviour
    {
        [SerializeField] SpiritBehaviour behaviour;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("PlayerController"))
            {
                PlayerManager.Instance.currentController.Death(0);
            }
            else if(other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                behaviour.SendDeathInfo();
            }
        }
	}
}