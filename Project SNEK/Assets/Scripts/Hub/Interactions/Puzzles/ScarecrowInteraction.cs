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
        public string winSoundEffectName;
        public string defeatSoundEffectName;
        public List<PumpkinInteraction> pumpkinsList = new List<PumpkinInteraction>();
        public PumpkinInteraction currentPumpkin;
        public float simonLength;
        public Coroutine simonCoroutine;
        private Coroutine clockCoroutine;

        [Header("Feedback")]
        public ParticleSystem startParticle;
        public ParticleSystem winParticle;
        public ParticleSystem failParticle;

        public override IEnumerator BeginInteraction()
        {
            Interact();
            yield return null;
        }

        protected override void Interact()
        {
            objectAnimator.Play(Animator.StringToHash(animationName));
            AudioManager.Instance.PlaySoundEffect(scarecrowSoundEffectName);

            if(simonCoroutine == null) // + check save state
            {
                clockCoroutine = StartCoroutine(Clock());
                simonCoroutine = StartCoroutine(SimonCoroutine());
                startParticle.Play();
            }
        }

        private IEnumerator SimonCoroutine()
        {
            currentPumpkin = null;
            for (int i = 0; i < pumpkinsList.Count; i++)
            {
                yield return new WaitUntil(() => currentPumpkin!= null);

                if (currentPumpkin == pumpkinsList[i])
                {
                    print("Pressed correct pumpkin");
                    currentPumpkin = null;
                    continue;
                }
                else
                {
                    print("You failed");
                    //Play defeat sound
                    failParticle.Play();
                    AudioManager.Instance.PlaySoundEffect(defeatSoundEffectName);
                    if (clockCoroutine != null)
                    {
                        StopCoroutine(clockCoroutine);
                        clockCoroutine = null;
                    }

                    StopCoroutine(simonCoroutine);
                    simonCoroutine = null;
                }

            }
            if(clockCoroutine!=null)
                StopCoroutine(clockCoroutine);

            clockCoroutine = null;
            simonCoroutine = null;

            //Play victory sound and give reward + update save state
            AudioManager.Instance.PlaySoundEffect(winSoundEffectName);
            winParticle.Play();
            print("Finished Simon");
        }

        IEnumerator Clock()
        {
            yield return new WaitForSeconds(simonLength);
            if (simonCoroutine != null)
            {
                print("You failed");
                //Play defeat sound
                failParticle.Play();
                AudioManager.Instance.PlaySoundEffect(defeatSoundEffectName);
                StopCoroutine(simonCoroutine);
                simonCoroutine = null;
            }
            clockCoroutine = null;
        }
    }
}

