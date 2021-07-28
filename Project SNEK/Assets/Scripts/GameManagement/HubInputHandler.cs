using UnityEngine;
using Hub.Interaction;
using Rendering.Hub;
using Saving;

namespace GameManagement
{
    public class HubInputHandler : MonoBehaviour
    {
        [SerializeField] Camera hubCamera;

        private void Update()
        {
            InteractOnTouch();
        }

        void InteractOnTouch(/*InputType _input*/)
        {
            if (HubCamTargetController.Instance.movedCamera)
                return;

            RaycastHit hit;

            if(Input.GetMouseButtonUp(0))
            {
                Ray ray = hubCamera.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.GetComponent<Interactor>() && SaveManager.Instance.state.tutorialState != 0)
                    {
                        StartCoroutine(hit.collider.gameObject.GetComponent<Interactor>().BeginInteraction());
                    }
                }
            }
        }
    }
}