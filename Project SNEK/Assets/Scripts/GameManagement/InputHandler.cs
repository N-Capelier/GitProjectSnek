using UnityEngine;
using Player;
using Rendering.Run;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

namespace GameManagement
{
    public enum InputType
    {
        None,
        Tap,
        SwipeUp,
        SwipeRight,
        SwipeDown,
        SwipeLeft,
        Hold
    }

    /// <summary>
    /// Nico
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        public delegate void InputReceiver(InputType inputType);
        public static event InputReceiver InputReceived;
        public delegate void HoldInputReceiver();
        public static event HoldInputReceiver HoldInputReceived;

        [SerializeField] [Range(0, 500)] float deadzone = 100f;
        [SerializeField] [Range(0, 1)] float tapTimerDuration = 0.1f;
        [SerializeField] [Range(0f, 5f)] float holdTimerDuration = 0.6f;
        float sqrDeadzone;

        Vector2 startPos;
        Vector2 currentPos;

        Clock tapTimer;
        Clock holdTimer;

        bool swiped = false;
        [HideInInspector] public bool holding = false;
        bool holded = false;

        private void Start()
        {
            sqrDeadzone = Mathf.Pow(deadzone, 2);
            tapTimer = new Clock();
            holdTimer = new Clock();
            tapTimer.ClockEnded += OnTapTimerEnded;
            holdTimer.ClockEnded += OnHoldTimerEnded;
        }

        private void Update()
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            HandleStandaloneMouseInput();
            HandleStandaloneKeyboardInput();
#elif UNITY_ANDROID || UNITY_IOS
            HandleMobileTouchInput();
#endif
        }

        private void HandleStandaloneKeyboardInput()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                InputReceived?.Invoke(InputType.SwipeUp);
            if (Input.GetKeyDown(KeyCode.RightArrow))
                InputReceived?.Invoke(InputType.SwipeRight);
            if (Input.GetKeyDown(KeyCode.DownArrow))
                InputReceived?.Invoke(InputType.SwipeDown);
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                InputReceived?.Invoke(InputType.SwipeLeft);
            if (Input.GetKeyDown(KeyCode.Space))
                InputReceived?.Invoke(InputType.Tap);
            if (Input.GetKeyDown(KeyCode.LeftAlt))
                InputReceived?.Invoke(InputType.Hold);
        }

        void HandleStandaloneMouseInput()
        {
            if (Input.GetMouseButtonDown(0)/* && (Input.touches.Length != 0 && Input.touches[0].phase == TouchPhase.Began)*/)
            {
                //tapTimer.StopWithoutEvent();
                startPos = Input.mousePosition; // startPos = Input.touches[0].position - startPos;
                if (GameManager.Instance.gameState.ActiveState == GameState.Run)
                {
                    if (PlayerManager.Instance.currentController.playerRunAttack.isAttacking == false)
                    {
                        holding = true;
                        //HoldInputReceived?.Invoke();
                    }
                }
                holdTimer.SetTime(holdTimerDuration);
                tapTimer.SetTime(tapTimerDuration);
            }
            else if (Input.GetMouseButtonUp(0)/* || Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled*/)
            {
                holding = false;
                if (holded)
                {
                    holded = false;
                    InputReceived?.Invoke(InputType.Hold);
                }
                else if (currentPos.sqrMagnitude < sqrDeadzone)
                {
                    if (GameManager.Instance.gameState.ActiveState == GameState.Run)
                    {
                        if (PlayerManager.Instance.currentController != null)
                        {
                            if (PlayerManager.Instance.currentController.playerRunSpell != null)
                            {
                                if (!PlayerManager.Instance.currentController.playerRunSpell.RaySensorOnUI())
                                    InputReceived?.Invoke(InputType.Tap);
                            }
                        }
                    }
                    else
                    {
                        InputReceived?.Invoke(InputType.Tap);
                    }
                }
                startPos = currentPos = Vector2.zero;

                if (GameManager.Instance.gameState.ActiveState == GameState.Run && RunCamController.Instance.feedbackObjects[RunCamController.Instance.index].isPlaying)
                {
                    RunCamController.Instance.feedbackObjects[RunCamController.Instance.index].Stop(true, ParticleSystemStopBehavior.StopEmitting);
                    RunCamController.Instance.playing = false;
                }
            }

            if (startPos != Vector2.zero)
            {
                if (Input.GetMouseButton(0))
                    currentPos = (Vector2)Input.mousePosition - startPos;
                /*else if (Input.touches.Length != 0)
                    currentPos = Input.touches[0].position - startPos;*/
            }

            if (currentPos.sqrMagnitude > sqrDeadzone)
            {
                float x = currentPos.x;
                float y = currentPos.y;

                //Add swipe trail feedback 
                if (GameManager.Instance.gameState.ActiveState == GameState.Run)
                {
                    RunCamController controller = RunCamController.Instance;
                    Camera tempCam = controller.cam;

                    if (!controller.playing)
                    {
                        for (int i = 0; i < controller.feedbackObjects.Length; i++)
                        {
                            if (!controller.feedbackObjects[i].isPlaying)
                            {
                                controller.feedbackObjects[i].Play();
                                controller.playing = true;
                                controller.index = i;
                                break;
                            }
                        }
                    }

                    controller.feedbackObjects[controller.index].transform.position = tempCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                    controller.feedbackObjects[controller.index].transform.forward = tempCam.transform.forward;
                }


                if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    if (x < 0)
                        InputReceived?.Invoke(InputType.SwipeLeft);
                    else
                        InputReceived?.Invoke(InputType.SwipeRight);
                }
                else
                {
                    if (y < 0)
                        InputReceived?.Invoke(InputType.SwipeDown);
                    else
                        InputReceived?.Invoke(InputType.SwipeUp);
                }
                swiped = true;
            }
        }

        void HandleMobileTouchInput()
        {
            if (Input.touchCount > 0)
            {
                Touch _touch = Input.GetTouch(0);
                if (_touch.phase == TouchPhase.Began)
                {
                    startPos = _touch.position;
                    if (GameManager.Instance.gameState.ActiveState == GameState.Run)
                    {
                        if (PlayerManager.Instance.currentController.playerRunAttack.isAttacking == false)
                        {
                            holding = true;
                            //HoldInputReceived?.Invoke();
                        }
                    }
                    tapTimer.SetTime(tapTimerDuration);
                    holdTimer.SetTime(holdTimerDuration);
                }
                else if (_touch.phase == TouchPhase.Ended || _touch.phase == TouchPhase.Canceled)
                {
                    holding = false;
                    if (holded)
                    {
                        holded = false;
                        InputReceived?.Invoke(InputType.Hold);
                    }
                    else if (currentPos.sqrMagnitude < sqrDeadzone)
                    {
                        InputReceived?.Invoke(InputType.Tap);
                    }
                    startPos = currentPos = Vector2.zero;

                    if (GameManager.Instance.gameState.ActiveState == GameState.Run && RunCamController.Instance.feedbackObjects[RunCamController.Instance.index].isPlaying)
                    {
                        RunCamController.Instance.feedbackObjects[RunCamController.Instance.index].Stop(true, ParticleSystemStopBehavior.StopEmitting);
                        RunCamController.Instance.playing = false;
                    }
                }

                if (startPos != Vector2.zero)
                {
                    if (_touch.phase == TouchPhase.Moved)
                        currentPos = _touch.position - startPos;
                }

                if (currentPos.sqrMagnitude > sqrDeadzone)
                {
                    float x = currentPos.x;
                    float y = currentPos.y;

                    //Add swipe trail feedback 
                    if (GameManager.Instance.gameState.ActiveState == GameState.Run)
                    {
                        RunCamController controller = RunCamController.Instance;
                        Camera tempCam = controller.cam;

                        if (!controller.playing)
                        {
                            for (int i = 0; i < controller.feedbackObjects.Length; i++)
                            {
                                if (!controller.feedbackObjects[i].isPlaying)
                                {
                                    controller.feedbackObjects[i].Play();
                                    controller.playing = true;
                                    controller.index = i;
                                    break;
                                }
                            }
                        }

                        controller.feedbackObjects[controller.index].transform.position = tempCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                        controller.feedbackObjects[controller.index].transform.forward = tempCam.transform.forward;
                    }


                    if (Mathf.Abs(x) > Mathf.Abs(y))
                    {
                        if (x < 0)
                            InputReceived?.Invoke(InputType.SwipeLeft);
                        else
                            InputReceived?.Invoke(InputType.SwipeRight);
                    }
                    else
                    {
                        if (y < 0)
                            InputReceived?.Invoke(InputType.SwipeDown);
                        else
                            InputReceived?.Invoke(InputType.SwipeUp);
                    }
                    swiped = true;
                }
            }
        }

        void OnTapTimerEnded()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            if (!Input.GetMouseButton(0) && !swiped)
            {
                if (GameManager.Instance.gameState.ActiveState == GameState.Run)
                {
                    if (PlayerManager.Instance.currentController != null)
                    {
                        if (PlayerManager.Instance.currentController.playerRunSpell != null)
                        {
                            if (!PlayerManager.Instance.currentController.playerRunSpell.RaySensorOnUI())
                            {
                                InputReceived?.Invoke(InputType.Tap);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    InputReceived?.Invoke(InputType.Tap);
                    return;
                }
            }
            swiped = false;
#elif UNITY_ANDROID || UNITY_IOS
            if(Input.touchCount <= 0  && !swiped)
            {
                InputReceived?.Invoke(InputType.Tap);
            }
            else
            {
                swiped = false;
            }
#endif
        }

        void OnHoldTimerEnded()
        {
            if (holding)
            {
                holding = false;
                holded = true;
            }
        }
    }
}