using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameManagement;
using Hub.Interaction;
using Cinematic;
using Saving;
using AudioManagement;
using PauseManagement;
using FaceManager;
using Player;
using System.Text;

namespace DialogueManagement
{
    /// <summary>
    /// Corentin
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {

        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] TextMeshProUGUI dialogueText;
        [SerializeField] Image dialogueArrow;
        [SerializeField] RectTransform dialogueBox;
        [SerializeField] RectTransform parentTransform;
        Dialogue currentDialogue;
        [HideInInspector] public bool isRunningDialogue = false;
        bool isSpeaking;
        bool isTapped;
        bool isCutSceneDialogue;
        bool skipSentence = false;
        int sentenceIndex;
        Animator animator, cinematicAnimator;
        int dialogCount;

        PauseManager pauseManager;

        WaitForSeconds charDelay = new WaitForSeconds(0.05f);


        private void Start()
        {
            GameManager.Instance.uiHandler.dialogueUI = this;
            dialogueBox.anchoredPosition += new Vector2(0, (0 - dialogueBox.rect.height));
            InputHandler.InputReceived += OnTap;

            pauseManager = GameManager.Instance.uiHandler.pauseUI;
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

            //                  Debug
            //Debug.Log($"Playing dialog {dialogue.name}");

            if (GameManager.Instance.gameState.ActiveState != GameState.Cinematic && GameManager.Instance.gameState.ActiveState != GameState.Run)
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
            dialogCount = 0;
            StartCoroutine(WriteNextLine());
            if (pauseManager == null)
            {
                yield break;
            }
           pauseManager.HideOpenMenuButton();
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
                    animator.Play(Animator.StringToHash(currentDialogue.sentences[sentenceIndex].anim));
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
            StringBuilder strBuilder = new StringBuilder("");
            skipSentence = false;
            foreach (char letter in currentDialogue.sentences[sentenceIndex].sentence.ToCharArray())
            {
                if (dialogCount == 3)
                {
                    if(currentDialogue.sentences[sentenceIndex].voiceLine != "")
                    AudioManager.Instance.PlaySoundEffect(currentDialogue.sentences[sentenceIndex].voiceLine);
                    animator.gameObject.GetComponent<NPCFaceManager>().RandomizeMouth();
                    dialogCount = 0;
                }
                else
                {
                    dialogCount++;
                }

                if(letter == '\\')
                {
                    strBuilder.Append('\n');
                    dialogueText.text = strBuilder.ToString();
                }
                else
                {
                    strBuilder.Append(letter);
                    dialogueText.text = strBuilder.ToString();
                }

                if (!skipSentence)
                {
                    yield return charDelay;
                }
            }

            NextLineFeedback();
            
            //if(currentDialogue.sentences[sentenceIndex].character == Character.Poppy
            //    || currentDialogue.sentences[sentenceIndex].character == Character.Bergamot
            //    || currentDialogue.sentences[sentenceIndex].character == Character.Thistle)
            //{
            //    animator.Play($"Anim_{currentDialogue.sentences[sentenceIndex].character}_idle");
            //}

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
                    AudioManager.Instance.PlaySoundEffect("UIClick");
                }
                else
                {
                    EndDialogue();
                    AudioManager.Instance.PlaySoundEffect("UIClick");
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

            bool canChange = true;

            if(currentDialogue.bergamotNewState > 0)
            {
                canChange = true;
                if(currentDialogue.bergamotMinimumState != 0 && currentDialogue.bergamotMinimumState <= SaveManager.Instance.state.bergamotState
                    || currentDialogue.poppyMinimumState != 0 && currentDialogue.poppyMinimumState <= SaveManager.Instance.state.poppyState
                    || currentDialogue.thistleMinimumState != 0 && currentDialogue.thistleMinimumState <= SaveManager.Instance.state.thistleState)
                {
                    canChange = false;
                }

                if(canChange)
                {
                    SaveManager.Instance.state.bergamotState = currentDialogue.bergamotNewState;
                    if (GameManager.Instance.gameState.ActiveState == GameState.Hub)
                    {
                        NPCManager.Instance.RefreshNPCs();
                    }
                }

                //if(currentDialogue.bergamotMinimumState >= SaveManager.Instance.state.bergamotState
                //    && currentDialogue.poppyMinimumState >= SaveManager.Instance.state.poppyState
                //    && currentDialogue.thistleMinimumState >= SaveManager.Instance.state.thistleState)
                //{
                //    SaveManager.Instance.state.bergamotState = currentDialogue.bergamotNewState;
                //    if(GameManager.Instance.gameState.ActiveState == GameState.Hub)
                //    {
                //        NPCManager.Instance.RefreshNPCs();
                //    }
                //}
            }
            if (currentDialogue.poppyNewState > 0)
            {
                canChange = true;
                if (currentDialogue.bergamotMinimumState != 0 && currentDialogue.bergamotMinimumState <= SaveManager.Instance.state.bergamotState
                    || currentDialogue.poppyMinimumState != 0 && currentDialogue.poppyMinimumState <= SaveManager.Instance.state.poppyState
                    || currentDialogue.thistleMinimumState != 0 && currentDialogue.thistleMinimumState <= SaveManager.Instance.state.thistleState)
                {
                    canChange = false;
                }

                if (canChange)
                {
                    SaveManager.Instance.state.poppyState = currentDialogue.poppyNewState;
                    if (GameManager.Instance.gameState.ActiveState == GameState.Hub)
                    {
                        NPCManager.Instance.RefreshNPCs();
                    }
                }

                //if (currentDialogue.bergamotMinimumState >= SaveManager.Instance.state.bergamotState
                //    && currentDialogue.poppyMinimumState >= SaveManager.Instance.state.poppyState
                //    && currentDialogue.thistleMinimumState >= SaveManager.Instance.state.thistleState)
                //{
                //    SaveManager.Instance.state.poppyState = currentDialogue.poppyNewState;
                //    if (GameManager.Instance.gameState.ActiveState == GameState.Hub)
                //    {
                //        NPCManager.Instance.RefreshNPCs();
                //    }
                //}
            }
            if (currentDialogue.thistleNewState > 0)
            {
                canChange = true;
                if (currentDialogue.bergamotMinimumState != 0 && currentDialogue.bergamotMinimumState <= SaveManager.Instance.state.bergamotState
                    || currentDialogue.poppyMinimumState != 0 && currentDialogue.poppyMinimumState <= SaveManager.Instance.state.poppyState
                    || currentDialogue.thistleMinimumState != 0 && currentDialogue.thistleMinimumState <= SaveManager.Instance.state.thistleState)
                {
                    canChange = false;
                }

                if (canChange)
                {
                    SaveManager.Instance.state.thistleState = currentDialogue.thistleNewState;
                    if (GameManager.Instance.gameState.ActiveState == GameState.Hub)
                    {
                        NPCManager.Instance.RefreshNPCs();
                    }
                }

                //if (currentDialogue.bergamotMinimumState >= SaveManager.Instance.state.bergamotState
                //    && currentDialogue.poppyMinimumState >= SaveManager.Instance.state.poppyState
                //    && currentDialogue.thistleMinimumState >= SaveManager.Instance.state.thistleState)
                //{
                //    SaveManager.Instance.state.thistleState = currentDialogue.thistleNewState;
                //    if (GameManager.Instance.gameState.ActiveState == GameState.Hub)
                //    {
                //        NPCManager.Instance.RefreshNPCs();
                //    }
                //}
            }
            CloseDialogueBox(currentDialogue);
            NPCFaceManager _face = animator.GetComponent<NPCFaceManager>();
            _face.SetEyesExpression(0);
            _face.SetMouthExpression(0);
            if (currentDialogue.getCoin == true)
            {
                SaveManager.Instance.state.heartCoinAmount++;
                PlayerManager.Instance.currentController.coinUI.UpdateCoinCount();
            }
            currentDialogue = null;
            animator = null;
            isRunningDialogue = false;
            isTapped = false;

            if(SaveManager.Instance.state.poppyState == 26f)
            {
                NPCManager.Instance.poppy.transform.position = NPCManager.Instance.poppy.waypointOutOfVillage.position;
            }

            //                              Debug
            //Debug.Log($"Poppy state after dialog: {SaveManager.Instance.state.poppyState}");
        }

        public void OpenDialogueBox()
        {
            LeanTween.value(gameObject,dialogueBox.anchoredPosition, new Vector2(0,0), 0.5f).setOnUpdate((Vector2 val) => { dialogueBox.anchoredPosition = val;});
        }
        public void CloseDialogueBox(Dialogue currentDialogue)
        {
            LeanTween.value(gameObject, dialogueBox.anchoredPosition, new Vector2(0, 0 - dialogueBox.rect.height), 0.5f).setOnUpdate((Vector2 val) => { dialogueBox.anchoredPosition = val;});

            if (GameManager.Instance.gameState.ActiveState == GameState.Hub)
            {
                if(currentDialogue.sentences[currentDialogue.sentences.Length - 1].activateButtons == false)
                {
                    InteractionManager.Instance.EndInteraction();
                    if (pauseManager is null)
                        return;
                    pauseManager.ShowOpenMenuButton();
                }
                else
                {
                    GameManager.Instance.uiHandler.hubUI.OpenPnjLevelAccess(currentDialogue.sentences[currentDialogue.sentences.Length - 1].levelIndex);
                }
            }
        }

        public void NextLineFeedback()
        {
            CanvasGroup _canvasGroup = dialogueArrow.GetComponent<CanvasGroup>();
            if (_canvasGroup.alpha == 0)
            {
                LeanTween.alphaCanvas(_canvasGroup, 1, 0.5f).setLoopPingPong();
            }
            else
            {
                LeanTween.cancel(dialogueArrow.gameObject);
                LeanTween.alphaCanvas(_canvasGroup, 0, 0f);
            }
        }

    }
}

