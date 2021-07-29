using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AudioManagement;

namespace Plates
{
	public abstract class PlateBase : MonoBehaviour
	{
		#region Variables
		public Opener opener;
		#endregion
		
		protected void CheckActivation()
        {
            AudioManager.Instance.PlaySoundEffect("LevelButtonOn");
            opener.plateActivationCount++;
			opener.CheckOpening();
        }

		protected void CheckDeactivation()
        {
			opener.plateActivationCount--;
        }

		public virtual IEnumerator DisablePlate() 
		{
			enabled = false;
			yield break;
		}
	}
}

