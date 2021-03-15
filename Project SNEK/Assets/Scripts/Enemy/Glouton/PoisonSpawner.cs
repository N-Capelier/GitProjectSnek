using System.Collections;
using UnityEngine;

namespace Enemy
{
    /// <summary>
    /// Arthur
    /// </summary>
    public class PoisonSpawner : MonoBehaviour
    {
        public float timeToSpawn;
        public GameObject explosion;
        public GameObject vomi;


        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(SpawnPoison());
        }

        IEnumerator SpawnPoison()
        {
            yield return new WaitForSeconds(timeToSpawn);
            Instantiate(explosion, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
            Instantiate(vomi, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}