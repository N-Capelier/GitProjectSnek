using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Controller;

namespace Player
{
    /// <summary>
    /// Nico
    /// </summary>
    public class PlayerManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] GameObject hubPlayer = null;
        [SerializeField] GameObject runPlayer = null;

        [HideInInspector] public PlayerController currentController = null;
    }
}