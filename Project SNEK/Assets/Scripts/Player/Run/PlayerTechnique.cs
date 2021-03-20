using System.Collections;
using UnityEngine;
using GameManagement;

namespace Player.Technique
{
    /// <summary>
    /// Arthur
    /// </summary>
    public abstract class PlayerTechnique : MonoBehaviour
    {
        [SerializeField] [Range(0, 10)] float techniqueCooldown = 4;

        Clock techniqueCooldonwTimer;
        bool canDoTechnique = true;

        private void Start()
        {
            techniqueCooldonwTimer = new Clock();
            techniqueCooldonwTimer.ClockEnded += OnCooldownEnded;
            InputHandler.InputReceived += HandleInput;
        }

        void LaunchTechnique(Controller.PlayerDirection techniqueDirection)
        {
            if (canDoTechnique)
            {
                canDoTechnique = false;
                techniqueCooldonwTimer.SetTime(techniqueCooldown);

                StartCoroutine(TechniqueCast(techniqueDirection));
            }
        }

        private void OnDestroy()
        {
            techniqueCooldonwTimer.ClockEnded -= OnCooldownEnded;
            InputHandler.InputReceived -= HandleInput;
        }

        void OnCooldownEnded()
        {
            canDoTechnique = true;
        }

        void HandleInput(InputType inputType)
        {
            if (inputType != InputType.Hold)
                return;

            switch (PlayerManager.Instance.currentController.currentDirection)
            {
                case Controller.PlayerDirection.Left:
                    TechniqueCast(Controller.PlayerDirection.Left);
                    break;
                case Controller.PlayerDirection.Up:
                    TechniqueCast(Controller.PlayerDirection.Up);
                    break;
                case Controller.PlayerDirection.Right:
                    TechniqueCast(Controller.PlayerDirection.Right);
                    break;
                case Controller.PlayerDirection.Down:
                    TechniqueCast(Controller.PlayerDirection.Down);
                    break;
            }
        }

        public abstract IEnumerator TechniqueCast(Controller.PlayerDirection techniqueDirection);

    }
}
