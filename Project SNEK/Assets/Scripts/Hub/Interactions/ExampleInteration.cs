using UnityEngine;
using DialogueManagement;

namespace Hub.Interaction
{
    public class ExampleInteration : Interactor
    {
        [SerializeField] Dialogue dialogue;
        protected override void Interact()
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }
    }
}