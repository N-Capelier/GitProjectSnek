using AudioManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hub.Interaction
{
    public class PumpkinInteraction : Interactor
    {
        public string animationName;
        public string soundEffectName;
        public Animator objectAnimator;
        [HideInInspector] public bool interacting;

        public override IEnumerator BeginInteraction()
        {

            Interact();
            yield return null;
        }

        protected override void Interact()
        {
            objectAnimator.Play(Animator.StringToHash(animationName));
            AudioManager.Instance.PlaySoundEffect(soundEffectName);
            interacting = true;
        }
    }
}
