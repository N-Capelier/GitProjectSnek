using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class MouchounoiaSpawner : MonoBehaviour
    {
        int mouchouClone;
        public int mouchouNumber;
        public GameObject mouchouPrefab;
        List<GameObject> shrubs;
        GameObject shrub;
        public GameObject shrubPrefab;
        public GameObject spawnFx;

        public EnemyAttackPattern pattern;

        // Start is called before the first frame update
        void Start()
        {
            for (int x = 0; x < pattern.row.Length; x++)
            {
                for (int y = 0; y < pattern.row[x].column.Length; y++)
                {
                    if (pattern.row[x].column[y] == true)
                    {
                        shrub = Instantiate(shrubPrefab, (new Vector3((transform.position.x + y), (transform.position.y), (transform.position.z - x + 5))), Quaternion.identity);
                        Instantiate(spawnFx, (new Vector3((transform.position.x + y), (transform.position.y), (transform.position.z - x + 5))), Quaternion.identity);
                        shrubs.Add(shrub);
                    }
                }
            }

            for (int j = 0; j < mouchouNumber; j++)
            {
                mouchouClone = Random.Range(0, shrubs.Count);
                for (int i = 0; i < shrubs.Count; i++)
                {
                    if (i == mouchouClone)
                    {
                        Instantiate(mouchouPrefab, shrubs[i].transform.position, Quaternion.identity);
                        Destroy(shrubs[i]);
                        shrubs.Remove(shrubs[i]);
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}