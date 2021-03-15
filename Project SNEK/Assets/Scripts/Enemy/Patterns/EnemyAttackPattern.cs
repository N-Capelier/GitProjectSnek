using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    
    [System.Serializable]
    public class EnemyAttackPattern
    {
        [System.Serializable]
        public struct rowData
        {
            public bool[] row;
        }

        
        public rowData[] rows = new rowData[7];
    }
}