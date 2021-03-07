using UnityEngine;
using Player;

namespace Map
{
    /// <summary>
    /// Nico
    /// </summary>
    public class CheckPointBehaviour : MonoBehaviour
    {
        bool hasChecked = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!hasChecked && other.CompareTag("Player"))
            {
                PlayerManager.Instance.currentController.checkPoint = transform;
                PlayerManager.Instance.currentController.respawnNode = transform.position;
                hasChecked = true;
            }
        }
    }
}