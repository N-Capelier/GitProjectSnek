using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueManagement;
using Hub.UI;

namespace Hub.Interaction
{
    public class LevelInteraction : Interactor
    {
        protected override void Interact()
        {
            HubUiManager.Instance.OpenLevelAccess();
            HubUiManager.Instance.SetOccupied(true);
        }
    }
}


