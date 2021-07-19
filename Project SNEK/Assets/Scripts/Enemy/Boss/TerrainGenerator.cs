using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Player;
using GameManagement.GameStates;
using GameManagement;
using Boss;

namespace Boss
{
    public class TerrainGenerator : Singleton<TerrainGenerator>
    {
        [SerializeField] Tilemap tilemap;
        [SerializeField] Tilemap tilemap2;

        [SerializeField] TileBase tile;
        [SerializeField] TileBase[] customTile;
        [SerializeField] GameObject[] cliff;

        [SerializeField] GameObject deathZone;

        [SerializeField] int currentIndex;

        [SerializeField] float delay;

        [SerializeField] GameObject leftCliffParent;
        [SerializeField] GameObject rightCliffParent;

        [SerializeField] List<GameObject> leftCliffs = new List<GameObject>();
        [SerializeField] List<GameObject> rightCliffs = new List<GameObject>();

        public bool bossIsDead = false;

        int cliffIndex = 6;

        private void Awake()
        {
            CreateSingleton();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("PlayerController"))
            {
                GenerateNextCliffs();
                GenerateTerrain();
            }
        }

        public void GenerateNextCliffs()
        {
            Destroy(leftCliffs[0]);
            leftCliffs.RemoveAt(0);
            Destroy(rightCliffs[0]);
            rightCliffs.RemoveAt(0);

            leftCliffs.Add(Instantiate(cliff[Random.Range(0,14)], new Vector3(0, 0, cliffIndex * 6.14f), Quaternion.identity, leftCliffParent.transform));
            rightCliffs.Add(Instantiate(cliff[Random.Range(0, 14)], new Vector3(10, 0, cliffIndex * 6.14f), Quaternion.identity, rightCliffParent.transform));
            rightCliffs[rightCliffs.Count - 1].transform.Rotate(0, 180, 0);

            cliffIndex++;

            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 6.14f);
        }

        private IEnumerator Start()
        {
            if (GameManager.Instance.playedBossCinematic)
            {
                yield return new WaitForEndOfFrame();
                StartCoroutine(TimedUpdate());
            }
            else
            {
                yield return new WaitForSeconds(25f);
                StartCoroutine(TimedUpdate());
                TestAnorexia.Instance.StartPatterns();
            }          
            yield return new WaitForSeconds(1f);
            deathZone.SetActive(true);
        }

        IEnumerator TimedUpdate()
        {
            if(PlayerManager.Instance.currentController.transform.position.z > currentIndex - 17 && bossIsDead == false)
                GenerateTerrain();
            yield return new WaitForSeconds(delay);
            StartCoroutine(TimedUpdate());
        }

        void GenerateTerrain()
        {
            for (int x = -1; x < 10; x++)
            {
                tilemap.SetTile(new Vector3Int(x, currentIndex, 0), tile);
                tilemap2.SetTile(new Vector3Int(x, currentIndex, 0), customTile[Random.Range(0,9)]);
                tilemap.SetTile(new Vector3Int(x, currentIndex - 35, 0), null);
                tilemap2.SetTile(new Vector3Int(x, currentIndex - 35, 0), null);
            }
            currentIndex++;
        }

        int secondIndex = -10;
        public void GenerateStartTerrain()
        {
            for (int i = 0; i < 45; i++)
            {
                for (int x = -1; x < 10; x++)
                {
                    tilemap.SetTile(new Vector3Int(x, secondIndex, -20), tile);
                    tilemap2.SetTile(new Vector3Int(x, secondIndex, - 20), customTile[Random.Range(0, 9)]);
                    //tilemap.SetTile(new Vector3Int(x, secondIndex - 45, 0), null);
                    //tilemap2.SetTile(new Vector3Int(x, secondIndex - 45, 0), null);
                }
                secondIndex++;
            }            
        }
    }
}