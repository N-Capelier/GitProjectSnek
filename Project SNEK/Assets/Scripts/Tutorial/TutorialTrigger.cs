using UnityEngine;
using GameManagement;

public class TutorialTrigger : MonoBehaviour
{
    public int tutoIndex;
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("PlayerController") && !triggered)
        {
            GameManager.Instance.uiHandler.tutorialUI.ActivateTutorialUI(tutoIndex);
            triggered = true;
        }
    }
}
