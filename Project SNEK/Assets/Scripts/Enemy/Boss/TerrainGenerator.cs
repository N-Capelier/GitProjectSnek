using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Boss
{
    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField] Tilemap tilemap;

        [SerializeField] TileBase tile;
        [SerializeField] GameObject cliff;

        [SerializeField] GameObject deathZone;

        [SerializeField] int currentIndex;

        [SerializeField] float delay;

        [SerializeField] GameObject leftCliffParent;
        [SerializeField] GameObject rightCliffParent;

        [SerializeField] List<GameObject> leftCliffs = new List<GameObject>();
        [SerializeField] List<GameObject> rightCliffs = new List<GameObject>();

        int cliffIndex = 6;

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("PlayerController"))
            {
                GenerateNextCliffs();
            }
        }

        public void GenerateNextCliffs()
        {
            Destroy(leftCliffs[0]);
            leftCliffs.RemoveAt(0);
            Destroy(rightCliffs[0]);
            rightCliffs.RemoveAt(0);

            leftCliffs.Add(Instantiate(cliff, new Vector3(0, 0, cliffIndex * 6.14f), Quaternion.identity, leftCliffParent.transform));
            rightCliffs.Add(Instantiate(cliff, new Vector3(10, 0, cliffIndex * 6.14f), Quaternion.identity, rightCliffParent.transform));
            rightCliffs[rightCliffs.Count - 1].transform.Rotate(0, 180, 0);

            cliffIndex++;

            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 6.14f);
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(.5f);
            StartCoroutine(TimedUpdate());

            yield return new WaitForSeconds(1f);
            deathZone.SetActive(true);
        }

        IEnumerator TimedUpdate()
        {
            if(Player.PlayerManager.Instance.currentController.transform.position.z > currentIndex - 17)
                GenerateTerrain();
            yield return new WaitForSeconds(delay);
            StartCoroutine(TimedUpdate());
        }

        void GenerateTerrain()
        {
            for (int x = -1; x < 10; x++)
            {
                tilemap.SetTile(new Vector3Int(x, currentIndex, 0), tile);
                tilemap.SetTile(new Vector3Int(x, currentIndex - 35, 0), null);
            }
            currentIndex++;
        }
    }
}