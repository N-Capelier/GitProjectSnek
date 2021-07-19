using System.Collections;
using UnityEngine;

namespace Wall
{
    /// <summary>
    /// Arthur
    /// </summary>
    public class WallTimed : WallBehaviour
    {
        public bool deadByTime;

        [Range(0, 5)] public float timeToDeath;
        public ParticleSystem trail;

        void Start()
        {
            if (deadByTime == true)
            {
                StartCoroutine(DeathByTime(timeToDeath));
                //trail.main.duration = timeToDeath;
            }

        }

        IEnumerator DeathByTime(float time)
        {
            yield return new WaitForSeconds(time);
            gameObject.GetComponent<Collider>().enabled = false;
            yield return new WaitForSeconds(2);
            Destroy(gameObject);
        }

        //Charles
        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            
            if(other.gameObject.layer == LayerMask.NameToLayer("Wall") || other.gameObject.layer == LayerMask.NameToLayer("LaBuBulle")/*temporary*/)
            {
                StartCoroutine(GetDestroyed());
            }
        }
    }
}