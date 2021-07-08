using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PauseManagement;
using GameManagement;

namespace Tutorial
{
    public class TutorialUIManager : MonoBehaviour
    {
        [SerializeField] GameObject[] tutorialUis;
        PauseManager pauseManager;

        void Start()
        {
            GameManager.Instance.uiHandler.tutorialUI = this;
            foreach (GameObject ui in tutorialUis)
            {
                ui.transform.localScale = Vector3.zero;
                ui.SetActive(false);
            }
            pauseManager = GameManager.Instance.uiHandler.pauseUI;
        }

        public void ActivateTutorialUI(int index)
        {
            tutorialUis[index].SetActive(true);
            tutorialUis[index].LeanScale(Vector3.one * 1.2f, 0.2f).setIgnoreTimeScale(true);
            if(pauseManager != null)
            {
                pauseManager.HideOpenMenuButton();
            }
            //Ajouter l'index à une liste pour empecher la répétition ?
            Time.timeScale = 0f;
        }

        public void DeactivateTutorialUI(int index)
        {
            StartCoroutine(UiDeactivation(index));
        }

        IEnumerator UiDeactivation(int index)
        {
            tutorialUis[index].LeanScale(Vector3.zero, 0.2f).setIgnoreTimeScale(true);
            Time.timeScale = 1f;
            yield return new WaitForSeconds(0.2f);
            if (pauseManager != null)
            {
                pauseManager.ShowOpenMenuButton();
            }
            tutorialUis[index].SetActive(false);
        }
    }
}

