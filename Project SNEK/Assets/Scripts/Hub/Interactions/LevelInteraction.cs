using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueManagement;
using Hub.UI;
using GameManagement;

namespace Hub.Interaction
{
    public class LevelInteraction : Interactor
    {
        protected override void Interact()
        {
            GameManager.Instance.uiHandler.hubUI.OpenLevelAccess();
            GameManager.Instance.uiHandler.hubUI.SetOccupied(true);
        }
    }
}


