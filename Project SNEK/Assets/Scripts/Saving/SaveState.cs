using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Saving
{
    public class SaveState
    {
        public int heartCoinAmount = 0;

        /// <summary>
        /// 0 is null,
        /// 1 is Swift,
        /// 2 is Beam,
        /// 3 is Shield
        /// </summary>
        public int equipedTechnic = 0;
    }
}