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
            defaultMat = GetComponentInChildren<MeshRenderer>().material;
        }

        private void LateUpdate()
        {
            playerPos = PlayerManager.Instance.currentController.transform.position;
            bulletDir = (playerPos - transform.position);
        }

        public IEnumerator Fire()
        {
            if(bulletDir.magnitude >= attackRange /*|| bulletDir.magnitude <= -attackRange*/)
            {
                yield break;
            }
            else
            {
                GetComponentInChildren<MeshRenderer>().material = chargingMat;
                yield return new WaitForSeconds(1);
                GetComponentInChildren<MeshRenderer>().material = defaultMat;
                incomingBullet = Instantiate(bullet, gameObject.transform.position, Quaternion.identity);
                incomingBullet.GetComponent<Rigidbody>().AddForce(bulletDir.normalized * bulletSpeed, ForceMode.Force);
            }
        }
    }
}
