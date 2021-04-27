using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hub.UI;

namespace Hub.Interaction
{
    public class ObjectInteraction : Interactor
    {
        public string animationName;
        public Animator objectAnimator;

        protected override void Interact()
        {
            objectAnimator.Play(animationName);
        }
    }
}

