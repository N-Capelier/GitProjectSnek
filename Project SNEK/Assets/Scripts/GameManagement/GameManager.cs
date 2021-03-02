﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagement
{
    /// <summary>
    /// Nico
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        [Header("References")]
        public StateMachine gameState = null;

        private void Awake()
        {
            CreateSingleton(true);
        }
    }
}