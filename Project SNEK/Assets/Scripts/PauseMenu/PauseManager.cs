﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameManagement;

namespace PauseManagement
{
    /// <summary>
    /// Coco
    /// </summary>
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] GameObject pauseMenu, menuAccessButton, background;

        void Start()
        {
            pauseMenu.transform.localScale = Vector3.zero;
            background.SetActive(false);
        }

        public void OpenPauseMenu()
        {
            Time.timeScale = 0f;
            background.SetActive(true);
            pauseMenu.LeanScale(Vector3.one, 0.2f);
            menuAccessButton.LeanScale(Vector3.zero, 0.2f);
        }

        public void ClosePauseMenu()
        {
            pauseMenu.LeanScale(Vector3.zero, 0.2f);
            menuAccessButton.LeanScale(Vector3.one, 0.2f);
            background.SetActive(false);
        }

        public void ResetTime()
        {
            Time.timeScale = 1f;
        }

    }
}
