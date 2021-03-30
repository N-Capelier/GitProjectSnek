using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueManagement;

namespace Hub.Interaction
{
    public class LevelInteraction : Interactor
    {
        protected override void Interact()
        {
            DialogueManager.Instance.OpenLevelAccess();
        }
    }
}


