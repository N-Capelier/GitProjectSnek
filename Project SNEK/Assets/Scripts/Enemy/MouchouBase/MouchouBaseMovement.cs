using UnityEngine;
using Map;

namespace Enemy
{
    public enum MouchouDirection
    {
        Up,
        Right,
        Down,
        Left
    }

    /// <summary>
    /// Arthur
    /// </summary>
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

        public GameObject objectRenderer;
        public Animator anim;

        [SerializeField] MouchouPattern pattern;
        int patternIndex = 0;


        void Start()
        {
            stats = GetComponentInParent<EnemyStats>();
            rb = GetComponentInParent<Rigidbody>();

            stats.movementClock.ClockEnded += OnShouldMove;

            currentNode = MapGrid.Instance.GetWorldPos((int)stats.gameObject.transform.position.x, (int)stats.gameObject.transform.position.z);
            currentDirection = MouchouDirection.Down;
            OnShouldMove();
        }


        void Update()
        {
            if (MapGrid.Instance == null)
                return;
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

        public void UpdateMovementDash()
        {
            rb.velocity = (nextNode - currentNode) * (stats.moveSpeed * 2);
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

        public void OnShouldMove()
        {
            if (canMove == true)
            {
                if(pattern.patternList.Count>0)
                {
                    currentDirection = pattern.patternList[patternIndex];
                    if(patternIndex < pattern.patternList.Count)
                    {
                        patternIndex++;
                    }
                    else
                    {
                        patternIndex = 0;
                    }


                    GetNextNode();
                    UpdateMovement();
                }
            }
        }


        public Vector3 GetNextNode()
        {
            switch (currentDirection)
            {
                case MouchouDirection.Up:
                    nextNode = new Vector3(currentNode.x, currentNode.y, currentNode.z + 1);
                    objectRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
                case MouchouDirection.Right:
                    nextNode = new Vector3(currentNode.x + 1, currentNode.y, currentNode.z);
                    objectRenderer.transform.rotation = Quaternion.Euler(0, 90, 0);
                    break;
                case MouchouDirection.Down:
                    nextNode = new Vector3(currentNode.x, currentNode.y, currentNode.z - 1);
                    objectRenderer.transform.rotation = Quaternion.Euler(0, 180, 0);
                    break;
                case MouchouDirection.Left:
                    nextNode = new Vector3(currentNode.x - 1, currentNode.y, currentNode.z);
                    objectRenderer.transform.rotation = Quaternion.Euler(0, 270, 0);
                    break;
            }
            return nextNode;
        }

        private void OnDestroy()
        {
            stats.movementClock.ClockEnded -= OnShouldMove;
        }
    }
}