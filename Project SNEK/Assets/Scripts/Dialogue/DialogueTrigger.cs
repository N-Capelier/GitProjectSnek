using UnityEngine;
using DialogueManagement;

/// <summary>
/// Corentin
/// </summary>
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
    }
}
