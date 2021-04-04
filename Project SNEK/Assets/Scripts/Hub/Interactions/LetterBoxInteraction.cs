using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueManagement;

namespace Hub.Interaction
{
    public class LetterBoxInteraction : Interactor
    {
        protected override void Interact()
        {
            DialogueManager.Instance.OpenLetterBox();
        }
    }

}
