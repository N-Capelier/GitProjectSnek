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
            GameManager.Instance.uiHandler.hubUI.OpenFountainBox();
            GameManager.Instance.uiHandler.hubUI.SetOccupied(true);
        }
    }
}


