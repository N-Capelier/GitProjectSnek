using UnityEngine;
using DialogueManagement;

namespace Hub.Interaction
{
    public class ExampleInteration : Interactor
    {
        [SerializeField] Dialogue dialogue;
        protected override void Interact()
        {
            StartCoroutine(DialogueManager.Instance.StartDialogue(dialogue));
        }
    }
}