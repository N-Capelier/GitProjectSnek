using Rendering.Run;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameManagement
{
    /// <summary>
    /// Nico
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        [Header("References")]
        public StateMachine gameState = null;
        public InputHandler inputHandler = null;
        public UIHandler uiHandler = null;
        public EventSystem eventSystem = null;

        [HideInInspector] public bool playedBossCinematic = false;

        private void Awake()
        {
            CreateSingleton(true);
            Application.targetFrameRate = 30;
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                RunCamController.Instance.vcam.transform.position = RunCamController.Instance.vcam.transform.position.SetX(10f);
            }

            //Debug.Log(RunCamController.Instance.vcam.Follow.ToString());
        }
#endif
    }
}