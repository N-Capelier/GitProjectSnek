using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using System;

namespace Plates
{
	public class DropOffPlate : PlateBase
	{
		#region Variables
		bool hasSpirit;
		[SerializeField] GameObject droppedSpirit;
		#endregion

		// Start is called before the first frame update
		void Start()
		{
			hasSpirit = false;
		}

        private void OnTriggerEnter(Collider other)
        {
			if(other.gameObject.layer == LayerMask.NameToLayer("PlayerController"))
            {
                if (!hasSpirit)
                {
                    if (PlayerManager.Instance.currentController.playerRunSpirits.ConsumeSpirits(1))
                    {
						CheckActivation();
						SetSpirit(true);
					}
				}
                else
                {
					CheckDeactivation();
					SetSpirit(false);
					PlayerManager.Instance.currentController.playerRunSpirits.AddSpirit();
				}
            }

		}

        private void SetSpirit(bool _setHasSpirit)
        {
			hasSpirit = _setHasSpirit;
			droppedSpirit.SetActive(_setHasSpirit);
        }

        private void OnDisable()
        {
            if (hasSpirit)
            {
				droppedSpirit.SetActive(false);
				PlayerManager.Instance.currentController.playerRunSpirits.AddSpirit();
			}
		}
    }
}

