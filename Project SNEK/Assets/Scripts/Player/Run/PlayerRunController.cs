﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameManagement;
using Map;

namespace Player.Controller
{
    /// <summary>
    /// Nico
    /// </summary>
    public class PlayerRunController : PlayerController
    {
        Vector3 currentNode;
        Vector3 nextNode;

        private void Start()
        {
            currentNode = startingNode;
            currentDirection = PlayerDirection.Up;
            nextDirection = PlayerDirection.Up;
            nextNode = GetNextNode();
            InputHandler.InputReveived += HandleInput;
        }

        private void Update()
        {
            if ((currentDirection == PlayerDirection.Up && transform.position.z >= MapGrid.Instance.GetWorldPos(0, (int)nextNode.z).z) ||
                (currentDirection == PlayerDirection.Right && transform.position.x >= MapGrid.Instance.GetWorldPos((int)nextNode.x, 0).x) ||
                (currentDirection == PlayerDirection.Down && transform.position.z <= MapGrid.Instance.GetWorldPos(0, (int)nextNode.z).z) ||
                (currentDirection == PlayerDirection.Left && transform.position.x <= MapGrid.Instance.GetWorldPos((int)nextNode.x, 0).x))
            {
                UpdateMovement();
            }
        }

        private void FixedUpdate()
        {
            rb.velocity = (nextNode - currentNode);
        }

        void UpdateMovement()
        {
            currentDirection = nextDirection;
            currentNode = nextNode;
            nextNode = GetNextNode();
        }

        void HandleInput(InputType inputType)
        {
            switch (inputType)
            {
                case InputType.SwipeUp:
                    if (currentDirection != PlayerDirection.Down)
                        nextDirection = PlayerDirection.Up;
                    break;
                case InputType.SwipeRight:
                    if (currentDirection != PlayerDirection.Left)
                        nextDirection = PlayerDirection.Right;
                    break;
                case InputType.SwipeDown:
                    if (currentDirection != PlayerDirection.Up)
                        nextDirection = PlayerDirection.Down;
                    break;
                case InputType.SwipeLeft:
                    if (currentDirection != PlayerDirection.Right)
                        nextDirection = PlayerDirection.Left;
                    break;
                default:
                    break;
            }
        }

        Vector3 GetNextNode()
        {
            Vector3 _nextNode;

            switch (currentDirection)
            {
                case PlayerDirection.Up:
                    _nextNode = new Vector3(currentNode.x, currentNode.y, currentNode.z + 1);
                    break;
                case PlayerDirection.Right:
                    _nextNode = new Vector3(currentNode.x + 1, currentNode.y, currentNode.z);
                    break;
                case PlayerDirection.Down:
                    _nextNode = new Vector3(currentNode.x, currentNode.y, currentNode.z - 1);
                    break;
                case PlayerDirection.Left:
                    _nextNode = new Vector3(currentNode.x - 1, currentNode.y, currentNode.z);
                    break;
                default:
                    throw new System.NotImplementedException("Player direction not implemented.");
            }
            return _nextNode;
        }

    }
}