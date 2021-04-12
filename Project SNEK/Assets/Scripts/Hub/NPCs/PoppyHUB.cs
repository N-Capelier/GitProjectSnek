using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saving;
using Hub.Interaction;
using DialogueManagement;

namespace Saving
{
    public class PoppyHUB : NPCHUB
    {
        [Space]
        [SerializeField] Dialogue dialogue1;
        [SerializeField] Transform waypoint1;
        [Space]
        [SerializeField] Dialogue dialogue2;
        [Space]
        [SerializeField] Dialogue dialogue3;
        [Space]
        [SerializeField] Dialogue dialogue4;
        [Space]
        [SerializeField] Dialogue dialogue5;
        [Space]
        [SerializeField] Dialogue dialogue6;
        [Space]
        [SerializeField] Dialogue dialogue7;
        [Space]
        [SerializeField] Dialogue dialogue8;
        [Space]
        [SerializeField] Dialogue dialogue9;

        private void Start()
        {
            Refresh();
        }

        public override void Refresh()
        {
            switch (SaveManager.Instance.state.poppyState)
            {
                //Chapitre Tutoriel
                case 1f:
                    SetDialogue(dialogue1);
                    SetTransform(waypoint1);
                    break;
                case 2f:
                    SetDialogue(dialogue2);
                    SetTransform(waypoint1);
                    break;
                case 3f:
                    SetDialogue(dialogue3);
                    SetTransform(waypoint1);
                    break;
                case 4f:
                    SetDialogue(dialogue4);
                    SetTransform(waypoint1);
                    break;
                case 5f:
                    SetDialogue(dialogue5);
                    SetTransform(waypoint1);
                    break;
                case 6f:
                    SetDialogue(dialogue6);
                    SetTransform(waypoint1);
                    break;
                case 7f:
                    SetDialogue(dialogue7);
                    SetTransform(waypoint1);
                    break;
                case 8f:
                    SetDialogue(dialogue8);
                    SetTransform(waypoint1);
                    break;
                case 9f:
                    SetDialogue(dialogue9);
                    SetTransform(waypoint1);
                    break;
            }
            started = true;
        }
    }
}