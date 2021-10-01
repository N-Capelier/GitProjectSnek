using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using System;
using AudioManagement;

namespace Door
{
	public class SpiritGiver : MonoBehaviour
	{
		#region Variables
		public int spiritCeiling;
		private int spiritsValue;
        public bool give;
        [SerializeField] ParticleSystem positiveParticle, negativeParticle;
        #endregion

        private void OnTriggerEnter(Collider other)
        {
			if (other.gameObject.layer == LayerMask.NameToLayer("PlayerController") /*&& !alreadyTriggered*/)
            {
				GiveSpiritToAmount();
            }
		}

        private void Start()
        {
            if(give == true)
            {
                positiveParticle.Play();
            }
            else
            {
                negativeParticle.Play();
            }
        }

        private void GiveSpiritToAmount()
        {
            if(give == true)
            {
                AudioManager.Instance.PlaySoundEffect("LevelSpiritGateStart");

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
                AudioManager.Instance.PlaySoundEffect("LevelSpiritGateEnd");

                int playerSpirits = PlayerManager.Instance.currentController.playerRunSpirits.GetActiveSpirits(); ;
                PlayerManager.Instance.currentController.playerRunSpirits.ConsumeSpirits(playerSpirits);
            }

        }
    }
}

