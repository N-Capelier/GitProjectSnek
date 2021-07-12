using UnityEngine;
using GameManagement;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        private void Start()
        {
            spellCooldownTimer = new Clock();
            spellCooldownTimer.ClockEnded += OnCooldownEnded;
            InputHandler.InputReceived += HandleInput;
        }

        private void OnDestroy()
        {
            spellCooldownTimer.ClockEnded -= OnCooldownEnded;
            InputHandler.InputReceived -= HandleInput;
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
                if (canAttack)
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