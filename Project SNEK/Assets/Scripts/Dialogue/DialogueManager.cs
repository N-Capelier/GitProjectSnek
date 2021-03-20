using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using GameManagement;

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

        private void Awake()
        {
            CreateSingleton(true);
        }
        private void Start()
        {
            canvas.SetActive(false);
            InputHandler.InputReceived += OnTap;
        }

        public void StartDialogue(Dialogue dialogue)
        {
            if (isRunningDialogue)
            {
                Debug.LogError("Cannot start a dialogue when it's already running!");
                return;
            }
            currentDialogue = dialogue;
            isRunningDialogue = true;
            isTapped = false;
            sentenceIndex = 0;
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
            sentenceIndex++;
            isSpeaking = false;
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
            isRunningDialogue = false;
            isTapped = false;
            canvas.SetActive(false);
        }

    }
}

