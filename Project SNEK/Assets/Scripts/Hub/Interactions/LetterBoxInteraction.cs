﻿using Hub.UI;
using Saving;

namespace Hub.Interaction
{
    public class LetterBoxInteraction : Interactor
    {
        protected override void Interact()
        {
            HubUiManager.Instance.OpenLetterBox();
            if(SaveManager.Instance.state.bergamotState == 3f)
            {
                SaveManager.Instance.state.bergamotState = 4f;
                NPCManager.Instance.RefreshNPCs();
            }
        }
    }

}
