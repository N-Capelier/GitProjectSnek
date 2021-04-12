using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saving;
using Hub.Interaction;
using DialogueManagement;

namespace Game
{
    public class BergamotHUB : MonoBehaviour
    {
        [SerializeField] DialogueInteraction dialogueInteraction;

        [SerializeField] Dialogue tuto1;
        
        private void Start()
        {
            switch(SaveManager.Instance.state.bergamotState)
            {
                case 1:
                    dialogueInteraction.dialogue = tuto1;
                    break;
            }
        }
    }
}