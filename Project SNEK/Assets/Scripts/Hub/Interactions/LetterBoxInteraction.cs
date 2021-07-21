using GameManagement;
using Hub.UI;
using Saving;
using UnityEngine;

namespace Hub.Interaction
{
    public class LetterBoxInteraction : Interactor
    {
        public GameObject newLetterFeedback;
        public void Start()
        {
            GameManager.Instance.uiHandler.hubUI.newLetterFeedback = newLetterFeedback;
            GameManager.Instance.uiHandler.hubUI.InitLetterBoxFeedback();
        }

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
