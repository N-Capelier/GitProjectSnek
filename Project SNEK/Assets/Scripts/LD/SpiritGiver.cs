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
		private int spiritsValue;
        public bool give;
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
            if(give == true)
            {
                int playerSpirits = PlayerManager.Instance.currentController.playerRunSpirits.GetActiveSpirits(); ;

                if (playerSpirits < spiritCeiling)
                {
                    spiritsValue = spiritCeiling - playerSpirits;

                    while (spiritsValue > 0)
                    {
                        PlayerManager.Instance.currentController.playerRunSpirits.AddSpirit();
                        spiritsValue--;
                    }
                }
            }
            else if (give == false)
            {
                int playerSpirits = PlayerManager.Instance.currentController.playerRunSpirits.GetActiveSpirits(); ;
                PlayerManager.Instance.currentController.playerRunSpirits.ConsumeSpirits(playerSpirits);
            }

        }
    }
}

