using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plates
{
	public class Opener : MonoBehaviour
	{
		#region Variables
		[SerializeField] GameObject blockadeGroup;
		List<GameObject> blockadeElements;

		List<PlateBase> plates;
		public int plateActivationCount;
		int numPlatesToActivate;
		#endregion

		// Start is called before the first frame update
		void Start()
		{
            for (int i = 0; i < blockadeGroup.transform.childCount; i++)
            {
				blockadeElements.Add(blockadeGroup.transform.GetChild(i).gameObject);
			}

			plateActivationCount = 0;
			numPlatesToActivate = plates.Count;
		}

		public void GetPlate(PlateBase plate)
		{
			plates.Add(plate);
		}

		public void CheckOpening()
        {
            if(plateActivationCount == numPlatesToActivate)
            {
				DoOpenWay();
			}
        }

		private void DoOpenWay()
        {
			foreach (GameObject blockadeElement in blockadeElements)
			{
				blockadeElement.gameObject.SetActive(false);
			}

			foreach (PlateBase plate in plates)
            {
				plate.enabled = false;
            }
		}
    }
}

