﻿using UnityEngine;
using Hub.Interaction;
using DialogueManagement;
using UnityEngine.Timeline;
using Cinematic;

namespace Saving
{
    public abstract class NPCHUB : MonoBehaviour
    {
        public bool started = false;
        [SerializeField] DialogueInteraction dialogueInteraction;
        Clock startTimer;
        float startDelay = 0.5f;

        public virtual void Start()
        {
            startTimer = new Clock(startDelay);
            startTimer.ClockEnded += OnStartDelayPassed;
        }

        void OnStartDelayPassed()
        {
            Refresh();
        }

        private void OnDestroy()
        {
            startTimer.ClockEnded -= OnStartDelayPassed;
        }

        public void SetStarted()
        {
            started = true;
        }

        protected void SetDialogue(Dialogue _dialogue)
        {
            dialogueInteraction.dialogue = _dialogue;
        }

        protected void PlayCutscene(TimelineAsset _cutscene)
        {
            if (started)
                return;
            SetStarted();
            CutsceneManager.Instance.PlayCutscene(_cutscene);
            NPCManager.Instance.RefreshNPCs();
        }

        protected void SetTransform(Transform _transform)
        {
            if (started)
                return;
            transform.position = _transform.position;
            transform.rotation = _transform.rotation;
        }

        public abstract void Refresh();
    }
}