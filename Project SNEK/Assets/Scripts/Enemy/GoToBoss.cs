using UnityEngine;
using GameManagement;

public class GoToBoss : MonoBehaviour
{
    public int index;

    private void OnTriggerEnter(Collider other)
    {
        switch (index)
        {
            case 0:
                if (other.gameObject.layer == LayerMask.NameToLayer("PlayerController"))
                {
                    GameManager.Instance.gameState.Set(GameState.Cinematic, "Boss Anorexia");
                }
                break;
            case 1:
                if (other.gameObject.layer == LayerMask.NameToLayer("PlayerController"))
                {
                    GameManager.Instance.gameState.Set(GameState.Cinematic, "Boss Paranoia");
                }
                break;
        }

        
    }
}
