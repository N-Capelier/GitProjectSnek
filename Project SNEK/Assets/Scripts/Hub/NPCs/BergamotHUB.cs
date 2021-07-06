using UnityEngine;
using DialogueManagement;
using UnityEngine.Timeline;

namespace Saving
{
    public class BergamotHUB : NPCHUB
    {
        #region old
        /*[Space]
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
        [Space]
        [Header("State16")]
        [SerializeField] Dialogue dialogue16;
        [SerializeField] Transform waypoint16;
        [Space]
        [Header("State17")]
        [SerializeField] Dialogue dialogue17;
        [SerializeField] Transform waypoint17;
        [Space]
        [Header("State18")]
        [SerializeField] Dialogue dialogue18;
        [SerializeField] Transform waypoint18;
        [Space]
        [Header("State19")]
        [SerializeField] Dialogue dialogue19;
        [SerializeField] Transform waypoint19;
        [Space]
        [Header("State20")]
        [SerializeField] Dialogue dialogue20;
        [SerializeField] Transform waypoint20;
        [Space]
        [Header("State21")]
        [SerializeField] Dialogue dialogue21;
        [SerializeField] Transform waypoint21;
        [Space]
        [Header("State22")]
        [SerializeField] Dialogue dialogue22;
        [SerializeField] Transform waypoint22;
        [Space]
        [Header("State23")]
        [SerializeField] Dialogue dialogue23;
        [SerializeField] Transform waypoint23;
        [Space]
        [Header("State24")]
        [SerializeField] Dialogue dialogue24;
        [SerializeField] Transform waypoint24;
        [Space]
        [Header("State25")]
        [SerializeField] Dialogue dialogue25;
        [SerializeField] Transform waypoint25;
        [Space]
        [Header("State26")]
        [SerializeField] Dialogue dialogue26;
        [SerializeField] Transform waypoint26;
        [Space]
        [Header("State27")]
        [SerializeField] Dialogue dialogue27;
        [SerializeField] Transform waypoint27;
        [Space]
        [Header("State28")]
        [SerializeField] Dialogue dialogue28;
        [SerializeField] Transform waypoint28;
        [Space]
        [Header("State29")]
        [SerializeField] Dialogue dialogue29;
        [SerializeField] Transform waypoint29;*/
        #endregion

        public override void Refresh()
        {
            if(SaveManager.Instance.state.talkedOnceToPoppy && SaveManager.Instance.state.talkedOnceToThistle && SaveManager.Instance.state.bergamotState.IsBetween(5f, 10f, ClusingType.II))
            {
                SaveManager.Instance.state.bergamotState = 11f;
            }

            for (int i = 0; i < npcStates.Count; i++)
            {
                if(npcStates[i].stateID == SaveManager.Instance.state.bergamotState)
                {
                    switch(npcStates[i].stateType)
                    {
                        case NPCStateType.None:
                        default:
                            break;
                        case NPCStateType.Dialogue:
                            SetDialogue(npcStates[i].dialogue);
                            break;
                        case NPCStateType.Cutscene:
                            PlayCutscene(npcStates[i].cutscene);
                            break;
                    }

                    if (npcStates[i].notInVillage)
                        SetTransform(waypointOutOfVillage);
                    else
                        SetTransform(npcStates[i].waypoint);

                    if(npcStates[i].setNewStates)
                    {
                        if(npcStates[i].newBergamotState != 0)
                            SaveManager.Instance.state.bergamotState = npcStates[i].newBergamotState;
                        if(npcStates[i].newPoppyState != 0)
                            SaveManager.Instance.state.poppyState = npcStates[i].newPoppyState;
                        if(npcStates[i].newThistleState != 0)
                            SaveManager.Instance.state.thistleState = npcStates[i].newThistleState;
                    }

                    break;
                }
            }

            started = true;

            #region old
            /*switch (SaveManager.Instance.state.bergamotState)
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
                case 16f:
                    SetDialogue(dialogue16);
                    SetTransform(waypoint16);
                    break;
                case 17f:
                    SetDialogue(dialogue17);
                    SetTransform(waypoint17);
                    break;
                case 18f:
                    SetDialogue(dialogue18);
                    SetTransform(waypoint18);
                    break;
                case 19f:
                    SetDialogue(dialogue19);
                    SetTransform(waypoint19);
                    break;
                case 20f:
                    SetDialogue(dialogue20);
                    SetTransform(waypoint20);
                    break;
                case 21f:
                    SetDialogue(dialogue21);
                    SetTransform(waypoint21);
                    break;
                case 22f:
                    SetDialogue(dialogue22);
                    SetTransform(waypoint22);
                    break;
                case 23f:
                    SetDialogue(dialogue23);
                    SetTransform(waypoint23);
                    break;
                case 24f:
                    SetDialogue(dialogue24);
                    SetTransform(waypoint24);
                    break;
                case 25f:
                    SetDialogue(dialogue25);
                    SetTransform(waypoint25);
                    break;
                case 26f:
                    SetDialogue(dialogue26);
                    SetTransform(waypoint26);
                    break;
                case 27f:
                    SetDialogue(dialogue27);
                    SetTransform(waypoint27);
                    break;
                case 28f:
                    SetDialogue(dialogue28);
                    SetTransform(waypoint28);
                    break;
                case 29f:
                    SetDialogue(dialogue29);
                    SetTransform(waypoint29);
                    break;
            }*/
            //started = true;
            #endregion

        }
    }
}