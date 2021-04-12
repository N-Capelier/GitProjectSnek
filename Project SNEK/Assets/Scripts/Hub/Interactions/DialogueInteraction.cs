using UnityEngine;
using DialogueManagement;

namespace Hub.Interaction
{
    public class DialogueInteraction : Interactor
    {
        public Dialogue dialogue;

        protected override void Interact()
        {
            StartCoroutine(DialogueManager.Instance.StartDialogue(dialogue, animator));
        }
    }
}