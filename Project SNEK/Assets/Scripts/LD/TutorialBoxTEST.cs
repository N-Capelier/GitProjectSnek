using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBoxTEST : MonoBehaviour
{
    public Animator animator;
    public string animName;
    public GameObject UiText;
    [SerializeField] bool isAnim;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("PlayerController"))
        {
            if (isAnim)
            {
                animator.Play(animName);
            }
            else
            {
                if(UiText.activeSelf == false)
                UiText.SetActive(true);
                else
                {
                    UiText.SetActive(false);
                }
            }

        }
    }
}
