using UnityEngine;
using GameManagement;

namespace Hub
{
    public class HubButtons : MonoBehaviour
    {

        public void StartLevel1()
        {
            GameManager.Instance.gameState.Set(GameState.Run, "Nico");
        }

    }
}