using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rendering.Hub
{
    public class HubCamTargetController : MonoBehaviour
    {
        [HideInInspector] public int actions = 0;

        [SerializeField] Camera cam;
        Plane ground;

        Vector3 currentPos = new Vector3();

#if UNITY_STANDALONE || UNITY_EDITOR
        Touch standaloneTouch = new Touch();
#endif

        private void Start()
        {
            standaloneTouch.position = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        public void Update()
        {
            if (actions > 0)
            {
                currentPos = Vector3.zero;
                return;
            }

#if UNITY_STANDALONE || UNITY_EDITOR
            HandleStandaloneInputs();
#elif UNITY_ANDROID || UNITY_IOS
            HandleMobileInputs();
#endif
        }

        private void HandleStandaloneInputs()
        {
            standaloneTouch.deltaPosition = standaloneTouch.position;
            Vector2 _screenPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            standaloneTouch.position = cam.ScreenToWorldPoint(_screenPosition);

            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
                ground.SetNormalAndPosition(transform.up, transform.position);
                print($"pos: {standaloneTouch.position} | posDelta: {standaloneTouch.deltaPosition}");
                if(standaloneTouch.deltaPosition != standaloneTouch.position)
                {
                    standaloneTouch.phase = TouchPhase.Moved;
                    currentPos = GetGroundPositionDelta(standaloneTouch);
                }
                else
                {
                    standaloneTouch.phase = TouchPhase.Stationary;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                cam.transform.Translate(currentPos, Space.World);
            }
        }

        private void HandleMobileInputs()
        {
            if (Input.touchCount > 0)
            {
                ground.SetNormalAndPosition(transform.up, transform.position);

                currentPos = GetGroundPositionDelta(Input.GetTouch(0));

                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    cam.transform.Translate(currentPos, Space.World);
                }
            }
        }

        Vector3 GetGroundPositionDelta(Touch _touch)
        {
            if (_touch.phase != TouchPhase.Moved)
                return Vector3.zero;

            Ray _lastRay = cam.ScreenPointToRay(_touch.position - _touch.deltaPosition);
            Ray _currentRay = cam.ScreenPointToRay(_touch.position);

            if (ground.Raycast(_lastRay, out float _lastDistance) && ground.Raycast(_currentRay, out float _currentDistance))
                return _lastRay.GetPoint(_lastDistance) - _currentRay.GetPoint(_currentDistance);

            return Vector3.zero;
        }
    }
}