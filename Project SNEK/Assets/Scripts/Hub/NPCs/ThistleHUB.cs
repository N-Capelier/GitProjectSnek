using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueManagement;

namespace Saving
{
    public class ThistleHUB : NPCHUB
    {
        [Space]
        [SerializeField] Transform waypointOutOfVillage;
        [Space]
        [SerializeField] Dialogue dialogue2;
        [SerializeField] Transform waypoint2;
        [Space]
        [SerializeField] Dialogue dialogue3;
        [SerializeField] Transform waypoint3;
        [Space]
        [SerializeField] Dialogue dialogue4;
        [SerializeField] Transform waypoint4;

        private void Start()
        {
            Refresh();
        }

        public override void Refresh()
        {
            switch (SaveManager.Instance.state.thistleState)
            {
                //Chapitre Tutoriel
                case 1f:
                    SetTransform(waypointOutOfVillage);
                    break;
                case 2f:
                    SetDialogue(dialogue2);
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