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

        [Space]

        [Header("Letter Animation and Menu")]
        [SerializeField] GameObject letterBox;
        [SerializeField] GameObject letterBoxSelectMenu;
        [SerializeField] GameObject letterBoxAnim;
        [SerializeField] GameObject closeLetterButton;
        [SerializeField] List<GameObject> letterList;

        [Space]

        [Header("demoScreen Box")]
        [SerializeField] GameObject demoScreen;

        [Space]

        [Header("Fade background")]
        [SerializeField] CanvasGroup fadeBackground;

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
            LeanTween.alphaCanvas(letterBoxAnim.GetComponent<CanvasGroup>(),0f,0f);
            letterBoxAnim.transform.localScale = Vector3.zero;
            letterBoxAnim.SetActive(false);

            if(SaveManager.Instance.state.isDemoFinished)
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
            FadeInBackground(0.2f);
            levelAccessBox.gameObject.SetActive(true);
            levelAccessBox.transform.LeanScale(Vector3.one, 0.2f);
        }

        public void CloseLevelAcces()
        {
            AudioManager.Instance.PlaySoundEffect("UINone");
            FadeOutBackground(0.2f);
            levelAccessBox.transform.LeanScale(Vector3.zero, 0.2f).setOnComplete(SetLevelAccessFalse); 
            if (GameManager.Instance.gameState.ActiveState == GameState.Hub)
            {
                InteractionManager.Instance.EndInteraction();
            }
        }

        public void OpenLetterBox()
        {
            AudioManager.Instance.PlaySoundEffect("UIClick");
            letterBox.SetActive(true);
            letterBox.transform.LeanScale(Vector3.one, 0.2f);
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

        public void OpenLetter(Dialogue letterContent)
        {
            letterBoxAnim.SetActive(true);
            letterBoxSelectMenu.LeanScale(Vector3.zero, 0.2f);
            letterBoxAnim.transform.localScale = Vector3.one;
            LeanTween.alphaCanvas(letterBoxAnim.GetComponent<CanvasGroup>(),1f, 3f);
            // Couroutine de dialogue pour afficher le contenu de la lettre
            // A la fin de la coroutine, affiché le feedback lettre done + clique = activé close letter
            closeLetterButton.SetActive(true); // L'activer à la fin de l'affichage des lettres
            LeanTween.alphaCanvas(closeLetterButton.GetComponent<CanvasGroup>(), 1f, 1f).setLoopPingPong().setDelay(4f);
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
            //Reset Text ?

        }

        public void FadeInBackground(float duration)
        {
            LeanTween.alphaCanvas(fadeBackground, 0.4f,duration);
        }

        public void FadeOutBackground(float duration)
        {
            LeanTween.alphaCanvas(fadeBackground, 0f, duration);
        }

        void SetLetterBoxFalse()
        {
            letterBox.SetActive(false);
        }
        void SetLetterAnimFalse()
        {
            letterBoxAnim.SetActive(false);
        }
        void SetLevelAccessFalse()
        {
            levelAccessBox.SetActive(false);
        }
    }
}

