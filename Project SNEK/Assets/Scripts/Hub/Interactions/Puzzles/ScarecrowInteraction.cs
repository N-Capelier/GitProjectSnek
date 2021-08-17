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
        public List<ObjectInteraction> pumpkinsList = new List<ObjectInteraction>();
        public float simonLength;
        private Coroutine simonCoroutine;

        public override IEnumerator BeginInteraction()
        {
            Interact();
            yield return null;
        }

        protected override void Interact()
        {
            interacting = true;
            objectAnimator.Play(Animator.StringToHash(animationName));
            AudioManager.Instance.PlaySoundEffect(scarecrowSoundEffectName);

            StartCoroutine(Clock());
            simonCoroutine = StartCoroutine(SimonCoroutine());
            interacting = false;
        }

        private IEnumerator SimonCoroutine()
        {
            for (int i = 0; i < pumpkinsList.Count; i++)
            {
                yield return new WaitUntil(() => pumpkinsList[i].interacting);
                print("pressed pumpkin" + i);
            }
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

            Debug.Log("Clock Finished Simon");
        }
    }
}

