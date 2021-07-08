using UnityEngine;
using DialogueManagement;
using GameManagement;

/// <summary>
/// Corentin
/// </summary>
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        StartCoroutine(GameManager.Instance.uiHandler.dialogueUI.StartDialogue(dialogue));
    }
}
