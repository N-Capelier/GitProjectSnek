using UnityEngine;
using Player;
using GameManagement;
using Hub.Interaction;

namespace Rendering.Hub
{
    /// <summary>
    /// Nico
    /// </summary>
    public class HubCamTargetController : Singleton<HubCamTargetController>
    {
        [SerializeField] [Range(0f, 1000f)] float cameraSpeed = 35f;
        [SerializeField] [Range(1f, 10f)] float horizontalSpeedModifier = 1.35f;
        [SerializeField] [Range(0, 100f)] float cameraMaxSpeed = 50f;

        [SerializeField] Rigidbody rb;

        [HideInInspector] public byte actions = 0;

        Clock interactionTimer;
        [HideInInspector] public bool movedCamera = false;
        [SerializeField] float interactionCancelTime = .08f;

        float leftConfiner = -13.95685f;
        float rightConfiner = 11.03282f;
        float topConfiner = 13.97992f;
        float bottomConfiner = -11.68559f;

        private void Awake()
        {
            CreateSingleton();
        }

        private void Start()
        {
            interactionTimer = new Clock();
            interactionTimer.ClockEnded += OnInteractionTimerEnded;
        }

        private void OnDestroy()
        {
            interactionTimer.ClockEnded -= OnInteractionTimerEnded;
        }

        private void FixedUpdate()
        {
            if (actions > 0) //Cancels cam movement on interaction
            {
                rb.velocity = Vector3.zero;
                //return;
            }

            if (PlayerManager.Instance == null)
            {
                return;
            }

#if UNITY_STANDALONE || UNITY_EDITOR
            HandleStandaloneInputs();
#elif UNITY_ANDROID || UNITY_IOS
            HandleMobileTouchInputs();
#endif
        }

        private void Update()
        {
            if (transform.position.x < leftConfiner)
            {
                rb.velocity = Vector3.zero;
                transform.position = new Vector3(leftConfiner, transform.position.y, transform.position.z);
            }
            else if (transform.position.x > rightConfiner)
            {
                rb.velocity = Vector3.zero;
                transform.position = new Vector3(rightConfiner, transform.position.y, transform.position.z);
            }
            else if (transform.position.z < bottomConfiner)
            {
                rb.velocity = Vector3.zero;
                transform.position = new Vector3(transform.position.x, transform.position.y, bottomConfiner);
            }
            else if (transform.position.z > topConfiner)
            {
                rb.velocity = Vector3.zero;
                transform.position = new Vector3(transform.position.x, transform.position.y, topConfiner);
            }
        }

        Vector3 lastPos = new Vector2();
        Vector3 currentPos = new Vector2();

        private void HandleStandaloneInputs()
        {
            if (PlayerManager.Instance.currentController == null)
            {
                return;
            }
            else if (PlayerManager.Instance.currentController.isInCutscene || InteractionManager.Instance.isInteracting)
            {
                return;
            }


            lastPos = currentPos;
            if (Input.GetMouseButton(0) && !GameManager.Instance.uiHandler.dialogueUI.isRunningDialogue)
            {
                currentPos = Input.mousePosition;

                if (currentPos != lastPos && lastPos != Vector3.zero)
                {
                    movedCamera = true;

                    float _dist = Mathf.Abs(lastPos.magnitude - currentPos.magnitude);
                    Vector3 _moveDir = new Vector3(lastPos.x - currentPos.x, 0, lastPos.y - currentPos.y).normalized;
                    _moveDir = new Vector3(_moveDir.x * horizontalSpeedModifier, _moveDir.y, _moveDir.z);
                    Vector3 _velocity = _moveDir * Time.fixedDeltaTime * cameraSpeed * _dist * 0.1f;
                    rb.velocity = new Vector3(Mathf.Clamp(_velocity.x, -cameraMaxSpeed, cameraMaxSpeed), 0, Mathf.Clamp(_velocity.z, -cameraMaxSpeed, cameraMaxSpeed));
                }
                else
                {
                    rb.velocity = Vector3.zero;
                    interactionTimer.SetTime(interactionCancelTime);
                }
            }
            else
            {
                rb.velocity = Vector3.zero;
                currentPos = lastPos = Vector3.zero;
                if(interactionTimer.finished)
                {
                    interactionTimer.SetTime(interactionCancelTime);
                }
            }
        }

        private void HandleMobileTouchInputs()
        {
            if (PlayerManager.Instance.currentController == null)
            {
                return;
            }
            else if (PlayerManager.Instance.currentController.isInCutscene || InteractionManager.Instance.isInteracting)
            {
                return;
            }

            lastPos = currentPos;
            if (Input.touchCount == 1 && !GameManager.Instance.uiHandler.dialogueUI.isRunningDialogue)
            {
                currentPos = Input.GetTouch(0).position;

                if (currentPos != lastPos && lastPos != Vector3.zero)
                {
                    movedCamera = true;

                    float _dist = Mathf.Abs(lastPos.magnitude - currentPos.magnitude);
                    Vector3 _moveDir = new Vector3(lastPos.x - currentPos.x, 0, lastPos.y - currentPos.y).normalized;
                    _moveDir = new Vector3(_moveDir.x * horizontalSpeedModifier, _moveDir.y, _moveDir.z);
                    Vector3 _velocity = _moveDir * Time.fixedDeltaTime * cameraSpeed * _dist * 0.1f;
                    rb.velocity = new Vector3(Mathf.Clamp(_velocity.x, -cameraMaxSpeed, cameraMaxSpeed), 0, Mathf.Clamp(_velocity.z, -cameraMaxSpeed, cameraMaxSpeed));
                }
                else
                {
                    rb.velocity = Vector3.zero;
                    //currentPos = lastPos = Vector3.zero; //Testing for moving cam during dialog bug

                    interactionTimer.SetTime(interactionCancelTime);
                }
            }
            else
            {
                rb.velocity = Vector3.zero;
                currentPos = lastPos = Vector3.zero;

                if (interactionTimer.finished)
                {
                    interactionTimer.SetTime(interactionCancelTime);
                }
            }
        }

        void OnInteractionTimerEnded()
        {
            movedCamera = false;
        }
    }
}