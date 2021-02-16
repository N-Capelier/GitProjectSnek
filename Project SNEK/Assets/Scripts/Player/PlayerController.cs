using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player.Controller
{
    public enum PlayerDirection
    {
        Up,
        Right,
        Down,
        Left
    }

    /// <summary>
    /// Nico
    /// </summary>
    public abstract class PlayerController : MonoBehaviour
    {
        public Vector3 startingNode = new Vector3(3, 0, 1);
        [HideInInspector] public PlayerDirection currentDirection;
        [HideInInspector] public PlayerDirection nextDirection;

        [Space]
        [HideInInspector] public Rigidbody rb = null;
        [Range(0, 500)] public float moveSpeed = 50;

        public virtual void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }
    }
}