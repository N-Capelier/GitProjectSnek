using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    public class TutorialUIManager : Singleton<TutorialUIManager>
    {
        [SerializeField] GameObject[] tutorialUis;

        private void Awake()
        {
            CreateSingleton();
        }

        void Start()
        {
            foreach (GameObject ui in tutorialUis)
            {
                ui.transform.localScale = Vector3.zero;
                ui.SetActive(false);
            }
        }

        public void ActivateTutorialUI(int index)
        {
            tutorialUis[index].SetActive(true);
            tutorialUis[index].LeanScale(Vector3.one, 0.2f).setIgnoreTimeScale(true);
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
            tutorialUis[index].SetActive(false);
        }
    }
}

