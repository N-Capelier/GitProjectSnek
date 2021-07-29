using UnityEngine;
using GameManagement;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Player.Spells
{
    /// <summary>
    /// Nico
    /// </summary>
    public abstract class PlayerSpell : MonoBehaviour
    {
        [SerializeField] [Range(0, 10)] float spellCooldown = 4;

        Clock spellCooldownTimer;
        bool canAttack = true;

        [SerializeField] Image buttonImage;
        [HideInInspector] public bool isSpellAccessible = false;

        [SerializeField] GraphicRaycaster graphicRaycaster;

        private void Start()
        {
            PlayerManager.Instance.currentController.playerRunSpell = this;
            spellCooldownTimer = new Clock();
            spellCooldownTimer.ClockEnded += OnCooldownEnded;

            /// /////////////////////////// Remove when new spell getter is added
            //if(SceneManager.GetActiveScene().name == "Level1_1"
            //    || SceneManager.GetActiveScene().name == "TutorialMap")
            //{
            //    gameObject.SetActive(false);
            //}
        }

        private void OnDestroy()
        {
            spellCooldownTimer.ClockEnded -= OnCooldownEnded;
        }

        private void Update()
        {
            buttonImage.fillAmount = 1 - spellCooldownTimer.time.Remap(0, spellCooldown, 0, 1);
        }

        public bool RaySensorOnUI()
        {
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

        public void SpellCastFromButton()
        {
            LaunchSpellCast(PlayerManager.Instance.currentController.currentDirection);
        }

        void LaunchSpellCast(Controller.PlayerDirection spellDirection)
        {
            //////////////////////////////              TEMPORARY                                ////////////////////////////////////////
            //if (SceneManager.GetActiveScene().name == "Level1_3" || SceneManager.GetActiveScene().name == "Boss Anorexia" || SceneManager.GetActiveScene().name == "Level1_2V2")
            //{
            if (canAttack && PlayerManager.Instance.currentController.playerRunSpirits.GetActiveSpirits() >= 3)
                {
                    canAttack = false;
                    spellCooldownTimer.SetTime(spellCooldown);
                    buttonImage.color = new Color(1f, 1f, 1f, .25f);

                    StartCoroutine(SpellCast(spellDirection));
                }
            //}
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        }

        void OnCooldownEnded()
        {
            canAttack = true;
            buttonImage.color = new Color(1f, 1f, 1f, 1f);
        }

        void HandleInput(InputType inputType)
        {
            if (PlayerManager.Instance.currentController.isInCutscene || SceneManager.GetActiveScene().name == "TutorialMap")
            {
                return;
            }

            //switch (inputType)
            //{
            //    case InputType.SwipeUp:
            //        if (PlayerManager.Instance.currentController.currentDirection == Controller.PlayerDirection.Down)
            //        {
            //            LaunchSpellCast(Controller.PlayerDirection.Down);
            //        }
            //        break;
            //    case InputType.SwipeRight:
            //        if (PlayerManager.Instance.currentController.currentDirection == Controller.PlayerDirection.Left)
            //        {
            //            LaunchSpellCast(Controller.PlayerDirection.Left);
            //        }
            //        break;
            //    case InputType.SwipeDown:
            //        if (PlayerManager.Instance.currentController.currentDirection == Controller.PlayerDirection.Up)
            //        {
            //            LaunchSpellCast(Controller.PlayerDirection.Up);
            //        }
            //        break;
            //    case InputType.SwipeLeft:
            //        if (PlayerManager.Instance.currentController.currentDirection == Controller.PlayerDirection.Right)
            //        {
            //            LaunchSpellCast(Controller.PlayerDirection.Right);
            //        }
            //        break;
            //    default:
            //        break;
            //}

            if(inputType == InputType.Hold)
            {
                LaunchSpellCast(PlayerManager.Instance.currentController.currentDirection);
            }
        }

        public abstract IEnumerator SpellCast(Controller.PlayerDirection spellDirection);
    }
}