using UnityEngine;
using GameManagement;

namespace Hub
{
    public class HubButtons : MonoBehaviour
    {

        public void StartLevel1()
        {
            StartCoroutine(GameManager.Instance.levelLauncher.StartLevel("Run"));
        }

    }
}