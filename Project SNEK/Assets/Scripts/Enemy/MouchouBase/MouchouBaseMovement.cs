﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using Map;

public enum MouchouDirection
{
    Up,
    Right,
    Down,
    Left
}

public class MouchouBaseMovement : MonoBehaviour
{
    EnemyStats stats;
    Rigidbody rb;

    [HideInInspector] public MouchouDirection currentDirection;
    //MouchouDirection nextDirection;

    [HideInInspector] public Vector3 currentNode;
    [HideInInspector] public Vector3 nextNode;
    [HideInInspector] public bool isMoving = false;
    [HideInInspector] public bool canMove = true;

    public GameObject renderer;
    public Animator anim;

    [Space]
    public bool lineVertical = false;
    public bool square = false;


    void Start()
    {
        stats = GetComponentInParent<EnemyStats>();
        rb = GetComponentInParent<Rigidbody>();

        stats.movementClock.ClockEnded += OnShouldMove;

        currentNode = stats.gameObject.transform.position;
        currentDirection = MouchouDirection.Down;
    }


    void Update()
    {
        if ((currentDirection == MouchouDirection.Up && transform.position.z >= MapGrid.Instance.GetWorldPos(0, (int)nextNode.z).z) ||
            (currentDirection == MouchouDirection.Right && transform.position.x >= MapGrid.Instance.GetWorldPos((int)nextNode.x, 0).x) ||
            (currentDirection == MouchouDirection.Down && transform.position.z <= MapGrid.Instance.GetWorldPos(0, (int)nextNode.z).z) ||
            (currentDirection == MouchouDirection.Left && transform.position.x <= MapGrid.Instance.GetWorldPos((int)nextNode.x, 0).x))
        {
            StopMove();
        }
    }

    public void UpdateMovement()
    {
        rb.velocity = (nextNode - currentNode) * stats.moveSpeed;
        anim.SetBool("isMoving", true);
        isMoving = true;
    }

    void StopMove()
    {
        rb.velocity = new Vector3(0, 0, 0);
        currentNode = nextNode;
        anim.SetBool("isMoving", false);
        isMoving = false;
    }

    void OnShouldMove()
    {
        if (canMove == true)
        {
            if (lineVertical == true)
            {
                LineVerticalPattern();
            }

            if (square == true)
            {
                SquarePattern();
            }
        }
    }

    void LineVerticalPattern()
    {
        currentDirection = MouchouDirection.Down;
        GetNextNode();
        UpdateMovement();
    }

    void SquarePattern()
    {

        switch (currentDirection)
        {
            case MouchouDirection.Up:
                currentDirection = MouchouDirection.Right;
                break;
            case MouchouDirection.Right:
                currentDirection = MouchouDirection.Down;
                break;
            case MouchouDirection.Down:
                currentDirection = MouchouDirection.Left;
                break;
            case MouchouDirection.Left:
                currentDirection = MouchouDirection.Up;
                break;
        }

        GetNextNode();
        UpdateMovement();
    }

    public Vector3 GetNextNode()
    {
        switch (currentDirection)
        {
            case MouchouDirection.Up:
                nextNode = new Vector3(currentNode.x, currentNode.y, currentNode.z + 1);
                renderer.transform.rotation = Quaternion.Euler(0,0,0);
                break;
            case MouchouDirection.Right:
                nextNode = new Vector3(currentNode.x + 1, currentNode.y, currentNode.z);
                renderer.transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case MouchouDirection.Down:
                nextNode = new Vector3(currentNode.x, currentNode.y, currentNode.z - 1);
                renderer.transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case MouchouDirection.Left:
                nextNode = new Vector3(currentNode.x - 1, currentNode.y, currentNode.z);
                renderer.transform.rotation = Quaternion.Euler(0, 270, 0);
                break;
        }        
        return nextNode;
    }
}
