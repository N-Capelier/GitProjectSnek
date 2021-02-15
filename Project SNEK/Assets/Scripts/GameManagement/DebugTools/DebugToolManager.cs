using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagement.DebugTools
{
    /// <summary>
    /// Nico
    /// </summary>
    public class DebugToolManager : MonoBehaviour
    {
        [SerializeField] bool activateTools = false;
        [Space]
        [SerializeField] GameObject[] tools;

        private void Update()
        {
            if(tools.Length != 0)
            {
                if (activateTools && !tools[0].activeSelf)
                {
                    foreach (GameObject go in tools)
                    {
                        go.SetActive(true);
                    }
                }
                else if(!activateTools && tools[0].activeSelf)
                {
                    foreach (GameObject go in tools)
                    {
                        go.SetActive(false);
                    }
                }
            }
        }
    }
}