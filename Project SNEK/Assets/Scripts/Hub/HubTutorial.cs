using GameManagement;
using Hub.UI;
using Player;
using Player.Controller;
using Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hub
{
    public class HubTutorial : Singleton<HubTutorial>
    {   
        public void Awake()
        {
            CreateSingleton();
        }

        PlayerHubController playerHub;
        [Header("Targets")]
        [SerializeField] public Transform letterBoxTransform;
        [SerializeField] public Transform fountainTransform;

        public void Init()
        {
            if (!SaveManager.Instance.state.isTutorialFinished && SaveManager.Instance.state.tutorialState != 0)
            {
                UpdateTutorialState(SaveManager.Instance.state.tutorialState);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void UpdateTutorialState(int state)
        {
            SaveManager.Instance.state.tutorialState = state;
            switch (state)
            {
                case 0:
                    //Afficher la box de tutoriel + "Try to find Bergamot in the village"
                    GameManager.Instance.uiHandler.hubUI.OpenTutoBox(0);

                    //Bergamot devient la target (flèche + feedback au dessus de bergamot)
                    playerHub = (PlayerHubController)PlayerManager.Instance.currentController;

                    playerHub.directionArrow.SetActive(true);
                    playerHub.tutorialTarget = NPCManager.Instance.bergamot.transform;

                    NPCManager.Instance.bergamot.exclamationMark.SetActive(true);
                    NPCManager.Instance.bergamot.bubble.SetActive(false);

                    //Quand le joueur est assez près de Bergamot passer au state suivant
                    StartCoroutine(tutorialCase0());

                    break;

                case 1:
                    //Afficher la box interagir avec bergamot
                    GameManager.Instance.uiHandler.hubUI.OpenTutoBox(1);

                    playerHub = (PlayerHubController)PlayerManager.Instance.currentController;

                    StartCoroutine(tutorialCase1());
                    break;

                case 2:
                    //Afficher la box boite au lettre
                    GameManager.Instance.uiHandler.hubUI.OpenTutoBox(2);

                    playerHub = (PlayerHubController)PlayerManager.Instance.currentController;

                    playerHub.directionArrow.SetActive(true);
                    playerHub.tutorialTarget = NPCManager.Instance.bergamot.transform;

                    NPCManager.Instance.bergamot.exclamationMark.SetActive(true);
                    NPCManager.Instance.bergamot.bubble.SetActive(false);

                    StartCoroutine(tutorialCase2());
                    break;

                case 3:
                    //Afficher la box fontaine
                    GameManager.Instance.uiHandler.hubUI.OpenTutoBox(3);

                    playerHub = (PlayerHubController)PlayerManager.Instance.currentController;
                    playerHub.directionArrow.SetActive(true);
                    playerHub.tutorialTarget = fountainTransform;

                    StartCoroutine(tutorialCase3());
                    break;

                default:
                    Debug.LogError("Wrong state input");
                    break;
            }
        }

        public IEnumerator tutorialCase0()
        {
            yield return new WaitUntil(() => NPCManager.Instance.bergamot.bubble.activeSelf);
            NPCManager.Instance.bergamot.bubble.SetActive(false);
            yield return new WaitUntil(() => (playerHub.tutorialTarget.transform.position - playerHub.objectRenderer.transform.position).magnitude < 2);
            playerHub.directionArrow.SetActive(false);
            UpdateTutorialState(1);
        }

        public IEnumerator tutorialCase1()
        {
            //Wait til player interact with bergamot
            yield return new WaitUntil(()=> GameManager.Instance.uiHandler.dialogueUI.currentInteraction != null && GameManager.Instance.uiHandler.dialogueUI.currentInteraction.dialogue.mainCharacter == DialogueManagement.Character.Bergamot);
            NPCManager.Instance.bergamot.exclamationMark.SetActive(false);

            yield return new WaitUntil(() => !GameManager.Instance.uiHandler.dialogueUI.isRunningDialogue && GameManager.Instance.uiHandler.dialogueUI.currentDialogue == null);

            playerHub.directionArrow.SetActive(true);
            playerHub.tutorialTarget = letterBoxTransform;
            GameManager.Instance.uiHandler.dialogueUI.currentInteraction = null;

            yield return new WaitUntil(() => (playerHub.tutorialTarget.transform.position - playerHub.objectRenderer.transform.position).magnitude < 2);
            playerHub.directionArrow.SetActive(false);

            yield return new WaitUntil(() => GameManager.Instance.uiHandler.hubUI.letterBox.activeSelf);
            

            yield return new WaitUntil(() => SaveManager.Instance.state.readLetters.Count > 0 && !GameManager.Instance.uiHandler.hubUI.letterBox.activeSelf);
            UpdateTutorialState(2); 
        }

        public IEnumerator tutorialCase2()
        {
            yield return new WaitUntil(() => (playerHub.tutorialTarget.transform.position - playerHub.objectRenderer.transform.position).magnitude < 2);
            playerHub.directionArrow.SetActive(false);

            yield return new WaitUntil(() => GameManager.Instance.uiHandler.dialogueUI.currentInteraction != null && GameManager.Instance.uiHandler.dialogueUI.currentInteraction.dialogue.mainCharacter == DialogueManagement.Character.Bergamot);

            NPCManager.Instance.bergamot.exclamationMark.SetActive(false);

            yield return new WaitUntil(() => !GameManager.Instance.uiHandler.dialogueUI.isRunningDialogue && GameManager.Instance.uiHandler.dialogueUI.currentDialogue == null);
            UpdateTutorialState(3); 
        }

        public IEnumerator tutorialCase3()
        {
            yield return new WaitUntil(() => (playerHub.tutorialTarget.transform.position - playerHub.objectRenderer.transform.position).magnitude < 3);
            playerHub.directionArrow.SetActive(false);
            print("TUTORIAL FINISHED");
            SaveManager.Instance.state.isTutorialFinished = true;
            SaveManager.Instance.Save();
        }
    }
}
