using UnityEngine;
using Hub.Interaction;
using Rendering.Hub;

namespace GameManagement
{
    public class HubInputHandler : MonoBehaviour
    {
        [SerializeField] Camera hubCamera;

        private void Update()
        {
            OnTap();
        }

        void OnTap(/*InputType _input*/)
        {
            RaycastHit hit;

            if(Input.GetMouseButtonUp(0) && HubCamTargetController.Instance.isMovingCamera == false)
            {
                Ray ray = hubCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.GetComponent<Interactor>())
                    {
                        StartCoroutine(hit.collider.gameObject.GetComponent<Interactor>().BeginInteraction());
                    }
                }
            }
        }
    }
}