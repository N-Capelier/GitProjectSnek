using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioManagement;
using Player;

namespace Enemy
{
    public class IllusionisteBehaviour : MonoBehaviour
    {
        EnemyStats stats;
        IllusionisteMovement movement;
        public GameObject clone;
        public int cloneNumber;
        //public GameObject[] clonesList;
        public List<GameObject> clonesList;
        bool isStunned;
        [SerializeField] BoxCollider spawnTrigger;
        public bool isSpawned = false;
        /*[HideInInspector]*/ public bool isKillable = false;

        private void Start()
        {
            List<GameObject> clonesList = new List<GameObject>();
            //clonesList = new GameObject[cloneNumber];
            stats = GetComponent<EnemyStats>();
            movement = GetComponent<IllusionisteMovement>();

        }

        private void Update()
        {
            /*if(isKillable == true && isStunned == false)
            {
                IsStunned();
            }*/
        }

        int index = 0;
        public IEnumerator OnshouldAttack()
        {
            yield return new WaitForSeconds(stats.attackCooldown);

            if(isKillable == false)
            {
                index = Random.Range(0, clonesList.Count);
                StartCoroutine(clonesList[index].GetComponent<IllusionisteClone>().Fire());
            }
        }

        /*public void IsStunned()
        {
            isStunned = true;
            GetComponentInChildren<Animator>().SetBool("isStunned", true);
            movement.StopAllCoroutines();
        }*/

        public void Death()
        {
            AudioManager.Instance.PlaySoundEffect("ObjectSpiritCollect");
            PlayerManager.Instance.currentController.playerRunSpirits.AddSpirit();
            Destroy(gameObject);
        }

        public void CloneDeath(GameObject index)
        {            
            for (int i = 0; i < clonesList.Count; i++)
            {

                if(clonesList[i] == index)
                {
                    clonesList[i].GetComponent<IllusionisteClone>().StopAllCoroutines();
                    clonesList.Remove(index);
                    cloneNumber--;
                    AudioManager.Instance.PlaySoundEffect("ObjectSpiritCollect");
                    if(clonesList.Count.Equals(0))
                    {
                        PlayerManager.Instance.currentController.playerRunSpirits.AddSpirit();
                        Destroy(gameObject);
                    }
                    Destroy(index);

                }                
            }
        }

        
    }
}
