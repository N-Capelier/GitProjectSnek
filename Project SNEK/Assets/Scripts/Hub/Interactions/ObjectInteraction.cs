using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hub.UI;
using AudioManagement;

namespace Hub.Interaction
{
    public class ObjectInteraction : Interactor
    {
        public string animationName;
        public Animator objectAnimator;

        public override IEnumerator BeginInteraction()
        {
            
            Interact();
            yield return null;
        }

        protected override void Interact()
        {
            objectAnimator.Play(Animator.StringToHash(animationName));
            AudioManager.Instance.PlaySoundEffect("HubInteract");
        }
    }
}

