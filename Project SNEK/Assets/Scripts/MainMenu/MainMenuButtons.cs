﻿using UnityEngine;
using Saving;
using AudioManagement;
using GameManagement;
using System.Collections;
using PauseManagement;

namespace MainMenu
{
    /// <summary>
    /// Nico
    /// </summary>
    public class MainMenuButtons : MonoBehaviour
    {
        public void LaunchGame()
        {
            AudioManager.Instance.PlaySoundEffect("UIConfirm");
            if (SaveManager.Instance.state.bergamotState > 1)
            {
                GameManager.Instance.gameState.Set(GameManager.Instance.gameState.ActiveState, "Hub");
                //SceneManager.LoadScene("Hub");
            }
            else
            {
                GameManager.Instance.gameState.Set(GameManager.Instance.gameState.ActiveState, "TutorialIntro");
                //SceneManager.LoadScene("TutorialIntro");
            }
        }

        public void ResetSave()
        {
            AudioManager.Instance.PlaySoundEffect("UIClick");
            Debug.Log("Reseting Save");
            SaveManager.Instance.state = new SaveState();
            PauseManager.Instance.SetSoundVolume(false);
            PauseManager.Instance.SetSoundVolume(true);
            SaveManager.Instance.Save();

            LaunchGame();
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void OpenBox(GameObject box)
        {
            box.SetActive(true);
            box.transform.localScale = Vector3.zero;
            box.transform.LeanScale(Vector3.one, 0.2f);
        }
        public void CloseBox(GameObject box)
        {
            box.transform.localScale = Vector3.zero;
            box.SetActive(false);
        }

        public void FadeIn(GameObject fade)
        {
            fade.SetActive(true);
            fade.GetComponent<CanvasGroup>().LeanAlpha(1, 0.3f);
        }

        public void FadeOut(GameObject fade)
        {
            StartCoroutine(FadeOutAnim(fade));
        }

        IEnumerator FadeOutAnim(GameObject fade)
        {
            fade.GetComponent<CanvasGroup>().LeanAlpha(0, 0.3f);
            yield return new WaitForSeconds(0.35f);
            fade.SetActive(false);
        }
    }
}