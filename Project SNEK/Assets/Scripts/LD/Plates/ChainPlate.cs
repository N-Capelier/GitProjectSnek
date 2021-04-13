using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plates
{
	public class ChainPlate : PlateBase
	{
        #region Variables
        int playerOrChainWeight;
        bool hasWeight;
        #endregion
        public ParticleSystem onEnterParticle;
        public Animator animator;
        [HideInInspector] public bool done = false;
        //private void Awake()
        //{
        //    opener.GetPlate(this);
        //}

        private void Start()
        {
            playerOrChainWeight = 0;
            hasWeight = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("PlayerController") || other.gameObject.layer == LayerMask.NameToLayer("Spirit"))
            {
                Instantiate(onEnterParticle, transform.position + Vector3.up * 0.25f, Quaternion.identity);
                if(done == false)
                animator.Play("animPlateON");

                if (other.gameObject.layer == LayerMask.NameToLayer("Spirit"))
                    print("sprit in");

                if (!hasWeight)
                {

                    print("weight >= 1");
                    CheckActivation();
                    hasWeight = true;
                }
                
                playerOrChainWeight++;
            }
        }

        //private void OnTriggerStay(Collider other)
        //{
        //    if (other.gameObject.layer == LayerMask.NameToLayer("PlayerController") || other.gameObject.layer == LayerMask.NameToLayer("Spirit"))
        //    {
        //        if (!playerOrChainWeight)
        //            playerOrChainWeight = true;
        //    }
        //}

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("PlayerController") || other.gameObject.layer == LayerMask.NameToLayer("Spirit"))
            {
                animator.Play("animPlateOFF");
                if (other.gameObject.layer == LayerMask.NameToLayer("Spirit"))
                    print("sprit out");

                playerOrChainWeight--;
                //StartCoroutine(DecrementWeightWithDelay());

                if (playerOrChainWeight == 0)
                {
                    print("weight = 0");
                    CheckDeactivation();
                    hasWeight = false;
                }
            }
        }

        //IEnumerator DecrementWeightWithDelay()
        //{
        //    yield return new WaitForSeconds(0.5f);
        //    playerOrChainWeight--;
        //}
    }
}

