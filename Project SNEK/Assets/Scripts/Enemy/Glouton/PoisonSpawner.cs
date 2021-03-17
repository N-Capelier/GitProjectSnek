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
        public GameObject bomb;

        GameObject projectile;


        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(SpawnPoison());
        }

        private void FixedUpdate()
        {
            if(projectile != null)
            {
                projectile.transform.position = new Vector3(projectile.transform.position.x, projectile.transform.position.y -0.5f, projectile.transform.position.z);
            }
        }

        IEnumerator SpawnPoison()
        {
            yield return new WaitForSeconds(timeToSpawn - 0.3f);
            projectile = Instantiate(bomb, new Vector3(transform.position.x, 6, transform.position.z), Quaternion.identity);
            yield return new WaitForSeconds(0.3f);
            Instantiate(explosion, transform.position, Quaternion.identity);            
            yield return new WaitForSeconds(0.2f);            
            Destroy(gameObject);
        }


    }
}