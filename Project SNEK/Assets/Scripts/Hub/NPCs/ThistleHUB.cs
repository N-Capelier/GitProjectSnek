using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueManagement;

namespace Saving
{
    public class ThistleHUB : NPCHUB
    {
        [Space]
        [Header("State1")]
        [SerializeField] Transform waypointOutOfVillage;
        [Space]
        [Header("State2")]
        [SerializeField] Dialogue dialogue2;
        [SerializeField] Transform waypoint2;
        [Space]
        [Header("State3")]
        [SerializeField] Dialogue dialogue3;
        [SerializeField] Transform waypoint3;
        [Space]
        [Header("State4")]
        [SerializeField] Dialogue dialogue4;
        [SerializeField] Transform waypoint4;
        [Space]
        [Header("State6")]
        [SerializeField] Dialogue dialogue6;
        [SerializeField] Transform waypoint6;
        [Space]
        [Header("State7")]
        [SerializeField] Dialogue dialogue7;
        [SerializeField] Transform waypoint7;
        [Space]
        [Header("State8")]
        [SerializeField] Dialogue dialogue8;
        [SerializeField] Transform waypoint8;
        [Space]
        [Header("State9")]
        [SerializeField] Dialogue dialogue9;
        [SerializeField] Transform waypoint9;
        [Space]
        [Header("State10")]
        [SerializeField] Dialogue dialogue10;
        [SerializeField] Transform waypoint10;
        [Space]
        [Header("State11")]
        [SerializeField] Dialogue dialogue11;
        [SerializeField] Transform waypoint11;
        [Space]
        [Header("State12")]
        [SerializeField] Dialogue dialogue12;
        [SerializeField] Transform waypoint12;
        [Space]
        [Header("State13")]
        [SerializeField] Dialogue dialogue13;
        [SerializeField] Transform waypoint13;
        [Space]
        [Header("State14")]
        [SerializeField] Dialogue dialogue14;
        [SerializeField] Transform waypoint14;
        [Space]
        [Header("State15")]
        [SerializeField] Dialogue dialogue15;
        [SerializeField] Transform waypoint15;

        public override void Refresh()
        {
            if(SaveManager.Instance.state.thistleState > 2 && SaveManager.Instance.state.talkedOnceToThistle == false)
            {
                SaveManager.Instance.state.talkedOnceToThistle = true;
                NPCManager.Instance.RefreshNPCs();
                return;
            }

            switch (SaveManager.Instance.state.thistleState)
            {
                default:
                    Debug.LogError("No save state for ThistleHub!");
                    break;
                //Chapitre Tutoriel
                case 1f:
                    SetTransform(waypointOutOfVillage);
                    break;
                case 2f:
                    SetDialogue(dialogue2);
                    started = false;
                    SetTransform(waypoint2);
                    break;
                case 3f:
                    SetDialogue(dialogue3);
                    SetTransform(waypoint3);
                    break;
                case 4f:
                    SetDialogue(dialogue4);
                    SetTransform(waypoint4);
                    break;
                case 5f:
                    SetTransform(waypointOutOfVillage);
                    break;
                case 6f:
                    SetDialogue(dialogue6);
                    SetTransform(waypoint6);
                    break;
                case 7f:
                    SetDialogue(dialogue7);
                    SetTransform(waypoint7);
                    break;
                case 8f:
                    SetDialogue(dialogue8);
                    SetTransform(waypoint8);
                    break;
                case 9f:
                    SetDialogue(dialogue9);
                    SetTransform(waypoint9);
                    break;
                case 10f:
                    SetDialogue(dialogue10);
                    SetTransform(waypoint10);
                    break;
                case 11f:
                    SetDialogue(dialogue11);
                    SetTransform(waypoint11);
                    break;
                case 12f:
                    SetDialogue(dialogue12);
                    SetTransform(waypoint12);
                    break;
                case 13f:
                    SetDialogue(dialogue13);
                    SetTransform(waypoint13);
                    break;
                case 14f:
                    SetDialogue(dialogue14);
                    SetTransform(waypoint14);
                    break;
                case 15f:
                    SetDialogue(dialogue15);
                    SetTransform(waypoint15);
                    break;
            }
            started = true;
        }
    }
}