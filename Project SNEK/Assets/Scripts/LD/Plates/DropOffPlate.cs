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
			SetSpirit(false);
		}

        private void OnTriggerEnter(Collider other)
        {
			if(other.gameObject.layer == LayerMask.NameToLayer("PlayerController"))
            {
				print(1);
				print(other.gameObject.name);
				if (!hasSpirit)
                {
					print(2);
					if (PlayerManager.Instance.currentController.playerRunSpirits.ConsumeSpirits(1))
                    {
						print(3);
						SetSpirit(true);
						CheckActivation();
					}
				}
                else
                {
					print(4);
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

		public override IEnumerator DisablePlate()
        {
			yield return new WaitForSeconds(1f);
			if (hasSpirit)
			{
				droppedSpirit.SetActive(false);
				PlayerManager.Instance.currentController.playerRunSpirits.AddSpirit();
			}
			print("je rends");
			StartCoroutine(base.DisablePlate());
		}
        
    }
}

