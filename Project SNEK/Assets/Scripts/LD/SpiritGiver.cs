using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using System;

namespace Door
{
	public class SpiritGiver : MonoBehaviour
	{
		#region Variables
		public int spiritCeiling;
		private int spiritsToGive;
        //bool alreadyTriggered = false;
        #endregion

        private void OnTriggerEnter(Collider other)
        {
			if (other.gameObject.layer == LayerMask.NameToLayer("PlayerController") /*&& !alreadyTriggered*/)
            {
				GiveSpiritToAmount();
            }
		}

        private void GiveSpiritToAmount()
        {
            int playerSpirits = PlayerManager.Instance.currentController.playerRunSpirits.GetActiveSpirits(); ;
            
            if (playerSpirits < spiritCeiling)
            {
                spiritsToGive = spiritCeiling - playerSpirits;

                while(spiritsToGive > 0)
                {
                    PlayerManager.Instance.currentController.playerRunSpirits.AddSpirit();
                    spiritsToGive--;
                    print("gate spirit +1");
                }

                enabled = false;
            }
        }
    }
}

