using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName = "New Mouchou Pattern", menuName = "Enemy/Mouchou Pattern", order = 50)]
    public class MouchouPattern : ScriptableObject
    {
        public List<MouchouDirection> patternList;
    }
}