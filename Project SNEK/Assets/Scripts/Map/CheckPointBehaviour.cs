using UnityEngine;
using Player;

namespace Map
{
    public class CheckPointBehaviour : MonoBehaviour
    {
        bool hasChecked = false;

        private void OnTriggerEnter(Collider other)
        {
            if(!hasChecked)
            {
                PlayerManager.Instance.currentController.checkPoint = transform;
                hasChecked = true;
            }
        }
    }
}