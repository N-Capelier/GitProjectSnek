using UnityEngine;
using System.Collections.Generic;
using Player;
using Cinematic;
using UnityEngine.Timeline;

namespace Map
{
    /// <summary>
    /// Nico
    /// </summary>
    public class CheckPointBehaviour : MonoBehaviour
    {
        bool hasChecked = false;
        public List<ParticleSystem> lanterFlameParticules = new List<ParticleSystem>();
        public List<ParticleSystem> lanterGlowParticules = new List<ParticleSystem>();
        public ParticleSystem conffeti;

        [SerializeField] bool useOtherPos = false;
        public Transform otherPos = null;
        [SerializeField] TimelineAsset cutscene;    

        private void OnTriggerEnter(Collider other)
        {
            if (!hasChecked && other.CompareTag("Player"))
            {
                hasChecked = true;

                if(useOtherPos)
                {
                    PlayerManager.Instance.currentController.checkPoint = otherPos.transform;
                    PlayerManager.Instance.currentController.respawnNode = otherPos.transform.position;

                    //start cinematic
                    CutsceneManager.Instance.PlayCutscene(cutscene);
                }
                else
                {
                    PlayerManager.Instance.currentController.checkPoint = transform;
                    PlayerManager.Instance.currentController.respawnNode = transform.position;
                }

                conffeti.Play();
                foreach (ParticleSystem particule in lanterFlameParticules)
                {
                    particule.Play();
                }
                foreach (ParticleSystem particule in lanterGlowParticules)
                {
                    particule.Play();
                }
            }
        }
    }
}