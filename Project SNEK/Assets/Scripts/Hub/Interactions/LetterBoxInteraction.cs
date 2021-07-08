using GameManagement;
using Hub.UI;
using Saving;

namespace Hub.Interaction
{
    public class LetterBoxInteraction : Interactor
    {
        protected override void Interact()
        {
            GameManager.Instance.uiHandler.hubUI.OpenLetterBox();
            GameManager.Instance.uiHandler.hubUI.SetOccupied(true);
            //if (SaveManager.Instance.state.bergamotState == 3f)
            //{
            //    SaveManager.Instance.state.bergamotState = 4f;
            //    NPCManager.Instance.RefreshNPCs();
            //}
        }
    }

}
