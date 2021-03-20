using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    
    [System.Serializable]
    public class EnemyAttackPattern
    {
        public RowData[] row = new RowData[9];


        [System.Serializable]
        public struct RowData
        {
            public bool[] column;
        }
    }
}