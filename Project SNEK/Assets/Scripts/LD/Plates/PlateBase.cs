using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plates
{
	public abstract class PlateBase : MonoBehaviour
	{
		#region Variables
		public Opener opener;
		#endregion
		
		protected void CheckActivation()
        {
			opener.plateActivationCount++;
			opener.CheckOpening();
        }

		protected void CheckDeactivation()
        {
			opener.plateActivationCount--;
        }
	}
}

