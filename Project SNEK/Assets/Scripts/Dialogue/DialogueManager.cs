using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameManagement;
using Hub.Interaction;

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
        [SerializeField] GameObject canvas;
        [SerializeField] GameObject DialogueBox,SkillTreeBox, LevelAccessBox;
        Dialogue currentDialogue;
        bool isRunningDialogue;
        bool isSpeaking;
        bool isTapped;
        float charDelay;
        int sentenceIndex;
        Animator animator;

        [SerializeField] float dialogBoxOffset;

        private void Awake()
        {
            CreateSingleton(true);
        }
        private void Start()
        {
            DialogueBox.transform.localPosition = new Vector3(0, -Screen.height * dialogBoxOffset);
            SkillTreeBox.transform.localScale = Vector3.zero;
            LevelAccessBox.transform.localScale = Vector3.zero;
            //canvas.SetActive(false);
            InputHandler.InputReceived += OnTap;
        }

        public IEnumerator StartDialogue(Dialogue dialogue, Animator animator = null)
        {
            if (isRunningDialogue)
            {
                Debug.LogError("Cannot start a dialogue when it's already running!");
                yield break;
            }
            currentDialogue = dialogue;
            isRunningDialogue = true;
            isTapped = false;
            sentenceIndex = 0;
            this.animator = animator;
            OpenDialogueBox();
            //Mouvement de caméra
            StartCoroutine(WriteNextLine());
        }

        IEnumerator WriteNextLine()
        {
            isSpeaking = true;
            charDelay = 0.05f;
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
            foreach (char letter in currentDialogue.sentences[sentenceIndex].sentence.ToCharArray())
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(charDelay);
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
                    charDelay = 0f;
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

        public void OpenSkillTree()
        {
            SkillTreeBox.transform.LeanScale(Vector3.one,0.3f);
        }

        public void CloseSkillTree()
        {
            SkillTreeBox.transform.LeanScale(Vector3.zero, 0.3f);
            if (GameManager.Instance.gameState.ActiveState == GameState.Hub)
            {
                InteractionManager.Instance.EndInteraction();
            }
        }

        public void OpenLevelAccess()
        {
            LevelAccessBox.transform.LeanScale(Vector3.one, 0.2f);
        }

        public void CloseLevelAcces()
        {
            LevelAccessBox.transform.LeanScale(Vector3.zero, 0.2f);
            InteractionManager.Instance.EndInteraction();
            if (GameManager.Instance.gameState.ActiveState == GameState.Hub)
            {
                InteractionManager.Instance.EndInteraction();
            }
        }

        public void OpenBox(GameObject box)
        {
            box.LeanScale(Vector3.one, 0.2f);
        }

        public void CloseBox(GameObject box)
        {
            box.LeanScale(Vector3.zero, 0.2f);
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
    }
}

