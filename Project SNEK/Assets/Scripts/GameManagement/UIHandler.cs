using DialogueManagement;
using Hub.UI;
using Map;
using PauseManagement;
using System.Collections;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

namespace GameManagement
{

    /// <summary>
    /// Thomas
    /// </summary>
    public class UIHandler : MonoBehaviour
    { 

        [Header("Run UI")]
        public LevelProgressUI levelProgressUI;
        public TutorialUIManager tutorialUI;
        public GameObject spellUI;
        public RectTransform spellButton;

        [Header("Hub UI")]
        public HubUiManager hubUI;

        [Header("Shared UI")]
        public DialogueManager dialogueUI;
        public PauseManager pauseUI;


        public void HideUIRun()
        {
            levelProgressUI.gameObject.SetActive(false);
            pauseUI.gameObject.SetActive(false);
            spellUI.SetActive(false);
        }

        public void ShowUIRun()
        {
            levelProgressUI.gameObject.SetActive(true);
            pauseUI.gameObject.SetActive(true);
        }

        public void HideUIHub()
        {
            pauseUI.gameObject.SetActive(false);
        }

        public void ShowUIHub()
        {
            pauseUI.gameObject.SetActive(true);
        }

        /// <summary>
        /// 0 = right
        /// 1 = left
        /// </summary>
        /// <param name="hand"></param>
        public void SwapHand(bool hand)
        {
            if(hand == true)
            {
                spellButton.anchorMin = new Vector2(0.85f, 0);
                spellButton.anchorMax = new Vector2(0.85f, 1);
            }
            else if(hand == false)
            {
                spellButton.anchorMin = new Vector2(0.15f, 0);
                spellButton.anchorMax = new Vector2(0.15f, 1);
            }
        }
    }




}

