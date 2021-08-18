using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hub.UI;
using AudioManagement;
using System;

namespace Hub.Interaction
{
    public class ScarecrowInteraction : Interactor
    {
        public string animationName;
        public Animator objectAnimator;

        [Header("Simon Mini-Game")]
        public string scarecrowSoundEffectName;
        public List<PumpkinInteraction> pumpkinsList = new List<PumpkinInteraction>();
        public float simonLength;
        private Coroutine simonCoroutine;
        private Coroutine clockCoroutine;

        public override IEnumerator BeginInteraction()
        {
            Interact();
            yield return null;
        }

        protected override void Interact()
        {
            objectAnimator.Play(Animator.StringToHash(animationName));
            AudioManager.Instance.PlaySoundEffect(scarecrowSoundEffectName);

            clockCoroutine = StartCoroutine(Clock());
            simonCoroutine = StartCoroutine(SimonCoroutine());
        }

        private IEnumerator SimonCoroutine()
        {
            for (int i = 0; i < pumpkinsList.Count; i++)
            {
                yield return new WaitUntil(() => pumpkinsList[i].interacting);
                pumpkinsList[i].interacting = false;
                print("pressed pumpkin " + i);
            }
            if(clockCoroutine!=null)
                StopCoroutine(clockCoroutine);

            clockCoroutine = null;
            simonCoroutine = null;
            print("Finished Simon");
        }

        IEnumerator Clock()
        {
            yield return new WaitForSeconds(simonLength);
            if (simonCoroutine != null)
            {
                StopCoroutine(simonCoroutine);
                simonCoroutine = null;
            }
            clockCoroutine = null;
        }
    }
}

