using UnityEngine;
using GameManagement;

public class TutorialTrigger : MonoBehaviour
{
    public int tutoIndex;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("PlayerController"))
        {
            GameManager.Instance.uiHandler.tutorialUI.ActivateTutorialUI(tutoIndex);
        }
    }
}
