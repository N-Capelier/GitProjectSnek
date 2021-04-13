using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBoxTEST : MonoBehaviour
{
    public Animator animator;
    public string animName;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("PlayerController"))
        {
            animator.Play(animName);
        }
    }
}
