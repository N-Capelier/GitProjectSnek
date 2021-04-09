using System.Collections;
using UnityEngine;
using GameManagement;
using Player.Controller;

namespace Player.Technique
{
    /// <summary>
    /// Arthur
    /// </summary>
    public abstract class PlayerTechnique : MonoBehaviour
    {
        [SerializeField] [Range(0, 10)] float techniqueCooldown = 4;

        Clock techniqueCooldonwTimer;
        public bool canDoTechnique = true;

        public void Start()
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
                    LaunchTechnique(Controller.PlayerDirection.Left);
                    break;
                case Controller.PlayerDirection.Up:
                    LaunchTechnique(Controller.PlayerDirection.Up);
                    break;
                case Controller.PlayerDirection.Right:
                    LaunchTechnique(Controller.PlayerDirection.Right);
                    break;
                case Controller.PlayerDirection.Down:
                    LaunchTechnique(Controller.PlayerDirection.Down);
                    break;
            }
        }

        public abstract IEnumerator TechniqueCast(Controller.PlayerDirection techniqueDirection);

    }
}
