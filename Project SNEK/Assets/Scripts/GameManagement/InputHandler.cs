using UnityEngine;

namespace GameManagement
{
    public enum InputType
    {
        None,
        Tap,
        SwipeUp,
        SwipeRight,
        SwipeDown,
        SwipeLeft
    }

    /// <summary>
    /// Nico
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        public delegate void InputReceiver(InputType inputType);
        public static event InputReceiver InputReceived;

        [SerializeField] [Range(0, 500)] float deadzone = 100f;
        [SerializeField] [Range(0, 1)] float tapTimerDuration = 0.1f;
        float sqrDeadzone;

        Vector2 startPos;
        Vector2 currentPos;

        Clock tapTimer;

        bool swiped = false;

        private void Start()
        {
            sqrDeadzone = Mathf.Pow(deadzone, 2);
            tapTimer = new Clock();
            tapTimer.ClockEnded += OnTapTimerEnded;
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
        }

        void HandleStandaloneMouseInput()
        {
            if (Input.GetMouseButtonDown(0)/* && (Input.touches.Length != 0 && Input.touches[0].phase == TouchPhase.Began)*/)
            {
                //tapTimer.StopWithoutEvent();
                startPos = Input.mousePosition; // startPos = Input.touches[0].position - startPos;
                tapTimer.SetTime(tapTimerDuration);
            }
            else if (Input.GetMouseButtonUp(0)/* || Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled*/)
            {
                startPos = currentPos = Vector2.zero;
                if (currentPos.sqrMagnitude > sqrDeadzone)
                {
                    InputReceived?.Invoke(InputType.Tap);
                    swiped = true;
                }
            }

            if(startPos != Vector2.zero)
            {
                if (Input.GetMouseButton(0))
                    currentPos = (Vector2)Input.mousePosition - startPos;
                /*else if (Input.touches.Length != 0)
                    currentPos = Input.touches[0].position - startPos;*/
            }

            if(currentPos.sqrMagnitude > sqrDeadzone)
            {
                float x = currentPos.x;
                float y = currentPos.y;
                if(Mathf.Abs(x) > Mathf.Abs(y))
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
                startPos = currentPos = Vector2.zero;
            }
        }

        void HandleMobileTouchInput()
        {
            if(Input.touchCount > 0)
            {
                Touch _touch = Input.GetTouch(0);
                if(_touch.phase == TouchPhase.Began)
                {
                    startPos = _touch.position;
                    tapTimer.SetTime(tapTimerDuration);
                }
                else if(_touch.phase == TouchPhase.Ended || _touch.phase == TouchPhase.Canceled)
                {
                    startPos = currentPos = Vector2.zero;
                }

                if (startPos != Vector2.zero)
                {
                    if(_touch.phase == TouchPhase.Moved)
                        currentPos = _touch.position - startPos;
                }

                if (currentPos.sqrMagnitude > sqrDeadzone)
                {
                    float x = currentPos.x;
                    float y = currentPos.y;
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
                    startPos = currentPos = Vector2.zero;
                }
            }
        }

        void OnTapTimerEnded()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            if(!Input.GetMouseButton(0) && !swiped)
            {
                InputReceived?.Invoke(InputType.Tap);
            }
            else
            {
                swiped = false;
            }
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
    }
}