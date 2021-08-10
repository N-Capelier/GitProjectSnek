﻿using UnityEngine;
using GameManagement;
using Map;
using AudioManagement;
using System.Collections;
using UnityEngine.SceneManagement;
using Saving;
using Player.Spells;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Player.Controller
{
    public enum Spell
    {
        None,
        Poppy,
        Thistle,
        Bergamot
    }


    /// <summary>
    /// Nico
    /// </summary>
    public class PlayerRunController : PlayerController
    {
        Vector3 currentNode;
        Vector3 nextNode;
        
        public delegate void PlayerChangingDirection(PlayerDirection direction);
        public static event PlayerChangingDirection PlayerChangedDirection;

        public delegate void PlayerChangingDirectionForAttack();
        public static event PlayerChangingDirectionForAttack PlayerChangedDirectionForAttack;

        float inputSpeed = 1f;

        public GameObject techniqueFx;

        public PlayerRunFeebacks feedbacks;

        bool spellAvailable = false;

        public GameObject spellUIContainer;
        public GameObject spellUIButton;
        public Button spellButton;

        public PlayerSpell poppySpell, thistleSpell, bergamotSpell;

        public GameObject spellCanvas;

        [SerializeField] GraphicRaycaster graphicRaycaster;

        [Header("Spell Sprites")]
        [SerializeField] Sprite bergamotSpellSprite;
        [SerializeField] Sprite poppySpellSprite;
        [SerializeField] Sprite thistleSpellSprite;
        

        public void SetSpell(Spell _spell)
        {
            if(playerRunSpell != null)
                playerRunSpell.enabled = false;

            switch (_spell)
            {
                case Spell.Poppy:
                    poppySpell.enabled = true;
                    spellCanvas.SetActive(true);

                    //Set Sprite
                    spellButton.image.sprite = poppySpellSprite;
                    //Set correct method to onclick on button
                    spellButton.onClick.RemoveAllListeners();
                    spellButton.onClick.AddListener(poppySpell.SpellCastFromButton);
                    break;
                case Spell.Thistle:
                    thistleSpell.enabled = true;
                    spellCanvas.SetActive(true);

                    //Set Sprite
                    spellButton.image.sprite = thistleSpellSprite;
                    //Set correct method to onclick on button
                    spellButton.onClick.RemoveAllListeners();
                    spellButton.onClick.AddListener(thistleSpell.SpellCastFromButton);

                    break;
                case Spell.Bergamot:
                    bergamotSpell.enabled = false;
                    spellCanvas.SetActive(true);

                    //Set Sprite
                    spellButton.image.sprite = bergamotSpellSprite;
                    //Set correct method to onclick on button
                    spellButton.onClick.RemoveAllListeners();
                    spellButton.onClick.AddListener(bergamotSpell.SpellCastFromButton);

                    break;
                default:
                    spellCanvas.SetActive(false);
                    poppySpell.enabled = false;
                    thistleSpell.enabled = false;
                    bergamotSpell.enabled = false;


                    break;
            }
        }

        public bool RaySensorOnUI()
        {
            if (!PlayerManager.Instance.currentController.runController.spellCanvas.gameObject.activeSelf)
                return false;

            bool _rayHit = false;

            PointerEventData _ped = new PointerEventData(GameManager.Instance.eventSystem);
            _ped.position = Input.mousePosition;

            List<RaycastResult> _results = new List<RaycastResult>();

            graphicRaycaster.Raycast(_ped, _results);
            foreach (RaycastResult result in _results)
            {
                if (result.gameObject.CompareTag("Interactable"))
                {
                    _rayHit = true;
                }
            }
            return _rayHit;
        }

        private IEnumerator Start()
        {
            animator = objectRenderer.GetComponent<Animator>();
            currentNode = startingNode;
            currentDirection = PlayerDirection.Up;
            nextDirection = PlayerDirection.Up;
            nextNode = GetNextNode();

            runController = this;

            GameManager.Instance.uiHandler.spellUI = spellUIContainer;
            GameManager.Instance.uiHandler.spellButton = spellUIButton.GetComponent<RectTransform>();

            GameManager.Instance.uiHandler.SwapHand(SaveManager.Instance.state.uiAccessibility);
            

            switch (SceneManager.GetActiveScene().name)
            {
                case "TutorialMap":
                case "Level1_1":
                case "Level1_2":
                    break;
                default:
                    spellAvailable = true;
                    break;
            }

            InputHandler.InputReceived += HandleInput;

            moveSpeed = 0.1f;
            animator.Play("Anim_PlayerRun_Idle");
            yield return new WaitForSeconds(3f);
            moveSpeed = cachedMoveSpeed;
            animator.Play("Anim_PlayerRun_Run");
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
            if (isDead == false && isInCutscene == false)
            {
                rb.velocity = (nextNode - currentNode) * moveSpeed * attackMoveSpeedModifier * spellMoveSpeedModifier * inputSpeed;
            }
            else if(isDead)
            {
                rb.velocity = Vector3.zero;
            }
            else
            {
                //Physics pro tips
                rb.velocity = new Vector3(.01f, .01f, .01f);
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
                animator.Play(Animator.StringToHash("Anim_PlayerRun_TurnR"));
                AudioManager.Instance.PlaySoundEffect("PlayerSwipe01");
                PlayerChangedDirectionForAttack?.Invoke();
            }
            else if (currentDirection == PlayerDirection.Up && nextDirection == PlayerDirection.Left
                || currentDirection == PlayerDirection.Right && nextDirection == PlayerDirection.Up
                || currentDirection == PlayerDirection.Down && nextDirection == PlayerDirection.Right
                || currentDirection == PlayerDirection.Left && nextDirection == PlayerDirection.Down)
            {
                objectRenderer.transform.Rotate(0, -90, 0);
                animator.Play(Animator.StringToHash("Anim_PlayerRun_TurnL"));
                AudioManager.Instance.PlaySoundEffect("PlayerSwipe01");
                PlayerChangedDirectionForAttack?.Invoke();
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
            if (isInCutscene)
                return;

            if (fadeToHoldCoroutine != null)
            {
                StopCoroutine(fadeToHoldCoroutine);
            }
            PlayerManager.Instance.currentController.animator.SetLayerWeight(1, 0f);

            switch (inputType)
            {
                case InputType.SwipeUp:
                    if (currentDirection != PlayerDirection.Down && currentDirection != PlayerDirection.Up)
                    {
                        //feedbacks.PlayAnimUp();
                        nextDirection = PlayerDirection.Up;
                        inputSpeed = bonusInputSpeed;                        
                    }
                    else if (currentDirection != PlayerDirection.Up) { AudioManager.Instance.PlaySoundEffect("UINoTurnOver"); }
                    break;
                case InputType.SwipeRight:
                    if (currentDirection != PlayerDirection.Left && currentDirection != PlayerDirection.Right)
                    {
                        //feedbacks.PlayAnimRight();
                        nextDirection = PlayerDirection.Right;
                        inputSpeed = bonusInputSpeed;                        

                    }
                    else if (currentDirection != PlayerDirection.Right) { AudioManager.Instance.PlaySoundEffect("UINoTurnOver"); }
                    break;
                case InputType.SwipeDown:
                    if (currentDirection != PlayerDirection.Up && currentDirection != PlayerDirection.Down)
                    {
                        //feedbacks.PlayAnimDown();
                        nextDirection = PlayerDirection.Down;
                        inputSpeed = bonusInputSpeed;                        

                    }
                    else if (currentDirection != PlayerDirection.Down) { AudioManager.Instance.PlaySoundEffect("UINoTurnOver"); }
                    break;
                case InputType.SwipeLeft:
                    if (currentDirection != PlayerDirection.Right && currentDirection != PlayerDirection.Left)
                    {
                        //feedbacks.PlayAnimLeft();
                        inputSpeed = bonusInputSpeed;
                        nextDirection = PlayerDirection.Left;                        
                    }
                    else if (currentDirection != PlayerDirection.Left) { AudioManager.Instance.PlaySoundEffect("UINoTurnOver"); }
                    break;
                default:
                    break;
            }
        }

        void OnHold()
        {
            if (isInCutscene || !spellAvailable)
                return;

            PlayerManager.Instance.currentController.animator.Play(Animator.StringToHash("Anim_PlayerRun_runCHARGE"));
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
            AudioManager.Instance.PlaySoundEffect("ChargeAttackDone");
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