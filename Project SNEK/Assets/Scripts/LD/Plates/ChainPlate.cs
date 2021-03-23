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
            if(other.gameObject.layer == LayerMask.NameToLayer("PlayerController") || other.gameObject.layer == LayerMask.NameToLayer("SpiritChain"))
            {
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
        //    if (other.gameObject.layer == LayerMask.NameToLayer("PlayerController") || other.gameObject.layer == LayerMask.NameToLayer("SpiritChain"))
        //    {
        //        if (!playerOrChainWeight)
        //            playerOrChainWeight = true;
        //    }
        //}

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("PlayerController") || other.gameObject.layer == LayerMask.NameToLayer("SpiritChain"))
            {
                playerOrChainWeight--;

                if(playerOrChainWeight == 0)
                {
                    print("weight = 0");
                    CheckDeactivation();
                    hasWeight = false;
                }
            }
        }
    }
}

