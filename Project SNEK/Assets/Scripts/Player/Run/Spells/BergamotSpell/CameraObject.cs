using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Spells
{
    public class CameraObject : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] AnimationClip photoAnim;

        void Start()
        {
            StartCoroutine(AnimationCoroutine());
        }

        IEnumerator AnimationCoroutine()
        {
            int stateHashName = Animator.StringToHash("Anim_Object_Photo");
            animator.Play(stateHashName);
            yield return new WaitForSeconds(photoAnim.length);
            Destroy(gameObject);
        }

    }

}
