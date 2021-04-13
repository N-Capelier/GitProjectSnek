using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playtesting
{
    public class PlayTestManager : MonoBehaviour
    {

        [SerializeField] bool isPlayTestBuild = false;

        [SerializeField] int year;
        [SerializeField] int month;
        [SerializeField] int day;

        DateTime outOfValidityDate;

        private void Start()
        {
            if(isPlayTestBuild)
            {
                CheckValidity();
            }
        }

        private void CheckValidity()
        {
            outOfValidityDate = new DateTime(year, month, day);
            if(outOfValidityDate < DateTime.Now)
            {
                AppManager.Quit();
            }
        }
    }
}