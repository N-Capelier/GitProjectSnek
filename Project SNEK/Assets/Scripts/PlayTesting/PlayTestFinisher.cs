using UnityEngine;
using Saving;

namespace Playtesting
{
    public class PlayTestFinisher : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                SaveManager.Instance.state.isDemoFinished = true;
        }

    }
}