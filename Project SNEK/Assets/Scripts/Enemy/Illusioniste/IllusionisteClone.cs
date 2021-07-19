using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Enemy
{
    public class IllusionisteClone : MonoBehaviour
    {
        Vector3 bulletDir;
        Vector3 playerPos;
        public Material chargingMat;
        public Material killableMat;
        [HideInInspector] public Material defaultMat;

        public GameObject bullet;
        GameObject incomingBullet;
        public float attackRange = 10;
        public float bulletSpeed = 10;

        private void Start()
        {
            //defaultMat = GetComponentInChildren<MeshRenderer>().material;
        }

        private void LateUpdate()
        {
            playerPos = PlayerManager.Instance.currentController.transform.position;
            bulletDir = (playerPos - transform.position);
            if (GetComponentInParent<IllusionisteBehaviour>().isKillable == false)
            {
                transform.LookAt(playerPos);
            }            
        }

        public IEnumerator Fire()
        {
            if(bulletDir.magnitude >= attackRange /*|| bulletDir.magnitude <= -attackRange*/)
            {
                yield break;
            }
            else
            {
                GetComponentInChildren<Animator>().SetBool("isAttacking", true);
                yield return new WaitForSeconds(1.2f);
                GetComponentInChildren<Animator>().SetBool("isAttacking", false);
                incomingBullet = Instantiate(bullet, gameObject.transform.position, Quaternion.identity);
                incomingBullet.GetComponent<Rigidbody>().AddForce(bulletDir.normalized * bulletSpeed, ForceMode.Force);
            }
        }
    }
}
