using UnityEngine;
using DialogueManagement;
using GameManagement;

namespace Hub.Interaction
{
    public class DialogueInteraction : Interactor
    {
        public Dialogue dialogue;

        protected override void Interact()
        {
            if(!GameManager.Instance.uiHandler.pauseUI.opened)
            {
                StartCoroutine(GameManager.Instance.uiHandler.dialogueUI.StartDialogue(dialogue, animator));
                GameManager.Instance.uiHandler.dialogueUI.currentInteraction = this;
            }
            else
            {
                InteractionManager.Instance.EndInteraction();
            }
        }
    }
}