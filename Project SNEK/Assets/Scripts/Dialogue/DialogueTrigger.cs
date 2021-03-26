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
        StartCoroutine(DialogueManager.Instance.StartDialogue(dialogue));
    }
}
