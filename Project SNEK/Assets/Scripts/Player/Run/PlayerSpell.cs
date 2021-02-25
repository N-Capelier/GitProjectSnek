using UnityEngine;
using GameManagement;

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

        private void Start()
        {
            spellCooldownTimer = new Clock();
            spellCooldownTimer.ClockEnded += OnCooldownEnded;
            InputHandler.InputReceived += HandleInput;
        }

        void LaunchSpellCast(Controller.PlayerDirection spellDirection)
        {
            if(canAttack)
            {
                canAttack = false;
                spellCooldownTimer.SetTime(spellCooldown);

                SpellCast(spellDirection);
            }
        }

        void OnCooldownEnded()
        {
            canAttack = true;
        }

        void HandleInput(InputType inputType)
        {
            switch (inputType)
            {
                case InputType.SwipeUp:
                    if(PlayerManager.Instance.currentController.currentDirection == Controller.PlayerDirection.Down)
                    {
                        LaunchSpellCast(Controller.PlayerDirection.Down);
                    }
                    break;
                case InputType.SwipeRight:
                    if (PlayerManager.Instance.currentController.currentDirection == Controller.PlayerDirection.Left)
                    {
                        LaunchSpellCast(Controller.PlayerDirection.Left);
                    }
                    break;
                case InputType.SwipeDown:
                    if (PlayerManager.Instance.currentController.currentDirection == Controller.PlayerDirection.Up)
                    {
                        LaunchSpellCast(Controller.PlayerDirection.Up);
                    }
                    break;
                case InputType.SwipeLeft:
                    if (PlayerManager.Instance.currentController.currentDirection == Controller.PlayerDirection.Right)
                    {
                        LaunchSpellCast(Controller.PlayerDirection.Right);
                    }
                    break;
                default:
                    break;
            }
        }

        public abstract void SpellCast(Controller.PlayerDirection spellDirection);
    }
}