using UnityEngine;
using Hub.Interaction;
using DialogueManagement;
using UnityEngine.Timeline;
using Cinematic;

namespace Saving
{
    public abstract class NPCHUB : MonoBehaviour
    {
        protected bool started = false;
        [SerializeField] DialogueInteraction dialogueInteraction;

        protected void SetDialogue(Dialogue _dialogue)
        {
            dialogueInteraction.dialogue = _dialogue;
        }

        protected void PlayCutscene(TimelineAsset _cutscene)
        {
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