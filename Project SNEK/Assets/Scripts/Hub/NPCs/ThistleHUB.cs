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

        public override void Refresh()
        {
            if(SaveManager.Instance.state.thistleState > 1)
            {
                SaveManager.Instance.state.talkedOnceToThistle = true;
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
            }
            started = true;
        }
    }
}