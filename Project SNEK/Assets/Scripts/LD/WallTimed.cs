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
            yield return new WaitForSeconds(time + 1);
            Destroy(gameObject);
        }
    }
}