using UnityEngine;
using DialogueManagement;
using UnityEngine.Timeline;

namespace Saving
{
    public class BergamotHUB : NPCHUB
    {
        [Space]
        [SerializeField] TimelineAsset cutsceneTuto;
        [SerializeField] Transform waypointTuto;
        [Space]
        [SerializeField] TimelineAsset cutscene2;
        [SerializeField] Transform waypoint2;
        [Space]
        [SerializeField] Dialogue dialogue3;
        [SerializeField] Transform waypoint3;
        [Space]
        [SerializeField] Dialogue dialogue4;
        [Space]
        [SerializeField] Dialogue dialogue5;
        [SerializeField] Transform waypoint5;
        [Space]
        [SerializeField] Dialogue dialogue6;
        [Space]
        [SerializeField] Dialogue dialogue7;
        [Space]
        [SerializeField] Dialogue dialogue8;
        [Space]
        [SerializeField] Dialogue dialogue9;
        [Space]
        [SerializeField] Dialogue dialogue10;
        [Space]
        [SerializeField] Dialogue dialogue11;
        [SerializeField] Transform waypoint11;
        [Space]
        [SerializeField] Dialogue dialogue12;
        [SerializeField] Transform waypoint12;

        public override void Refresh()
        {
            switch (SaveManager.Instance.state.bergamotState)
            {
                default:
                    Debug.LogError("No save state for BergamotHub!");
                    break;
                //Chapitre Tutoriel
                case 1f:
                    //dialogueInteraction.dialogue = tuto1;
                    PlayCutscene(cutsceneTuto);
                    SetTransform(waypointTuto);
                    SaveManager.Instance.state.bergamotState = 2f;
                    break;
                case 2f:
                    PlayCutscene(cutscene2);
                    SetTransform(waypoint2);
                    //SaveManager.Instance.state.bergamotState = 3f;
                    break;
                case 3f:
                    SetDialogue(dialogue3);
                    SetTransform(waypoint3);
                    break;
                case 4f:
                    SetDialogue(dialogue4);
                    SetTransform(waypoint3);
                    break;
                case 5f:
                    SetDialogue(dialogue5);
                    SetTransform(waypoint5);
                    break;
                case 6f:
                    SetDialogue(dialogue6);
                    SetTransform(waypoint5);
                    break;
                case 7f:
                    SetDialogue(dialogue7);
                    SetTransform(waypoint5);
                    break;
                case 8f:
                    SetDialogue(dialogue8);
                    SetTransform(waypoint5);
                    break;
                case 9f:
                    SetDialogue(dialogue9);
                    SetTransform(waypoint5);
                    break;
                case 10f:
                    SetDialogue(dialogue10);
                    SetTransform(waypoint5);
                    break;
                case 11f:
                    SetDialogue(dialogue11);
                    SetTransform(waypoint11);
                    break;
                case 12f:
                    SetDialogue(dialogue12);
                    SetTransform(waypoint12);
                    break;
            }
            started = true;
            print(SaveManager.Instance.state.bergamotState);
        }
    }
}