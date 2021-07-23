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
using System.Collections.Generic;

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
        [SerializeField] GameObject seeYouButton;
        [SerializeField] GameObject keepTalkingButton;

        Dialogue currentDialogue;
        [HideInInspector] public DialogueInteraction currentInteraction;

        [HideInInspector] public bool isRunningDialogue = false;
        bool isSpeaking;
        bool isTapped;
        bool isCutSceneDialogue;
        bool skipSentence = false;
        bool waitForClick = false;
        int sentenceIndex;
        Animator cinematicAnimator;
        int dialogCount;

        PauseManager pauseManager;

        WaitForSeconds charDelay = new WaitForSeconds(0.05f);

        [Header("Visuals")]
        [SerializeField] Image nameImage;
        [SerializeField] Image backgroundImage;

        [SerializeField] Sprite anaelName;
        [SerializeField] Sprite anaelBackground;

        [SerializeField] Sprite poppyName;
        [SerializeField] Sprite poppyBackground;

        [SerializeField] Sprite thistleName;
        [SerializeField] Sprite thistleBackground;

        [SerializeField] Sprite bergamotName;
        [SerializeField] Sprite bergamotBackground;

        [SerializeField] Sprite darkAnaelName;
        [SerializeField] Sprite darkAnaelBackground;


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
            skipSentence = false;
            sentenceIndex = 0;

            if (currentDialogue.isCutScene)
            {
                StartCoroutine(CutsceneManager.Instance.PauseCutscene());
            }

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
            switch (currentDialogue.sentences[sentenceIndex].character)
            {
                case Character.Anael:
                    nameImage.sprite = anaelName;
                    backgroundImage.sprite = anaelBackground;
                    break;
                case Character.Poppy:
                    nameImage.sprite = poppyName;
                    backgroundImage.sprite = poppyBackground;
                    break;
                case Character.Thistle:
                    nameImage.sprite = thistleName;
                    backgroundImage.sprite = thistleBackground;
                    break;
                case Character.Bergamot:
                    nameImage.sprite = bergamotName;
                    backgroundImage.sprite = bergamotBackground;
                    break;
                case Character.Object:
                    nameImage.sprite = anaelName;
                    backgroundImage.sprite = anaelBackground;
                    break;
                default:
                    Debug.LogError("Missing character case in the switch");
                    break;
            }

            //charDelay = 0.05f; //////
            if(currentDialogue.sentences[sentenceIndex].character == Character.Object)
            {
                nameText.text = "";
            }
            else
            {

                nameText.text = currentDialogue.sentences[sentenceIndex].character.ToString();
                if(NPCManager.characterAnimatorDictionary[currentDialogue.sentences[sentenceIndex].characterAnimator] != null && currentDialogue.sentences[sentenceIndex].anim != "")
                {
                    NPCManager.characterAnimatorDictionary[currentDialogue.sentences[sentenceIndex].characterAnimator].Play(Animator.StringToHash(currentDialogue.sentences[sentenceIndex].anim));
                }
                else if(NPCManager.characterAnimatorDictionary[currentDialogue.sentences[sentenceIndex].characterAnimator] != null)
                {
                    NPCManager.characterAnimatorDictionary[currentDialogue.sentences[sentenceIndex].characterAnimator].SetLayerWeight(NPCManager.characterAnimatorDictionary[currentDialogue.sentences[sentenceIndex].characterAnimator].GetLayerIndex("Talk"), 1);
                }
            }

            HideNextLineFeedback();

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
            isSpeaking = true;
            dialogCount = 0;
            foreach (char letter in currentDialogue.sentences[sentenceIndex].sentence.ToCharArray())
            {
                if(!skipSentence)
                {
                    if (dialogCount == 3)
                    {
                        if(currentDialogue.sentences[sentenceIndex].voiceLine != "")
                        AudioManager.Instance.PlaySoundEffect(currentDialogue.sentences[sentenceIndex].voiceLine);
                        if(currentDialogue.sentences[sentenceIndex].characterAnimator != CharacterAnimator.None)
                            NPCManager.characterAnimatorDictionary[currentDialogue.sentences[sentenceIndex].characterAnimator].gameObject.GetComponent<NPCFaceManager>().RandomizeMouth();
                        dialogCount = 0;
                    }
                    else
                    {
                        dialogCount++;
                    }
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
            ShowNextLineFeedback();
            
            //if(currentDialogue.sentences[sentenceIndex].character == Character.Poppy
            //    || currentDialogue.sentences[sentenceIndex].character == Character.Bergamot
            //    || currentDialogue.sentences[sentenceIndex].character == Character.Thistle)
            //{
            //    animator.Play($"Anim_{currentDialogue.sentences[sentenceIndex].character}_idle");
            //}

            if (NPCManager.characterAnimatorDictionary[currentDialogue.sentences[sentenceIndex].characterAnimator] != null)
            {
                NPCManager.characterAnimatorDictionary[currentDialogue.sentences[sentenceIndex].characterAnimator].SetLayerWeight(NPCManager.characterAnimatorDictionary[currentDialogue.sentences[sentenceIndex].characterAnimator].GetLayerIndex("Talk"), 0);
            }

            sentenceIndex++;
            isSpeaking = false;
            isTapped = false;

            if (GameManager.Instance.gameState.ActiveState == GameState.Hub)
            {
                if(sentenceIndex == currentDialogue.sentences.Length)
                {
                    EndDialogue();
                }
            }
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
                if(waitForClick)
                {
                    waitForClick = false;
                    isRunningDialogue = false;
                    CutsceneManager.Instance.ResumeCutscene();
                    SeeYouButton();
                    return;
                }

                if (isSpeaking)
                {
                    if(!skipSentence)
                        skipSentence = true;
                }
                else if(sentenceIndex < currentDialogue.sentences.Length)
                {
                    StartCoroutine(WriteNextLine());
                    isTapped = false;
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
            //Cache main character current state
            float characterState = 0;
            switch(currentDialogue.mainCharacter)
            {
                case Character.Bergamot:
                    characterState = SaveManager.Instance.state.bergamotState;
                    break;
                case Character.Poppy:
                    characterState = SaveManager.Instance.state.poppyState;
                    break;
                case Character.Thistle:
                    characterState = SaveManager.Instance.state.thistleState;
                    break;
            }

            if(GameManager.Instance.gameState.ActiveState == GameState.Hub/* && CutsceneManager.Instance.mainDirector.playableAsset != null*/)
            {
                InteractionManager.Instance.camTarget.actions--;
                InteractionManager.Instance.playerController.actions--;
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

            bool change = false;
            switch (currentDialogue.mainCharacter)
            {
                case Character.Poppy:
                    if(characterState < SaveManager.Instance.state.poppyState)
                    {
                        change = true;
                    }
                    break;
                case Character.Thistle:
                    if (characterState < SaveManager.Instance.state.thistleState)
                    {
                        change = true;
                    }
                    break;
                case Character.Bergamot:
                    if (characterState < SaveManager.Instance.state.bergamotState)
                    {
                        change = true;
                    }
                    break;
            }




            if (currentDialogue.getCoin == true)
            {
                SaveManager.Instance.state.heartCoinAmount++;
                PlayerManager.Instance.currentController.coinUI.UpdateCoinCount();
            }

            if(SaveManager.Instance.state.poppyState == 26f)
            {
                NPCManager.Instance.poppy.transform.position = NPCManager.Instance.poppy.waypointOutOfVillage.position;
            }

            if(currentDialogue.unlocksLevel)
            {
                SaveManager.Instance.state.unlockedLevels = currentDialogue.levelToUnlockIndex;
                GameManager.Instance.uiHandler.hubUI.InitLevelAccessPanel();
            }


            if(!change)
            {
                if(!currentDialogue.isCutScene)
                {
                    isRunningDialogue = false;
                    seeYouButton.SetActive(true);
                    HideNextLineFeedback();
                }
                else
                {
                    waitForClick = true;
                }

            }
            else
            {
                if (!currentDialogue.isCutScene)
                {
                    //show see you or keep talking buttons
                    isRunningDialogue = false;
                    keepTalkingButton.SetActive(true);
                    seeYouButton.SetActive(true);
                    HideNextLineFeedback();
                }
                else
                {
                    waitForClick = true;
                }
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
                    GameManager.Instance.uiHandler.hubUI.OpenPnjLevelAccess();
                }

                keepTalkingButton.SetActive(false);
                seeYouButton.SetActive(false);
            }
        }

        public void ShowNextLineFeedback()
        {
            CanvasGroup _canvasGroup = dialogueArrow.GetComponent<CanvasGroup>();
            LeanTween.alphaCanvas(_canvasGroup, 1, 0.5f).setLoopPingPong();
        }

        void HideNextLineFeedback()
        {
            CanvasGroup _canvasGroup = dialogueArrow.GetComponent<CanvasGroup>();
            LeanTween.cancel(dialogueArrow.gameObject);
            LeanTween.alphaCanvas(_canvasGroup, 0, 0f);
        }

        public void KeepTalking()
        {
            keepTalkingButton.SetActive(false);
            seeYouButton.SetActive(false);
            StartCoroutine(StartDialogue(currentInteraction.dialogue, currentInteraction.animator));
        }

        public void SeeYouButton()
        {
            CloseDialogueBox(currentDialogue);

            NPCFaceManager _face;
            foreach(KeyValuePair<CharacterAnimator, Animator> entry in NPCManager.characterAnimatorDictionary)
            {
                if (entry.Key == CharacterAnimator.None || entry.Value.GetComponent<NPCFaceManager>() == null)
                    continue;

                _face = entry.Value.GetComponent<NPCFaceManager>();
                _face.SetEyesExpression(0);
                _face.SetMouthExpression(0);
            }

            currentDialogue = null;
            isRunningDialogue = false;
            isTapped = false;
        }

    }
}

