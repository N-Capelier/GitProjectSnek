using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Controller;
using System;

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

    public class InputHandler : MonoBehaviour
    {
        public delegate void InputReveiver(InputType inputType);
        public static event InputReveiver InputReveived;

        [SerializeField] [Range(0, 500)] float deadzone = 100f;
        [SerializeField] [Range(0, 1)] float tapTimerDuration = 0.2f;
        float sqrDeadzone;

        Vector2 startPos;
        Vector2 currentPos;

        Clock tapTimer;

        private void Start()
        {
            sqrDeadzone = Mathf.Pow(deadzone, 2);
            tapTimer = new Clock();
            tapTimer.ClockEnded += OnTapTimerEnded;
        }

        private void Update()
        {
            HandleTap();
            HandleKeyboard();
        }

        private void HandleKeyboard()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                InputReveived?.Invoke(InputType.SwipeUp);
            if (Input.GetKeyDown(KeyCode.RightArrow))
                InputReveived?.Invoke(InputType.SwipeRight);
            if (Input.GetKeyDown(KeyCode.DownArrow))
                InputReveived?.Invoke(InputType.SwipeDown);
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                InputReveived?.Invoke(InputType.SwipeLeft);
        }

        void HandleTap()
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
                        InputReveived?.Invoke(InputType.SwipeLeft);
                    else
                        InputReveived?.Invoke(InputType.SwipeRight);
                }
                else
                {
                    if (y < 0)
                        InputReveived?.Invoke(InputType.SwipeDown);
                    else
                        InputReveived?.Invoke(InputType.SwipeUp);
                }
                startPos = currentPos = Vector2.zero;
            }
        }

        void OnTapTimerEnded()
        {
            if(!Input.GetMouseButton(0))
            {
                InputReveived?.Invoke(InputType.Tap);
            }
        }
    }
}