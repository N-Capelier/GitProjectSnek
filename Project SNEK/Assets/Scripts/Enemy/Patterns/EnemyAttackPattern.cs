using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName = "New Enemy Attack Pattern", menuName = "Enemy/Attack Pattern", order = 51)]
    public class EnemyAttackPattern : ScriptableObject
    {
        public bool[,] attackPattern;
    }
}