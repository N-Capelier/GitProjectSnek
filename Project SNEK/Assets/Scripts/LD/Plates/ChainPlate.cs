using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plates
{
	public class ChainPlate : PlateBase
	{
        #region Variables
        bool playerOrChainWeight;
        #endregion
        //private void Awake()
        //{
        //    opener.GetPlate(this);
        //}

        private void Start()
        {
            playerOrChainWeight = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("PlayerController"))
            {
                CheckActivation();
                playerOrChainWeight = true;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("PlayerController") || other.gameObject.layer == LayerMask.NameToLayer("SpiritChain"))
            {
                if (!playerOrChainWeight)
                    playerOrChainWeight = true;
            }
            else
                playerOrChainWeight = false;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("PlayerController") || other.gameObject.layer == LayerMask.NameToLayer("SpiritChain"))
            {
                if (!playerOrChainWeight)
                    CheckDeactivation();
            }
        }
    }
}

