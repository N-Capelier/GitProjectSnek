using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueManagement;
using Hub.UI;

namespace Hub.Interaction
{
    public class LetterBoxInteraction : Interactor
    {
        protected override void Interact()
        {
            HubUiManager.Instance.OpenLetterBox();
        }
    }

}
