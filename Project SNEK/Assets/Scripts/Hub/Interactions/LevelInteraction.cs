using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueManagement;
using Hub.UI;
using GameManagement;
using Saving;

namespace Hub.Interaction
{
    public class LevelInteraction : Interactor
    {
        protected override void Interact()
        {
            if(SaveManager.Instance.state.unlockedLevels > 0)
            {
                GameManager.Instance.uiHandler.hubUI.OpenLevelAccess();
                GameManager.Instance.uiHandler.hubUI.SetOccupied(true);
            }
            else
            {
                InteractionManager.Instance.EndInteraction();
            }
        }
    }
}


