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
            StartCoroutine(GameManager.Instance.uiHandler.dialogueUI.StartDialogue(dialogue, animator));
        }
    }
}