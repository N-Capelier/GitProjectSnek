using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Spells
{
    public class MonochromElementsManager : Singleton<MonochromElementsManager>
    {
        private void Awake()
        {
            CreateSingleton(false);
        }

        public List<MonochromElement> elements = new List<MonochromElement>();

    }
}


