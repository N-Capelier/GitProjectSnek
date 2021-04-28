using UnityEngine;
using System.Collections.Generic;
using Player;

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

        private void OnTriggerEnter(Collider other)
        {
            if (!hasChecked && other.CompareTag("Player"))
            {
                conffeti.Play();
                foreach (ParticleSystem particule in lanterFlameParticules)
                {
                    particule.Play();
                }
                foreach (ParticleSystem particule in lanterGlowParticules)
                {
                    particule.Play();
                }
                PlayerManager.Instance.currentController.checkPoint = transform;
                PlayerManager.Instance.currentController.respawnNode = transform.position;
                hasChecked = true;
            }
        }
    }
}