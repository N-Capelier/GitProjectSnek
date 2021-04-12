﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Plates
{
	public class Opener : MonoBehaviour
	{
		#region Variables
		[SerializeField] GameObject blockadeGroup;
        [SerializeField] List<GameObject> blockadeElements = new List<GameObject>();

		[SerializeField] GameObject plateGroup;
		[SerializeField] List<PlateBase> plates = new List<PlateBase>();
		public int plateActivationCount;
		int numPlatesToActivate;
		#endregion

		// Start is called before the first frame update
		void Start()
		{
			int i;
			for (i = 0; i < blockadeGroup.transform.childCount; i++)
            {
                blockadeElements.Add(blockadeGroup.transform.GetChild(i).gameObject);
            }

            for (i = 0; i < plateGroup.transform.childCount; i++)
            {
				plates.Add(plateGroup.transform.GetChild(i).gameObject.GetComponent<PlateBase>());
				plates[i].opener = this;
            }

            plateActivationCount = 0;
			numPlatesToActivate = plates.Count;
		}

		//public void GetPlate(PlateBase plate)
		//{
		//	plates.Add(plate);
		//}

		public void CheckOpening()
        {
            if(plateActivationCount == numPlatesToActivate)
            {
				StartCoroutine(DoOpenWay());
			}
        }

		private IEnumerator DoOpenWay()
        {
			foreach (GameObject blockadeElement in blockadeElements)
			{
				blockadeElement.GetComponent<Animator>().Play("animPlateBloc");
				blockadeElement.GetComponentInChildren<BoxCollider>().enabled = false;
			}
			yield return new WaitForSeconds(0.9f);

			foreach (PlateBase plate in plates)
            {
				StartCoroutine(plate.DisablePlate());
				plate.GetComponent<Collider>().enabled = false;
				plate.GetComponent<Animator>().Play("animPlateON");
            }
		}
    }
}

