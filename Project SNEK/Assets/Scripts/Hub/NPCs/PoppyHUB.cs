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
        [SerializeField] Transform waypoint2;
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

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.O))
            {
                Debug.Log($"b{SaveManager.Instance.state.bergamotState} p{SaveManager.Instance.state.poppyState} t{SaveManager.Instance.state.thistleState}");
                Debug.Log($"pT{SaveManager.Instance.state.talkedOnceToPoppy} tT{SaveManager.Instance.state.talkedOnceToThistle}");
            }
        }

        public override void Refresh()
        {
            if (SaveManager.Instance.state.poppyState > 1)
            {
                SaveManager.Instance.state.talkedOnceToPoppy = true;
            }

            switch (SaveManager.Instance.state.poppyState)
            {
                default:
                    Debug.LogError("No save state for PoppyHub!");
                    break;
                //Chapitre Tutoriel
                case 1f:
                    SetDialogue(dialogue1);
                    SetTransform(waypoint1);
                    break;
                case 2f:
                    SetDialogue(dialogue2);
                    started = false;
                    SetTransform(waypoint2);
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