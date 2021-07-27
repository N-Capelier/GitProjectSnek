using DialogueManagement;
using GameManagement;
using Hub.UI;

namespace Hub.Interaction
{
    /// <summary>
    /// Coco
    /// </summary>
    public class FountainInteraction : Interactor
    {
        protected override void Interact()
        {
            if(!GameManager.Instance.uiHandler.pauseUI.opened)
            {
                GameManager.Instance.uiHandler.hubUI.OpenFountainBox();
                GameManager.Instance.uiHandler.hubUI.SetOccupied(true);
            }
            else
            {
                InteractionManager.Instance.EndInteraction();
            }
        }
    }
}


