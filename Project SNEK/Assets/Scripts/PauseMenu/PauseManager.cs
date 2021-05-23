﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameManagement;
using Hub.Interaction;
using AudioManagement;
using Saving;

namespace PauseManagement
{
    /// <summary>
    /// Coco
    /// </summary>
    public class PauseManager : Singleton<PauseManager>
    {
        [SerializeField] GameObject pauseMenu, openPauseMenu;
        [SerializeField] GameObject[] qualityToggles;
        [SerializeField] GameObject background;
        [SerializeField] Slider soundSlider, musicSlider;
        bool occupied = false;

        private void Awake()
        {
            CreateSingleton();
        }

        void Start()
        {
            pauseMenu.transform.localScale = Vector3.zero;
            soundSlider.value = SaveManager.Instance.state.soundVolume;
            musicSlider.value = SaveManager.Instance.state.musicVolume;
            pauseMenu.SetActive(false);
            ManageQualitySettings(SaveManager.Instance.state.quality);
            FadeBackground(false);
        }

        public void OpenPauseMenu()
        {
            pauseMenu.SetActive(true);
            pauseMenu.LeanScale(Vector3.one, 0.2f).setIgnoreTimeScale(true);
            openPauseMenu.GetComponent<CanvasGroup>().LeanAlpha(0,0.2f).setIgnoreTimeScale(true).setOnComplete(SetOpenPauseFalse);
            Time.timeScale = 0f;
        }

        public void ClosePauseMenu()
        {
            Time.timeScale = 1f;
            pauseMenu.LeanScale(Vector3.zero, 0.2f).setIgnoreTimeScale(true).setOnComplete(SetPauseMenuFalse);
            openPauseMenu.SetActive(true);
            openPauseMenu.GetComponent<CanvasGroup>().LeanAlpha(1, 0.2f).setIgnoreTimeScale(true);
        }

        public void FadeBackground(bool state)
        {
            if(state == true)
            {
                background.SetActive(true);
                background.GetComponent<CanvasGroup>().LeanAlpha(1, 0.2f).setIgnoreTimeScale(true);
            }
            else
            {
                background.GetComponent<CanvasGroup>().LeanAlpha(0, 0.2f).setIgnoreTimeScale(true).setOnComplete(SetBackgroundFalse);
            }
        }

        public void HideOpenMenuButton()
        {
            occupied = true;
            openPauseMenu.GetComponent<CanvasGroup>().LeanAlpha(0, 0.2f).setIgnoreTimeScale(true).setOnComplete(SetOpenPauseFalse);

        }

        public void ShowOpenMenuButton()
        {
            occupied = false;
            openPauseMenu.SetActive(true);
            openPauseMenu.GetComponent<CanvasGroup>().LeanAlpha(1, 0.2f).setIgnoreTimeScale(true);
        }

        void SetBackgroundFalse()
        {
            background.SetActive(false);
        }

        void SetOpenPauseFalse()
        {
            openPauseMenu.SetActive(false);
        }

        void SetPauseMenuFalse()
        {
            pauseMenu.SetActive(false);
        }

        public void ResetTime()
        {
            Time.timeScale = 1f;
        }

        public void ManageQualitySettings(int toggleIndex = 0)
        {
            QualitySettings.SetQualityLevel(toggleIndex);

            for (int i = 0; i < qualityToggles.Length; i++)
            {
                if(i != toggleIndex)
                {
                    qualityToggles[i].SetActive(false);
                }
                else
                {
                    qualityToggles[i].SetActive(true);
                }
            }

            SaveManager.Instance.state.quality = toggleIndex;
            SaveManager.Instance.Save();
        }

        public void QuitOrHub()
        {
            if(GameManager.Instance.gameState.ActiveState == GameManagement.GameState.Hub)
            {
                Application.Quit();
            }
            else
            {
                GameManager.Instance.gameState.Set(GameManagement.GameState.Hub, "Hub");
                ResetTime();
            }
        }

        public void SetSoundVolume(bool music)
        {
            if (music == true)
            {
                AudioManager.Instance.MusicsVolume = musicSlider.value;
                SaveManager.Instance.state.musicVolume = musicSlider.value;
                SaveManager.Instance.Save();
                AudioManager.Instance.UpdateSliders();
            }
            else
            {
                AudioManager.Instance.SoundEffectsVolume = soundSlider.value;
                SaveManager.Instance.state.soundVolume = soundSlider.value;
                SaveManager.Instance.Save();
                AudioManager.Instance.UpdateSliders();
            }
        }


    }
}

