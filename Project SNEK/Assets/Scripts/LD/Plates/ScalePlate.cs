using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

namespace Plates
{
	public class ScalePlate : PlateBase
	{
        #region Variables
        [SerializeField] int numSpiritsRequired;
        #endregion
       
        private void Awake()
        {
            opener.GetPlate(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == LayerMask.NameToLayer("PlayerController") 
                && PlayerManager.Instance.currentController.playerRunSpirits.spiritChain.Count >= numSpiritsRequired)
            {
                CheckActivation();
            } 
        }

        
    }
}

