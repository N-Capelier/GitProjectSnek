using UnityEngine;
using UnityEngine.UI;
using GameManagement;
using Saving;
using Hub.Interaction;
using TMPro;
using AudioManagement;
using System.Collections.Generic;
using System.Collections;
using DialogueManagement;
using LetterMailManagement;
using Player;
using PauseManagement;

namespace Hub.UI
{
    /// <summary>
    /// Coco
    /// </summary>
    public class HubUiManager : Singleton<HubUiManager>
    {
        enum BonusStat
        {
            Health,
            Range,
            Power
        }

        [Header("Level Access Menu")]
        [SerializeField] GameObject levelAccessBox;
        [SerializeField] TextMeshProUGUI CoinCountText;
        [Header("PNJ Level Access")]
        [SerializeField] GameObject[] levelBoxPNJ;
        [Space]

        [Header("Letter Animation and Menu")]
        [SerializeField] GameObject letterBox;
        [SerializeField] GameObject letterBoxSelectMenu;
        [SerializeField] GameObject letterBoxAnim;
        [SerializeField] GameObject closeLetterButton;
        [SerializeField] List<GameObject> letterList;
        [SerializeField] TextMeshProUGUI letterText;

        [Space]

        [Header("Fontain Animation and boxes")]
        [SerializeField] GameObject FontainConfirmBox;
        [SerializeField] GameObject FontainLevelUpBox;



        [Header("demoScreen Box")]
        [SerializeField] GameObject demoScreen;

        [Space]

        [Header("Fade background")]
        [SerializeField] CanvasGroup fadeBackground;

        [Space]

        [Header("Sword get Ui")]
        [SerializeField] GameObject swordBox;

        [Header("Hub Tuto UI")]
        [SerializeField] GameObject TutoBox;

        [Header("WIP")]

        [SerializeField] TextMeshProUGUI hearthCoins;
        [SerializeField] TextMeshProUGUI upgradeText;
        [SerializeField] GameObject confirmationBox;

        int cost;
        BonusStat statToUpgrade;

        private void Awake()
        {
            CreateSingleton();
        }

        void Start()
        {

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
            TutoBox.transform.localScale = Vector3.zero;
            TutoBox.SetActive(false);
            foreach(GameObject box in levelBoxPNJ)
            {
                box.transform.localScale = Vector3.zero;
                box.SetActive(false);
            }

            for (int i = 0; i < SaveManager.Instance.state.unlockedLetters; i++)
            {
                letterList[i].SetActive(true);
            }

            //if(SaveManager.Instance.state.bergamotState == 2f)
            //{
            //    OpenTutoBox();
            //}

            if (SaveManager.Instance.state.isDemoFinished)
            {
                OpenBox(demoScreen);
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
            FadeInBackground(.2f);
            levelAccessBox.gameObject.SetActive(true);
            levelAccessBox.transform.LeanScale(Vector3.one, 0.2f);
        }

        public void OpenPnjLevelAccess(int boxIndex)
        {
            AudioManager.Instance.PlaySoundEffect("UIClick");
            FadeInBackground(.2f);
            levelBoxPNJ[boxIndex].SetActive(true);
            levelBoxPNJ[boxIndex].transform.LeanScale(Vector3.one, 0.2f);
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
            closeLetterButton.GetComponent<Button>().interactable = false;
            letterBoxAnim.SetActive(true);
            letterBoxSelectMenu.LeanScale(Vector3.zero, 0.2f);
            letterBoxAnim.transform.localScale = Vector3.one;
            LeanTween.alphaCanvas(letterBoxAnim.GetComponent<CanvasGroup>(), 1f, 3f);
            // Couroutine de dialogue pour afficher le contenu de la lettre 
            StartCoroutine(OpenLetterCoroutine(letterContent));
            // A la fin de la coroutine, affiché le feedback lettre done + clique = activé close letter 
            closeLetterButton.SetActive(true); // L'activer à la fin de l'affichage des lettres 
            LeanTween.alphaCanvas(closeLetterButton.GetComponent<CanvasGroup>(), 1f, 1f).setLoopPingPong().setDelay(4f);
        }

        public void OpenDemoBox()
        {
            demoScreen.SetActive(true);
            demoScreen.LeanScale(Vector3.one, 0.2f);
        }

        public void CloseLetter()
        {
            AudioManager.Instance.PlaySoundEffect("UINone");
            LeanTween.cancel(closeLetterButton);
            LeanTween.alphaCanvas(closeLetterButton.GetComponent<CanvasGroup>(), 0, 0f);
            closeLetterButton.SetActive(false);
            LeanTween.alphaCanvas(letterBoxAnim.GetComponent<CanvasGroup>(), 0f, 1f);
            letterBoxSelectMenu.LeanScale(Vector3.one, 0.5f).setDelay(1.25f);
            letterBoxAnim.LeanScale(Vector3.zero, 0.1f).setDelay(1f).setOnComplete(SetLetterAnimFalse);
            //Reset Text ? No is ok

        }

        public IEnumerator OpenLetterCoroutine(LetterMail letterContent)
        {
            letterText.text = "";

            foreach(char letter in letterContent.text.ToCharArray())
            {
                letterText.text += letter;
                yield return new WaitForSeconds(0.005f);
            }
            if(SaveManager.Instance.state.bergamotState == 3f)
            {
                OpenSwordBox();
                SaveManager.Instance.state.bergamotState = 4f;
                NPCManager.Instance.RefreshNPCs();
            }
            closeLetterButton.GetComponent<Button>().interactable = true;
        }

        public void OpenLetterBox()
        {
            AudioManager.Instance.PlaySoundEffect("UIClick");
            letterBox.SetActive(true);
            letterBox.transform.LeanScale(Vector3.one, 0.2f);
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

        public void OpenTutoBox()
        {
            TutoBox.SetActive(true);
            TutoBox.transform.LeanScale(Vector3.one, 0.2f);
        }

        public void CloseTutoBox()
        {
            TutoBox.LeanScale(Vector3.zero, 0.2f).setOnComplete(SetTutoBoxFalse);
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
            PlayerManager.Instance.currentController.animator.Play("Anim_PlayerHub_TakeOutCoin");
            PlayerManager.Instance.currentController.coinAnimator.Play("Anim_ObjectCoin_TakeOut");
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
            PlayerManager.Instance.currentController.animator.Play("Anim_Playerhub_PutBackCoin");
            PlayerManager.Instance.currentController.coinAnimator.Play("Anim_ObjectCoin_PutBack");
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
            PlayerManager.Instance.currentController.animator.Play("Anim_PlayerHub_ThrowCoin");
            PlayerManager.Instance.currentController.coinAnimator.Play("Anim_ObjectCoin_Throw");
            FontainConfirmBox.transform.LeanScale(Vector3.zero, 0.2f).setOnComplete(SetFontainConfirmBoxFalse);
            yield return new WaitForSeconds(4.6f);
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
                PauseManager.Instance.HideOpenMenuButton();
            }
            else
            {
                PauseManager.Instance.ShowOpenMenuButton();
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
            TutoBox.SetActive(false);
        }
    }
}

