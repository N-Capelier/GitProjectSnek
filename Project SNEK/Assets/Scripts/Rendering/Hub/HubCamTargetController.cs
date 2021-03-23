using UnityEngine;

namespace Rendering.Hub
{
    /// <summary>
    /// Nico
    /// </summary>
    public class HubCamTargetController : Singleton<HubCamTargetController>
    {
        [SerializeField] [Range(0f, 500f)] float cameraSpeed = 20f;

        [SerializeField] Rigidbody rb;

        private void FixedUpdate()
        {
#if UNITY_STANDALONE || UNITY_EDITOR
            HandleStandaloneInputs();
#elif UNITY_ANDROID || UNITY_IOS
            HandleMobileTouchInputs();
#endif
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
                    float _dist = Mathf.Abs(lastPos.magnitude - currentPos.magnitude);
                    Vector3 _moveDir = new Vector3(lastPos.x - currentPos.x, 0, lastPos.y - currentPos.y);
                    rb.velocity = _moveDir.normalized * Time.fixedDeltaTime * cameraSpeed * _dist * 0.1f;
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

        private void HandleMobileTouchInputs()
        {
            lastPos = currentPos;
            if (Input.touchCount > 0)
            {
                currentPos = Input.GetTouch(0).position;

                if (currentPos != lastPos && lastPos != Vector3.zero)
                {
                    float _dist = Mathf.Abs(lastPos.magnitude - currentPos.magnitude);
                    Vector3 _moveDir = new Vector3(lastPos.x - currentPos.x, 0, lastPos.y - currentPos.y);
                    rb.velocity = _moveDir.normalized * Time.fixedDeltaTime * cameraSpeed * _dist * 0.1f;
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