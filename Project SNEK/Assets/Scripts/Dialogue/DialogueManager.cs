using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using GameManagement;
using Hub.Interaction;

namespace DialogueManagement
{
    /// <summary>
    /// Corentin
    /// </summary>
    public class DialogueManager : Singleton<DialogueManager>
    {

        [SerializeField] Text nameText;
        [SerializeField] Text dialogueText;
        [SerializeField] GameObject canvas;
        Dialogue currentDialogue;
        bool isRunningDialogue;
        bool isSpeaking;
        bool isTapped;
        float charDelay;
        int sentenceIndex;
        Animator animator;

        private void Awake()
        {
            CreateSingleton(true);
        }
        private void Start()
        {
            canvas.SetActive(false);
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
            canvas.SetActive(true);
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
            canvas.SetActive(false);
            if (GameManager.Instance.gameState.ActiveState == GameState.Hub)
            {
                InteractionManager.Instance.EndInteraction();
            }
        }

    }
}

