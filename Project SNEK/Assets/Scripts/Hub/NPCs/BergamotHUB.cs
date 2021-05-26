using UnityEngine;
using DialogueManagement;
using UnityEngine.Timeline;

namespace Saving
{
    public class BergamotHUB : NPCHUB
    {
        [Space]
        [Header("State1")]
        [SerializeField] TimelineAsset cutsceneTuto;
        [SerializeField] Transform waypointTuto;
        [Space]
        [Header("State2")]
        [SerializeField] TimelineAsset cutscene2;
        [SerializeField] Transform waypoint2;
        [Space]
        [Header("State3")]
        [SerializeField] Dialogue dialogue3;
        [SerializeField] Transform waypoint3;
        [Space]
        [Header("State4")]
        [SerializeField] Dialogue dialogue4;
        [Space]
        [Header("State5")]
        [SerializeField] Dialogue dialogue5;
        [SerializeField] Transform waypoint5;
        [Space]
        [Header("State6")]
        [SerializeField] Dialogue dialogue6;
        [Space]
        [Header("State7")]
        [SerializeField] Dialogue dialogue7;
        [Space]
        [Header("State8")]
        [SerializeField] Dialogue dialogue8;
        [Space]
        [Header("State9")]
        [SerializeField] Dialogue dialogue9;
        [Space]
        [Header("State10")]
        [SerializeField] Dialogue dialogue10;
        [Space]
        [Header("State11")]
        [SerializeField] Dialogue dialogue11;
        [SerializeField] Transform waypoint11;
        [Space]
        [Header("State12")]
        [SerializeField] Dialogue dialogue12;
        [SerializeField] Transform waypoint12;

        public override void Refresh()
        {
            if(SaveManager.Instance.state.talkedOnceToPoppy && SaveManager.Instance.state.talkedOnceToThistle && SaveManager.Instance.state.bergamotState.IsBetween(5f, 10f, ClusingType.II))
            {
                SaveManager.Instance.state.bergamotState = 11f;
            }

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
        }
    }
}