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
		GameObject droppedSpirit;
		#endregion

		// Start is called before the first frame update
		void Start()
		{
			hasSpirit = false;
		}

        private void OnTriggerEnter(Collider other)
        {
			if(other.gameObject.layer == LayerMask.NameToLayer("PlayerController") && !hasSpirit)
            {
				CheckActivation();
				TakeSpirit();
            }

		}

        private void TakeSpirit()
        {
			hasSpirit = true;
			//Spawn and/or move sprit object
        }

        private void OnDisable()
        {
			droppedSpirit = null;
        }
    }
}

