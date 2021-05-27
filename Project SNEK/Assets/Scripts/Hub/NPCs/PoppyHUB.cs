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
        [Header ("State1")]
        [SerializeField] Transform waypoint1;
        [Space]
        [Header("State2")]
        [SerializeField] Dialogue dialogue2;
        [SerializeField] Transform waypoint2;
        [Space]
        [Header("State3")]
        [SerializeField] Dialogue dialogue3;
        [Space]
        [Header("State4")]
        [SerializeField] Dialogue dialogue4;
        [Space]
        [Header("State5")]
        [SerializeField] Dialogue dialogue5;
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
        [SerializeField] Transform waypoint29;
        [Space]
        [Header("State30")]
        [SerializeField] Dialogue dialogue30;
        [SerializeField] Transform waypoint30;
        [Space]
        [Header("State31")]
        [SerializeField] Dialogue dialogue31;
        [SerializeField] Transform waypoint31;
        [Space]
        [Header("State32")]
        [SerializeField] Dialogue dialogue32;
        [SerializeField] Transform waypoint32;

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