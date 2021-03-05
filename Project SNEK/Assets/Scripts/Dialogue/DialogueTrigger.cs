using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueManagement;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
    }
}
