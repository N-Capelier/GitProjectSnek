using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tutorial;

public class TutorialTrigger : MonoBehaviour
{

    public int tutoIndex;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("PlayerController"))
        {
            TutorialUIManager.Instance.ActivateTutorialUI(tutoIndex);
        }
    }
}
