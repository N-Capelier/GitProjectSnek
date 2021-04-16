using UnityEngine;

namespace Rendering.Hub
{
    /// <summary>
    /// Nico
    /// </summary>
    public class HubCamTargetController : Singleton<HubCamTargetController>
    {
        [SerializeField] [Range(0f, 1000f)] float cameraSpeed = 20f;
        [SerializeField] [Range(1f, 10f)] float horizontalSpeedModifier = 2f;

        [SerializeField] Rigidbody rb;

        [HideInInspector] public byte actions = 0;

        float leftConfiner = -13.95685f;
        float rightConfiner = 11.03282f;
        float topConfiner = 13.97992f;
        float bottomConfiner = -11.68559f;

        public bool isMovingCamera = false;

        private void Start()
        {
            CreateSingleton();
        }

        private void FixedUpdate()
        {
            if (actions > 0)
                return;

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
            lastPos = currentPos;
            if(Input.GetMouseButton(0))
            {
                currentPos = Input.mousePosition;

                if(currentPos != lastPos && lastPos != Vector3.zero)
                {
                    isMovingCamera = true;

                    float _dist = Mathf.Abs(lastPos.magnitude - currentPos.magnitude);
                    Vector3 _moveDir = new Vector3(lastPos.x - currentPos.x, 0, lastPos.y - currentPos.y).normalized;
                    _moveDir = new Vector3(_moveDir.x * horizontalSpeedModifier, _moveDir.y, _moveDir.z);
                    rb.velocity = _moveDir * Time.fixedDeltaTime * cameraSpeed * _dist * 0.1f;
                }
                else
                {
                    isMovingCamera = false;
                    rb.velocity = Vector3.zero;
                }
            }
            else
            {
                isMovingCamera = false;
                rb.velocity = Vector3.zero;
                currentPos = lastPos = Vector3.zero;
            }
        }

        private void HandleMobileTouchInputs()
        {
            lastPos = currentPos;
            if (Input.touchCount == 1)
            {
                currentPos = Input.GetTouch(0).position;

                if (currentPos != lastPos && lastPos != Vector3.zero)
                {
                    float _dist = Mathf.Abs(lastPos.magnitude - currentPos.magnitude);
                    Vector3 _moveDir = new Vector3(lastPos.x - currentPos.x, 0, lastPos.y - currentPos.y).normalized;
                    _moveDir = new Vector3(_moveDir.x * horizontalSpeedModifier, _moveDir.y, _moveDir.z);
                    rb.velocity = _moveDir * Time.fixedDeltaTime * cameraSpeed * _dist * 0.1f;
                }
                else
                {
                    rb.velocity = Vector3.zero;
                }
            }
            else
            {
                rb.velocity = Vector3.zero;
                currentPos = lastPos = Vector3.zero;
            }
        }
    }
}