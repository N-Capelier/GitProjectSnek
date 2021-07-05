using UnityEngine;
using Hub.Interaction;
using DialogueManagement;
using UnityEngine.Timeline;
using Cinematic;
using System.Collections;
using System.Collections.Generic;

namespace Saving
{
    public enum NPCStateType
    {
        None,
        Cutscene,
        Dialogue,
    }

    [System.Serializable]
    public struct NPCState
    {
        [Header("State info")]
        public int stateID;
        [Space]
        [Header("State content")]
        public bool notInVillage;
        public Transform waypoint;
        [Space]
        public NPCStateType stateType;
        public TimelineAsset cutscene;
        public Dialogue dialogue;
        [Space]
        [Header("Additionnal set")]
        public bool setNewStates;
        public float newBergamotState;
        public float newPoppyState;
        public float newThistleState;
    }

    public abstract class NPCHUB : MonoBehaviour
    {
        [HideInInspector] public bool started = false;
        [SerializeField] DialogueInteraction dialogueInteraction;
        Clock startTimer;
        float startDelay = 1f;

        [Space]
        [SerializeField] protected Transform waypointOutOfVillage = null;
        [Space]
        [SerializeField] protected List<NPCState> npcStates = new List<NPCState>();

        public virtual IEnumerator Start()
        {
            yield return new WaitForSeconds(0.1f);
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
            StartCoroutine(DoPlayCutscene(_cutscene));
        }

        IEnumerator DoPlayCutscene(TimelineAsset _cutscene)
        {
            if (started)
                yield break;
            SetStarted();
            yield return new WaitUntil(() => CutsceneManager.Instance != null);
            yield return new WaitUntil(() => InteractionManager.Instance != null);
            CutsceneManager.Instance.PlayCutscene(_cutscene);
            //NPCManager.Instance.RefreshNPCs();
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