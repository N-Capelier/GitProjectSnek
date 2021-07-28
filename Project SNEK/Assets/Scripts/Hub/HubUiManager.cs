using UnityEngine;
using UnityEngine.UI;
using GameManagement;
using Saving;
using Hub.Interaction;
using TMPro;
using AudioManagement;
using System.Collections.Generic;
using System.Collections;
using LetterMailManagement;
using Player;
using PauseManagement;

namespace Hub.UI
{
    /// <summary>
    /// Coco
    /// </summary>
    public class HubUiManager : MonoBehaviour
    {
        [Header("Level Access Menu")]
        [SerializeField] GameObject levelAccessBox;
        [SerializeField] RectTransform scrollviewContent;
        [SerializeField] TextMeshProUGUI CoinCountText;
        [SerializeField] List<GameObject> levels = new List<GameObject>();
        [SerializeField] GameObject slider;

        [Header("PNJ Level Access")]
        [SerializeField] GameObject levelBoxPNJ;
        [Space]

        [Header("Letter Animation and Menu")]
        [SerializeField] public GameObject letterBox;
        [SerializeField] GameObject letterBoxSelectMenu;
        [SerializeField] GameObject letterBoxAnim;
        [SerializeField] GameObject closeLetterButton;
        public GameObject newLetterFeedback;
        [SerializeField] List<GameObject> letterList;
        [SerializeField] List<LetterMail> letterSOList;
        [SerializeField] TextMeshProUGUI letterText;
        [SerializeField] Sprite readLetter;
        [SerializeField] Sprite notReadLetter;

        WaitForSeconds charDelay = new WaitForSeconds(0.005f);
        Coroutine letterCoroutine;
        LetterMail currentContent;
        bool skipText;
        bool writingLetter;

        [Space]

        [Header("Fontain Animation and boxes")]
        [SerializeField] GameObject FontainConfirmBox;
        [SerializeField] GameObject FontainLevelUpBox;
        [SerializeField] ParticleSystem upgradeParticule;


        [Header("demoScreen Box")]
        [SerializeField] GameObject demoScreen;

        [Space]

        [Header("Fade background")]
        [SerializeField] CanvasGroup fadeBackground;

        [Space]

        [Header("Sword get Ui")]
        [SerializeField] GameObject swordBox;

        [Header("Hub Tuto UI")]
        [SerializeField] List<GameObject> tutoBoxes = new List<GameObject>();


        PauseManager pauseManager;

        void Start()
        {
            GameManager.Instance.uiHandler.hubUI = this;
            pauseManager = GameManager.Instance.uiHandler.pauseUI;

            levelAccessBox.transform.localScale = Vector3.zero;
            levelAccessBox.SetActive(false);
            letterBox.transform.localScale = Vector3.zero;
            letterBox.SetActive(false);
            demoScreen.transform.localScale = Vector3.zero;
            demoScreen.SetActive(false);
            demoScreen.SetActive(false);
            LeanTween.alphaCanvas(letterBoxAnim.GetComponent<CanvasGroup>(), 0f, 0f);
            letterBoxAnim.transform.localScale = Vector3.zero;
            letterBoxAnim.SetActive(false);
            FontainConfirmBox.transform.localScale = Vector3.zero;
            letterBoxAnim.SetActive(false);
            FontainLevelUpBox.transform.localScale = Vector3.zero;
            letterBoxAnim.SetActive(false);
            swordBox.transform.localScale = Vector3.zero;
            swordBox.SetActive(false);

            for (int i = 0; i < tutoBoxes.Count; i++)
            {
                tutoBoxes[i].transform.localScale = Vector3.zero;
                tutoBoxes[i].SetActive(false);
            }



            levelBoxPNJ.transform.localScale = Vector3.zero;
            levelBoxPNJ.SetActive(false);


            //Set read letter based on save state
            for (int i = 0; i < letterSOList.Count; i++)
            {
                letterSOList[i].read = false;
            }

            for (int i = 0; i < SaveManager.Instance.state.readLetters.Count; i++)
            {
                letterSOList[SaveManager.Instance.state.readLetters[i]].read = true;
            }



            //if(SaveManager.Instance.state.bergamotState == 2f)
            //{
            //    OpenTutoBox();
            //}

            if (SaveManager.Instance.state.isDemoFinished)
            {
                OpenBox(demoScreen);
            }

            InitLevelAccessPanel();
        }

        public void InitLetterBoxFeedback()
        {            
            //set active new message feedback
            if (SaveManager.Instance.state.unlockedLetters > SaveManager.Instance.state.readLetters.Count)
            {
                newLetterFeedback.SetActive(true);
            }
            else
            {
                newLetterFeedback.SetActive(false);
            }
        }

        public void InitLevelAccessPanel()
        {
            

            for (int i = 0; i < levels.Count; i++)
            {
                levels[i].SetActive(false);
            }

            int temp = SaveManager.Instance.state.unlockedLevels;

            if (temp == 1)
            {
                slider.SetActive(false);
            }
            else
            {
                slider.SetActive(false);
            }

            for (int i = 0; i < temp; i++)
            {
                levels[i].SetActive(true);
            }
        }

        public void UpdateLetterSprites()
        {
            //set Sprite of read letters
            for (int i = 0; i < SaveManager.Instance.state.unlockedLetters; i++)
            {
                letterList[i].SetActive(true);
                if (letterSOList[i].read)
                {
                    letterList[i].GetComponent<Image>().sprite = readLetter;
                }
                else
                {
                    letterList[i].GetComponent<Image>().sprite = notReadLetter;
                }
            }
        }

        public void OpenBox(GameObject box)
        {
            box.LeanScale(Vector3.one, 0.2f);
            //upgradeText.text = $"Pay {cost} heart coins to upgrade this skill?";
        }

        public void CloseBox(GameObject box)
        {
            AudioManager.Instance.PlaySoundEffect("UINone");
            box.LeanScale(Vector3.zero, 0.2f);
        }

        public void OpenLevelAccess()
        {
            AudioManager.Instance.PlaySoundEffect("UIClick");
            scrollviewContent.offsetMin = Vector2.zero;
            FadeInBackground(.2f);
            levelAccessBox.gameObject.SetActive(true);
            levelAccessBox.transform.LeanScale(Vector3.one, 0.2f);
        }

        public void OpenPnjLevelAccess()
        {
            AudioManager.Instance.PlaySoundEffect("UIClick");
            FadeInBackground(.2f);
            levelBoxPNJ.SetActive(true);
            levelBoxPNJ.transform.LeanScale(Vector3.one, 0.2f);
        }

        public void CloseLevelAcces()
        {
            AudioManager.Instance.PlaySoundEffect("UINone");
            FadeOutBackground(.2f);
            levelAccessBox.transform.LeanScale(Vector3.zero, 0.2f).setOnComplete(SetLevelAccessFalse); 
            if (GameManager.Instance.gameState.ActiveState == GameState.Hub)
            {
                InteractionManager.Instance.EndInteraction();
            }
        }

        public void ClosePNJLevelAcces(GameObject box)
        {
            AudioManager.Instance.PlaySoundEffect("UINone");
            FadeOutBackground(.2f);
            box.transform.LeanScale(Vector3.zero, 0.2f).setOnComplete(SetLevelAccessFalse);
            if (GameManager.Instance.gameState.ActiveState == GameState.Hub)
            {
                InteractionManager.Instance.EndInteraction();
            }
        }

        public void OpenLetter(LetterMail letterContent)
        {
            currentContent = letterContent;
            //closeLetterButton.GetComponent<Button>().interactable = false;
            letterBoxAnim.SetActive(true);
            letterBoxSelectMenu.LeanScale(Vector3.zero, 0.2f);
            letterBoxAnim.transform.localScale = Vector3.one;
            LeanTween.alphaCanvas(letterBoxAnim.GetComponent<CanvasGroup>(), 1f, 3f);
            // Couroutine de dialogue pour afficher le contenu de la lettre 
            letterCoroutine = StartCoroutine(OpenLetterCoroutine(letterContent));
        }

        public void OpenDemoBox()
        {
            demoScreen.SetActive(true);
            demoScreen.LeanScale(Vector3.one, 0.2f);
        }

        public void CloseLetter()
        {
                FadeOutBackground(0.5f);

                AudioManager.Instance.PlaySoundEffect("UINone");
                LeanTween.cancel(closeLetterButton);
                LeanTween.alphaCanvas(closeLetterButton.GetComponent<CanvasGroup>(), 0, 0f);
                closeLetterButton.SetActive(false);
                LeanTween.alphaCanvas(letterBoxAnim.GetComponent<CanvasGroup>(), 0f, 1f);
                letterBoxSelectMenu.LeanScale(Vector3.one, 0.5f).setDelay(1.25f);
                letterBoxAnim.LeanScale(Vector3.zero, 0.1f).setDelay(1f).setOnComplete(SetLetterAnimFalse);

        }

        public IEnumerator OpenLetterCoroutine(LetterMail letterContent)
        {
            letterText.text = "";
            skipText = false;
            writingLetter = true;
            foreach (char letter in letterContent.text.ToCharArray())
            {
                if (letter == '\\')
                {
                    letterText.text += "\n";
                }
                else
                {
                    letterText.text += letter;
                }         

                if(!skipText)
                {
                    yield return charDelay;
                }
            }
            if(SaveManager.Instance.state.bergamotState == 3f)
            {
                OpenSwordBox();
                SaveManager.Instance.state.bergamotState = 4f;
                NPCManager.Instance.RefreshNPCs();
            }
            //closeLetterButton.GetComponent<Button>().interactable = true;

            // A la fin de la coroutine, affiché le feedback lettre done + clique = activé close letter 
            closeLetterButton.SetActive(true); // L'activer à la fin de l'affichage des lettres 
            LeanTween.alphaCanvas(closeLetterButton.GetComponent<CanvasGroup>(), 1f, 1f).setLoopPingPong();
            writingLetter = false;

            if(!letterContent.read)
            {
                SaveManager.Instance.state.readLetters.Add(letterContent.letterIndex);
                letterContent.read = true;
            }

            UpdateLetterSprites();
            InitLetterBoxFeedback();
        }

        public void OpenLetterBox()
        {
            AudioManager.Instance.PlaySoundEffect("UIClick");
            letterBox.SetActive(true);
            letterBox.transform.LeanScale(Vector3.one, 0.2f);
            UpdateLetterSprites();
        }

        public void OpenSwordBox()
        {
            AudioManager.Instance.PlaySoundEffect("UIClick");
            swordBox.SetActive(true);
            swordBox.transform.LeanScale(Vector3.one, 0.2f);
        }

        public void CloseSwordBox()
        {
            AudioManager.Instance.PlaySoundEffect("UINone");
            swordBox.transform.localScale = Vector3.zero;
            swordBox.SetActive(false);

        }

        public void OpenTutoBox(int index)
        {
            tutoBoxes[index].SetActive(true);
            tutoBoxes[index].transform.LeanScale(Vector3.one, 0.2f);
        }

        public void CloseTutoBox(int index)
        {
            tutoBoxes[index].LeanScale(Vector3.zero, 0.2f).setOnComplete(SetTutoBoxFalse);
        }

        public void CloseLetterBox()
        {
            AudioManager.Instance.PlaySoundEffect("UINone");
            letterBox.transform.LeanScale(Vector3.zero, 0.2f).setOnComplete(SetLetterBoxFalse);
            if (GameManager.Instance.gameState.ActiveState == GameState.Hub)
            {
                InteractionManager.Instance.EndInteraction();
            }
        }

        
        public void OpenFountainBox()
        {
            AudioManager.Instance.PlaySoundEffect("UIClick");
            PlayerManager.Instance.currentController.animator.Play(Animator.StringToHash("Anim_PlayerHub_TakeOutCoin"));
            PlayerManager.Instance.currentController.coinAnimator.Play(Animator.StringToHash("Anim_ObjectCoin_TakeOut"));
            FontainConfirmBox.SetActive(true);
            CoinCountText.text = "x" + SaveManager.Instance.state.heartCoinAmount.ToString();
            switch (SaveManager.Instance.state.powerLevel)
            {
                case 0:
                    FontainConfirmBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Do you want to pay " + 1 + " heart coins to improve yourself ?";
                    break;
                case 1:
                    FontainConfirmBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Do you want to pay " + 2 + " heart coins to improve yourself ?";
                    break;
                case 2:
                    FontainConfirmBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Do you want to pay " + 3 + " heart coins to improve yourself ?";
                    break;
                case 3:
                    FontainConfirmBox.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Do you want to pay " + 4 + " heart coins to improve yourself ?";
                    break;
            }
            FontainConfirmBox.transform.LeanScale(Vector3.one, 0.2f);
        }

        public void CloseFountainBox()
        {
            AudioManager.Instance.PlaySoundEffect("UINone");
            PlayerManager.Instance.currentController.animator.Play(Animator.StringToHash("Anim_Playerhub_PutBackCoin"));
            PlayerManager.Instance.currentController.coinAnimator.Play(Animator.StringToHash("Anim_ObjectCoin_PutBack"));
            FontainConfirmBox.transform.LeanScale(Vector3.zero, 0.2f).setOnComplete(SetFontainConfirmBoxFalse);
            if (GameManager.Instance.gameState.ActiveState == GameState.Hub)
            {
                InteractionManager.Instance.EndInteraction();
            }
        }

        public void PlayFountainAnim()
        {
            switch (SaveManager.Instance.state.powerLevel)
            {
                case 0:
                    if (SaveManager.Instance.state.heartCoinAmount >= 1)
                    {
                        StartCoroutine(FountainAnim(0));
                    }
                    break;
                case 1:
                    if(SaveManager.Instance.state.heartCoinAmount >= 2)
                    {
                        StartCoroutine(FountainAnim(1));
                    }
                    break;
                case 3:
                    if (SaveManager.Instance.state.heartCoinAmount >= 3)
                    {
                        StartCoroutine(FountainAnim(3));
                    }
                    break;
                case 4:
                    if (SaveManager.Instance.state.heartCoinAmount >= 4)
                    {
                        StartCoroutine(FountainAnim(4));
                    }
                    break;
            }

                   
        }

        public IEnumerator FountainAnim(int level)
        {
            PlayerManager.Instance.currentController.animator.Play(Animator.StringToHash("Anim_PlayerHub_ThrowCoin"));
            PlayerManager.Instance.currentController.coinAnimator.Play(Animator.StringToHash("Anim_ObjectCoin_Throw"));
            FontainConfirmBox.transform.LeanScale(Vector3.zero, 0.2f).setOnComplete(SetFontainConfirmBoxFalse);
            yield return new WaitForSeconds(3f);
            Instantiate(upgradeParticule, PlayerManager.Instance.currentController.transform.GetChild(0).transform.position, Quaternion.identity);
            AudioManager.Instance.PlaySoundEffect("PlayerLevelUp");
            yield return new WaitForSeconds(1.6f);
            switch (level)
            {
                case 0:
                    SaveManager.Instance.state.heartCoinAmount -= 1;
                    SaveManager.Instance.state.spentHeartCoinAmount++;
                    SaveManager.Instance.state.powerLevel++;
                    SaveManager.Instance.state.bonusHealth = 2;
                    SaveManager.Instance.state.bonusRange = 1f;
                    SaveManager.Instance.Save();
                    break;
                case 1:
                    SaveManager.Instance.state.heartCoinAmount -= 2;
                    SaveManager.Instance.state.spentHeartCoinAmount += 2;
                    SaveManager.Instance.state.powerLevel++;
                    SaveManager.Instance.state.bonusHealth = 3;
                    SaveManager.Instance.state.bonusRange = 1.15f;
                    SaveManager.Instance.Save();
                    break;
                case 2:
                    SaveManager.Instance.state.heartCoinAmount -= 3;
                    SaveManager.Instance.state.spentHeartCoinAmount += 3;
                    SaveManager.Instance.state.powerLevel++;
                    SaveManager.Instance.state.bonusHealth = 4;
                    SaveManager.Instance.state.bonusRange = 1.25f;
                    SaveManager.Instance.Save();
                    break;
                case 3:
                    SaveManager.Instance.state.heartCoinAmount -= 4;
                    SaveManager.Instance.state.spentHeartCoinAmount += 4;
                    SaveManager.Instance.state.powerLevel++;
                    SaveManager.Instance.state.bonusHealth = 5;
                    SaveManager.Instance.state.bonusRange = 1.4f;
                    SaveManager.Instance.Save();
                    break;
            }
            CoinCountText.text = "x" + SaveManager.Instance.state.heartCoinAmount.ToString();
            FontainLevelUpBox.SetActive(true);
            FontainLevelUpBox.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "You are now level " + SaveManager.Instance.state.powerLevel + " . You gain both attack range and vitality";
            FontainLevelUpBox.LeanScale(Vector3.one, 0.2f);
        }

        public void CloseFountainLevelupBox()
        {
            AudioManager.Instance.PlaySoundEffect("UIClick");
            FontainLevelUpBox.LeanScale(Vector3.one, 0.2f).setOnComplete(SetFontainLevelUpBoxFalse);
            if (GameManager.Instance.gameState.ActiveState == GameState.Hub)
            {
                InteractionManager.Instance.EndInteraction();
            }
        }

        public void SetOccupied(bool state)
        {
            if(state == true)
            {
               pauseManager.HideOpenMenuButton();
            }
            else
            {
                pauseManager.ShowOpenMenuButton();
            }

        }

        public void FadeInBackground(float duration)
        {
            LeanTween.alphaCanvas(fadeBackground, 0.8f, duration);
        }

        public void FadeOutBackground(float duration)
        {
            LeanTween.alphaCanvas(fadeBackground, 0f, duration);
        }

        void SetLetterAnimFalse()
        {
            letterBoxAnim.SetActive(false);
        }

        void SetLetterBoxFalse()
        {
            letterBox.SetActive(false);
        }

        void SetFontainConfirmBoxFalse()
        {
            FontainConfirmBox.SetActive(false);
        }

        void SetFontainLevelUpBoxFalse()
        {
            FontainLevelUpBox.SetActive(false);
        }

        public void SetLevelAccessFalse()
        {
            levelAccessBox.SetActive(false);
        }

        public void SetTutoBoxFalse()
        {
            for (int i = 0; i < tutoBoxes.Count; i++)
            {
                tutoBoxes[i].SetActive(false);
            }
        }

        public void Skip()
        {
            skipText = true;
        }
    }
}

