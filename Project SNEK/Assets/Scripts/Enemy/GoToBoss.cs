using UnityEngine;
using GameManagement;

public class GoToBoss : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("PlayerController"))
        {
            GameManager.Instance.gameState.Set(GameState.Cinematic, "Boss Anorexia");
        }
    }
}
