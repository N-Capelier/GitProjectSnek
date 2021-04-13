using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameManagement;
using Hub.Interaction;
using Cinematic;
using Saving;

namespace DialogueManagement
{
    /// <summary>
    /// Corentin
    /// </summary>
    public class DialogueManager : Singleton<DialogueManager>
    {

        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] TextMeshProUGUI dialogueText;
        [SerializeField] Image dialogueArrow;
        [SerializeField] GameObject DialogueBox;
        Dialogue currentDialogue;
        bool isRunningDialogue;
        bool isSpeaking;
        bool isTapped;
        bool isCutSceneDialogue;
        bool skipSentence = false;
        int sentenceIndex;
        Animator animator, cinematicAnimator;

        [SerializeField] float dialogBoxOffset;

        private void Awake()
        {
            CreateSingleton();
        }
        private void Start()
        {
            DialogueBox.transform.localPosition = new Vector3(0, -Screen.height * dialogBoxOffset);
            InputHandler.InputReceived += OnTap;
        }
        public void SetCinematicDialogueAnimator(Animator animator)
        {
            cinematicAnimator = animator;
        }
        public void StartDialogueInCinematic(Dialogue dialogue)
        {
            StartCoroutine(StartDialogue(dialogue,cinematicAnimator));
        }
        public IEnumerator StartDialogue(Dialogue dialogue, Animator animator = null)
        {
            if (isRunningDialogue)
            {
                Debug.LogError("Cannot start a dialogue when it's already running!");
                yield break;
            }

            if(GameManager.Instance.gameState.ActiveState != GameState.Cinematic)
            {
                InteractionManager.Instance.camTarget.actions++;
                InteractionManager.Instance.playerController.actions++;
            }

            currentDialogue = dialogue;
            isRunningDialogue = true;
            isTapped = false;
            sentenceIndex = 0;

            if (currentDialogue.isCutScene)
            {
                StartCoroutine(CutsceneManager.Instance.PauseCutscene());
            }

            this.animator = animator;
            OpenDialogueBox();
            //Mouvement de caméra
            StartCoroutine(WriteNextLine());
        }

        IEnumerator WriteNextLine()
        {
            isSpeaking = true;
            //charDelay = 0.05f; //////
            if(currentDialogue.sentences[sentenceIndex].character == Character.Object)
            {
                nameText.text = "";
            }
            else
            {
                nameText.text = currentDialogue.sentences[sentenceIndex].character.ToString();
                if(animator != null && currentDialogue.sentences[sentenceIndex].anim != "")
                {
                    animator.Play(currentDialogue.sentences[sentenceIndex].anim);
                }
                else if(animator != null)
                {
                    animator.SetLayerWeight(animator.GetLayerIndex("Talk"), 1);
                }
            }

            NextLineFeedback();

            // Joue le SFX
            if (currentDialogue.sentences[sentenceIndex].activateButtons)
            {
                //Affiche l'ui des boutons
            }
            else
            {
                //Cacher l'ui des boutons
            }
            dialogueText.text = "";
            skipSentence = false;
            foreach (char letter in currentDialogue.sentences[sentenceIndex].sentence.ToCharArray())
            {
                dialogueText.text += letter;
                if(!skipSentence)
                {
                    yield return new WaitForSeconds(0.05f);
                }
                else
                {
                    dialogueText.text = currentDialogue.sentences[sentenceIndex].sentence;
                    break;
                }
            }

            NextLineFeedback();
            
            if(currentDialogue.sentences[sentenceIndex].character == Character.Poppy
                || currentDialogue.sentences[sentenceIndex].character == Character.Bergamot
                || currentDialogue.sentences[sentenceIndex].character == Character.Thistle)
            {
                animator.Play($"Anim_{currentDialogue.sentences[sentenceIndex].character}_idle");
            }

            if (animator != null)
            {
                animator.SetLayerWeight(animator.GetLayerIndex("Talk"), 0);
            }

            sentenceIndex++;
            isSpeaking = false;
            isTapped = false;
        }

        void OnTap(InputType input)
        {
            if (!isTapped)
            {
                isTapped = true;
                return;
            }
            if (input == InputType.Tap && isRunningDialogue)
            {
                if (isSpeaking)
                {
                    //charDelay = 0f; //////
                    skipSentence = true;
                }
                else if(sentenceIndex < currentDialogue.sentences.Length)
                {
                    StartCoroutine(WriteNextLine());
                }
                else
                {
                    EndDialogue();
                }
            }
        }

        void EndDialogue()
        {

            if(GameManager.Instance.gameState.ActiveState == GameState.Hub/* && CutsceneManager.Instance.mainDirector.playableAsset != null*/)
            {
                InteractionManager.Instance.camTarget.actions--;
                InteractionManager.Instance.playerController.actions--;
            }
            if (currentDialogue.isCutScene)
            {
                CutsceneManager.Instance.ResumeCutscene();
            }

            if(currentDialogue.bergamotNewState > 0)
            {
                if(currentDialogue.bergamotMinimumState >= SaveManager.Instance.state.bergamotState
                    && currentDialogue.poppyMinimumState >= SaveManager.Instance.state.poppyState
                    && currentDialogue.thistleMinimumState >= SaveManager.Instance.state.thistleState)
                {
                    SaveManager.Instance.state.bergamotState = currentDialogue.bergamotNewState;
                    if(GameManager.Instance.gameState.ActiveState == GameState.Hub)
                    {
                        NPCManager.Instance.RefreshNPCs();
                    }
                }
            }
            if (currentDialogue.poppyNewState > 0)
            {
                if (currentDialogue.bergamotMinimumState >= SaveManager.Instance.state.bergamotState
                    && currentDialogue.poppyMinimumState >= SaveManager.Instance.state.poppyState
                    && currentDialogue.thistleMinimumState >= SaveManager.Instance.state.thistleState)
                {
                    SaveManager.Instance.state.poppyState = currentDialogue.poppyNewState;
                }
            }
            if (currentDialogue.thistleNewState > 0)
            {
                if (currentDialogue.bergamotMinimumState >= SaveManager.Instance.state.bergamotState
                    && currentDialogue.poppyMinimumState >= SaveManager.Instance.state.poppyState
                    && currentDialogue.thistleMinimumState >= SaveManager.Instance.state.thistleState)
                {
                    SaveManager.Instance.state.thistleState = currentDialogue.thistleNewState;
                }
            }

            currentDialogue = null;
            animator = null;
            isRunningDialogue = false;
            isTapped = false;
            CloseDialogueBox();

        }

        public void OpenDialogueBox()
        {
            float dialogYPos = Screen.height * -0.05f;
            DialogueBox.transform.LeanMoveLocalY(dialogYPos, 0.5f);
        }

        public void CloseDialogueBox()
        {
            float dialogYPos = -Screen.height * dialogBoxOffset;
            DialogueBox.transform.LeanMoveLocalY(dialogYPos, 0.5f);
            if (GameManager.Instance.gameState.ActiveState == GameState.Hub)
            {
                InteractionManager.Instance.EndInteraction();
            }
        }

        public void NextLineFeedback()
        {
            if(dialogueArrow.transform.localScale == Vector3.zero)
            {
                dialogueArrow.transform.LeanScale(Vector3.one,0.2f);
            }
            else
            {
                dialogueArrow.transform.LeanScale(Vector3.zero, 0f);
            }
        }

        public void ChangeGameMode()
        {

        }

        public void LaunchCutscene()
        {

        }
    }
}

