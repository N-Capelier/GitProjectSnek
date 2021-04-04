using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Saving;

namespace Player.Technique
{
    public class PlayerRunTechnique : MonoBehaviour
    {

        public GameObject swiftCombo;
        public GameObject swordBeam;
        public GameObject bubbleShield;

        private void Start()
        {
            switch (SaveManager.Instance.state.equipedTechnic)
            {
                case 0:
                    break;
                case 1:
                    swiftCombo.SetActive(true);
                    break;
                case 2:
                    swordBeam.SetActive(true);
                    break;
                case 3:
                    bubbleShield.SetActive(true);
                    break;
                default:
                    throw new System.Exception("Equiped technic is not valid");
            }
        }
    }
}