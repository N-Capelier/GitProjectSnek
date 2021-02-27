using System.Collections;
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

    MouchouDirection currentDirection;
    MouchouDirection nextDirection;

    Vector3 currentNode;
    Vector3 nextNode;
    [SerializeField] bool canMove = true;

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
        if (canMove == false)
        {
            if ((currentDirection == MouchouDirection.Up && transform.position.z >= MapGrid.Instance.GetWorldPos(0, (int)nextNode.z).z) ||
                (currentDirection == MouchouDirection.Right && transform.position.x >= MapGrid.Instance.GetWorldPos((int)nextNode.x, 0).x) ||
                (currentDirection == MouchouDirection.Down && transform.position.z <= MapGrid.Instance.GetWorldPos(0, (int)nextNode.z).z) ||
                (currentDirection == MouchouDirection.Left && transform.position.x <= MapGrid.Instance.GetWorldPos((int)nextNode.x, 0).x))
            {
                StopMove();
            }                
        }
    }

    void UpdateMovement()
    {
        rb.velocity = (nextNode - currentNode) * stats.moveSpeed;
    }

    void StopMove()
    {
        rb.velocity = new Vector3(0, 0, 0);
        canMove = true;
        currentNode = nextNode;
    }

    void OnShouldMove()
    {
        if(lineVertical == true && canMove == true)
        {
            LineVerticalPattern();
        }

        if (square == true && canMove == true)
        {
            SquarePattern();
        }
    }

    void LineVerticalPattern()
    {
        currentDirection = MouchouDirection.Down;
        canMove = false;
        GetNextNode();
        UpdateMovement();
    }

    void SquarePattern()
    {
        canMove = false;

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

    Vector3 GetNextNode()
    {
        switch (currentDirection)
        {
            case MouchouDirection.Up:
                nextNode = new Vector3(currentNode.x, currentNode.y, currentNode.z + 1);
                break;
            case MouchouDirection.Right:
                nextNode = new Vector3(currentNode.x + 1, currentNode.y, currentNode.z);
                break;
            case MouchouDirection.Down:
                nextNode = new Vector3(currentNode.x, currentNode.y, currentNode.z - 1);
                break;
            case MouchouDirection.Left:
                nextNode = new Vector3(currentNode.x - 1, currentNode.y, currentNode.z);
                break;
        }
        return nextNode;
    }
}
