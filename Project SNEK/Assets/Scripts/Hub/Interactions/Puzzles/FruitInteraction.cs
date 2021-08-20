using AudioManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hub.Interaction
{
    public class FruitInteraction : Interactor
    {
        public string animationName;
        public string soundEffectName;
        public Animator objectAnimator;
        [HideInInspector] public bool interacting;
        public GameObject feedback;
        public GameObject fruitToActivate;

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
            Instantiate(feedback, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
            if (fruitToActivate != null)
                fruitToActivate.SetActive(true);
        }
    }

}
