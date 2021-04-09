using UnityEngine;
using GameManagement;
using Map;
using AudioManagement;
using System.Collections;

namespace Player.Controller
{
    /// <summary>
    /// Nico
    /// </summary>
    public class PlayerRunController : PlayerController
    {
        Vector3 currentNode;
        Vector3 nextNode;
        

        public delegate void PlayerChangingDirection(PlayerDirection direction);
        public static event PlayerChangingDirection PlayerChangedDirection;

        float inputSpeed = 1f;

        public GameObject techniqueFx;

        private void Start()
        {
            animator = objectRenderer.GetComponent<Animator>();
            currentNode = startingNode;
            currentDirection = PlayerDirection.Up;
            nextDirection = PlayerDirection.Up;
            nextNode = GetNextNode();
            InputHandler.InputReceived += HandleInput;
            InputHandler.HoldInputReceived += OnHold;
        }

        private void OnEnable()
        {
            currentDirection = PlayerDirection.Up;
            nextDirection = PlayerDirection.Up;
            objectRenderer.transform.rotation = Quaternion.Euler(0, 0, 0);
            currentNode = respawnNode;
            nextNode = GetNextNode();
        }

        private void OnDestroy()
        {
            InputHandler.InputReceived -= HandleInput;
            InputHandler.HoldInputReceived -= OnHold;
        }

        private void Update()
        {

            if ((currentDirection == PlayerDirection.Up && transform.position.z >= MapGrid.Instance.GetWorldPos(0, (int)nextNode.z).z) ||
                (currentDirection == PlayerDirection.Right && transform.position.x >= MapGrid.Instance.GetWorldPos((int)nextNode.x, 0).x) ||
                (currentDirection == PlayerDirection.Down && transform.position.z <= MapGrid.Instance.GetWorldPos(0, (int)nextNode.z).z) ||
                (currentDirection == PlayerDirection.Left && transform.position.x <= MapGrid.Instance.GetWorldPos((int)nextNode.x, 0).x))
            {
                inputSpeed = 1f;
                SnapPosition();
                UpdateMovement();
            }
        }

        private void FixedUpdate()
        {
            if (isDead == false)
            {
                rb.velocity = (nextNode - currentNode) * moveSpeed * attackMoveSpeedModifier * spellMoveSpeedModifier * inputSpeed;
            }
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
                AudioManager.Instance.PlaySoundEffect("PlayerSwipe01");
            }
            else if (currentDirection == PlayerDirection.Up && nextDirection == PlayerDirection.Left
                || currentDirection == PlayerDirection.Right && nextDirection == PlayerDirection.Up
                || currentDirection == PlayerDirection.Down && nextDirection == PlayerDirection.Right
                || currentDirection == PlayerDirection.Left && nextDirection == PlayerDirection.Down)
            {
                objectRenderer.transform.Rotate(0, -90, 0);
                animator.Play("Anim_PlayerRun_TurnL");
                AudioManager.Instance.PlaySoundEffect("PlayerSwipe01");
            }

            PlayerChangedDirection?.Invoke(currentDirection);

            currentDirection = nextDirection;
            currentNode = nextNode;
            nextNode = GetNextNode();
        }

        float bonusInputSpeed = 1.75f;

        /// <summary>
        /// Get new direction on user input
        /// </summary>
        /// <param name="inputType"></param>
        void HandleInput(InputType inputType)
        {
            if(fadeToHoldCoroutine != null)
            {
                StopCoroutine(fadeToHoldCoroutine);
            }
            PlayerManager.Instance.currentController.animator.SetLayerWeight(1, 0f);

            switch (inputType)
            {
                case InputType.SwipeUp:
                    if (currentDirection != PlayerDirection.Down && currentDirection != PlayerDirection.Up)
                    {
                        nextDirection = PlayerDirection.Up;
                        inputSpeed = bonusInputSpeed;
                    }
                    break;
                case InputType.SwipeRight:
                    if (currentDirection != PlayerDirection.Left && currentDirection != PlayerDirection.Right)
                    {
                        nextDirection = PlayerDirection.Right;
                        inputSpeed = bonusInputSpeed;

                    }
                    break;
                case InputType.SwipeDown:
                    if (currentDirection != PlayerDirection.Up && currentDirection != PlayerDirection.Down)
                    {
                        nextDirection = PlayerDirection.Down;
                        inputSpeed = bonusInputSpeed;

                    }
                    break;
                case InputType.SwipeLeft:
                    if (currentDirection != PlayerDirection.Right && currentDirection != PlayerDirection.Left)
                    {
                        inputSpeed = bonusInputSpeed;

                        nextDirection = PlayerDirection.Left;
                    }
                    break;
                default:
                    break;
            }
        }

        void OnHold()
        {
            PlayerManager.Instance.currentController.animator.Play("Anim_PlayerRun_runCHARGE");
            fadeToHoldCoroutine = StartCoroutine(FadeToHold());
        }

        [SerializeField] float holdFadeSpeed = 1f;
        Coroutine fadeToHoldCoroutine;

        IEnumerator FadeToHold()
        {
            while(PlayerManager.Instance.currentController.animator.GetLayerWeight(1) < 1)
            {
                float _weight = PlayerManager.Instance.currentController.animator.GetLayerWeight(1);
                _weight += holdFadeSpeed * Time.deltaTime;
                _weight = Mathf.Clamp(_weight, 0, 1);

                PlayerManager.Instance.currentController.animator.SetLayerWeight(1, _weight);
                yield return new WaitForEndOfFrame();
            }
            techniqueFx.GetComponent<ParticleSystem>().Play();
            yield break;            
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