using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Controller
{
    /// <summary>
    /// Nico
    /// </summary>
    public abstract class PlayerController : MonoBehaviour
    {

        Rigidbody rb = null;
        [SerializeField] [Range(0, 500)] float moveSpeed = 100;

    }
}