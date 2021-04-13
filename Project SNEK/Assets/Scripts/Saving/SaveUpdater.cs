using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Saving
{
    public class SaveUpdater : MonoBehaviour
    {

        private void Start()
        {
            SaveManager.Instance.Save();
        }

    }
}