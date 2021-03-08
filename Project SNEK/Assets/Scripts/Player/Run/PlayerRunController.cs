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
        Animator animator;

        public delegate void PlayerChangingDirection();
        public static event PlayerChangingDirection PlayerChangedDirection;

        private void Start()
        {
            animator = objectRenderer.GetComponent<Animator>();
            currentNode = startingNode;
            currentDirection = PlayerDirection.Up;
            nextDirection = PlayerDirection.Up;
            nextNode = GetNextNode();
            InputHandler.InputReceived += HandleInput;
        }

        private void OnEnable()
        {
            currentDirection = PlayerDirection.Up;
            nextDirection = PlayerDirection.Up;
            objectRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
            currentNode = respawnNode;
            nextNode = GetNextNode();
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
            if (canMove == true && isDead == false)
                rb.velocity = (nextNode - currentNode) * moveSpeed * moveSpeedModifier;
            else
            {
                rb.velocity = Vector3.zero;
            }
        }

        void UpdateMovement()
        {
            if (currentDirection == PlayerDirection.Up && nextDirection == PlayerDirection.Right
                || currentDirection == PlayerDirection.Right && nextDirection == PlayerDirection.Down
                || currentDirection == PlayerDirection.Down && nextDirection == PlayerDirection.Left
                || currentDirection == PlayerDirection.Left && nextDirection == PlayerDirection.Up)
            {
                objectRenderer.transform.Rotate(0, 90, 0);
                animator.Play("Anim_PlayerRun_TurnR");
            }
            else if (currentDirection == PlayerDirection.Up && nextDirection == PlayerDirection.Left
                || currentDirection == PlayerDirection.Right && nextDirection == PlayerDirection.Up
                || currentDirection == PlayerDirection.Down && nextDirection == PlayerDirection.Right
                || currentDirection == PlayerDirection.Left && nextDirection == PlayerDirection.Down)
            {
                objectRenderer.transform.Rotate(0, -90, 0);
                animator.Play("Anim_PlayerRun_TurnL");
            }

            currentDirection = nextDirection;
            currentNode = nextNode;
            nextNode = GetNextNode();
        }

        /// <summary>
        /// Get new direction on user input
        /// </summary>
        /// <param name="inputType"></param>
        void HandleInput(InputType inputType)
        {
            switch (inputType)
            {
                case InputType.SwipeUp:
                    if (currentDirection != PlayerDirection.Down)
                    {
                        PlayerChangedDirection?.Invoke();
                        nextDirection = PlayerDirection.Up;
                    }
                    break;
                case InputType.SwipeRight:
                    if (currentDirection != PlayerDirection.Left)
                    {
                        PlayerChangedDirection?.Invoke();
                        nextDirection = PlayerDirection.Right;
                    }
                    break;
                case InputType.SwipeDown:
                    if (currentDirection != PlayerDirection.Up)
                    {
                        PlayerChangedDirection?.Invoke();
                        nextDirection = PlayerDirection.Down;
                    }
                    break;
                case InputType.SwipeLeft:
                    if (currentDirection != PlayerDirection.Right)
                    {
                        PlayerChangedDirection?.Invoke();
                        nextDirection = PlayerDirection.Left;
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// find next targeted node on the grid
        /// </summary>
        /// <returns></returns>
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